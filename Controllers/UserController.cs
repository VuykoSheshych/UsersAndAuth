using Microsoft.AspNetCore.Mvc;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService userService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetUsersAsync()
	{
		var users = await userService.GetUsersAsync();

		if (users is null || users.Count == 0) return BadRequest("No users found");

		return Ok(users);
	}

	[HttpGet("id-{id}")]
	public async Task<IActionResult> GetUserById(string id)
	{
		var user = await userService.GetUserByIdAsync(id);

		if (user is null) return BadRequest("No user found");

		return Ok(user);
	}

	[HttpGet("username-{userName}")]
	public async Task<IActionResult> GetUserByUsername(string userName)
	{
		var user = await userService.GetUserByUserNameAsync(userName);

		if (user is null) return BadRequest("No user found");

		return Ok(user);
	}
}