using ChessShared.Dtos;
using ChessShared.Models;
using Microsoft.AspNetCore.Identity;
using UsersAndAuth.Data.Models;

namespace UsersAndAuth.Services;

public class JwtAuthService(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : IAuthService
{
	public async Task<IdentityResult> RegisterAsync(RegisterDto model)
	{
		var user = new User { UserName = model.UserName, Email = model.Email };
		var result = await userManager.CreateAsync(user, model.Password);

		if (!result.Succeeded) return result;

		await signInManager.SignInAsync(user, isPersistent: false);
		return result;
	}
	public async Task<bool> LoginAsync(LoginDto model)
	{
		var user = await userManager.FindByNameAsync(model.UserName);
		if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
			return false;

		var token = tokenService.GenerateToken(user);

		httpContextAccessor.HttpContext!.Response.Cookies.Append("jwt-token", token, new CookieOptions
		{
			HttpOnly = true,
			Secure = true,
			SameSite = SameSiteMode.None,
			Expires = DateTimeOffset.UtcNow.AddHours(1)
		});

		await signInManager.SignInAsync(user, isPersistent: false);
		return true;
	}
	public async Task LogoutAsync()
	{
		await signInManager.SignOutAsync();
		httpContextAccessor.HttpContext?.Response.Cookies.Delete("jwt-token");
	}
	public async Task<CurrentUser> GetCurrentUserAsync()
	{
		var user = await userManager.GetUserAsync(signInManager.Context.User);
		return new CurrentUser
		{
			IsAuthenticated = signInManager.Context.User.Identity?.IsAuthenticated ?? false,
			UserName = user?.UserName ?? string.Empty,
			Claims = signInManager.Context.User.Claims.ToDictionary(c => c.Type, c => c.Value)
		};
	}
}