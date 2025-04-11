using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersAndAuth.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UsersAndAuth.Data.Models;
using UsersAndAuth.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using ChessShared.Models;

var builder = WebApplication.CreateBuilder(args);

string dbConnection;
string frontendUrl;
JwtOptions jwtOptions;

if (builder.Environment.IsDevelopment())
{
	// У випадку запуску проєкту в середовищі розробки використовується підключення через localhost
	dbConnection = builder.Configuration.GetConnectionString("PostgresLocalhostConnection")!;
	jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>()!;
	frontendUrl = builder.Configuration.GetSection("Urls").GetValue<string>("Frontend")!;
	// When running the project in a development environment, connection via localhost are used
}
else
{
	// В інших випадках використовується змінна середовища
	dbConnection = Environment.GetEnvironmentVariable("USERS_DB_CONNECTION")!;
	jwtOptions = JsonSerializer.Deserialize<JwtOptions>(Environment.GetEnvironmentVariable("JWT_OPTIONS")!)!;
	frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")!;
	// In other cases, environment variable are used
}

// В проєкті використовується PostgreSQL та "ліниве завантаження"
builder.Services.AddDbContext<UserDbContext>(options =>
	options.UseLazyLoadingProxies().UseNpgsql(dbConnection));
// The project uses PostgreSQL and lazy loading

builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<UserDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtOptions.Issuer,
		ValidAudience = jwtOptions.Audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
	};

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			if (context.Request.Cookies.TryGetValue("jwt-token", out var token))
			{
				context.Token = token;
			}
			return Task.CompletedTask;
		}
	};
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddPolicy("FrontendPolicy", policy =>
	{
		policy
			.WithOrigins(frontendUrl)
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
	dbContext.Database.Migrate();
}

app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();