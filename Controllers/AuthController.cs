using ChessShared.Dtos;
using ChessShared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAndAuth.Services;

namespace UsersAndAuth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto model)
	{
		var result = await authService.RegisterAsync(model);

		if (!result.Succeeded)
			return BadRequest(result.Errors.FirstOrDefault()?.Description);

		var loginSuccess = await authService.LoginAsync(new LoginDto(model.UserName, model.Password));

		return loginSuccess
			? Ok(new { message = "Registered and logged in." })
			: BadRequest("Registration succeeded, but login failed.");
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto model)
	{
		var result = await authService.LoginAsync(model);

		return result
			? Ok(new { message = "Login successful" })
			: BadRequest("Invalid login attempt.");
	}

	[Authorize]
	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await authService.LogoutAsync();
		return Ok(new { message = "Logout successful" });
	}

	[HttpGet("currentuserinfo")]
	public async Task<CurrentUser> GetCurrentUserInfo()
	{
		return await authService.GetCurrentUserAsync();
	}
}