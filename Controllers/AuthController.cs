using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Data.Dtos;
using UserService.Models;

namespace UserService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
{
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto model)
	{
		var user = new User { UserName = model.UserName, Email = model.Email };
		var result = await userManager.CreateAsync(user, model.Password);

		if (!result.Succeeded) return BadRequest(result.Errors);

		return Ok("User registered successfully");
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto model)
	{
		var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
		if (!result.Succeeded) return Unauthorized("Invalid login attempt");

		return Ok("Logged in successfully");
	}

	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await signInManager.SignOutAsync();
		return Ok("Logged out successfully");
	}
}