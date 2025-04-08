using ChessShared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsersAndAuth.Data.Models;

namespace UsersAndAuth.Services;

public class JwtTokenService(JwtOptions jwtOptions) : ITokenService
{
	public string GenerateToken(User user)
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: jwtOptions.Issuer,
			audience: jwtOptions.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationMinutes),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}