using Microsoft.AspNetCore.Mvc;
using UsersAndAuth.Services;

namespace UsersAndAuth.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService UserService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetUsersAsync()
	{
		var users = await UserService.GetUsersAsync();

		if (users is null || users.Count == 0) return BadRequest("No users found");

		return Ok(users);
	}

	[HttpGet("id-{id}")]
	public async Task<IActionResult> GetUserById(string id)
	{
		var user = await UserService.GetUserByIdAsync(id);

		if (user is null) return BadRequest("No user found");

		return Ok(user);
	}

	[HttpGet("username-{userName}")]
	public async Task<IActionResult> GetUserByUsername(string userName)
	{
		var user = await UserService.GetUserByUserNameAsync(userName);

		if (user is null) return BadRequest("No user found");

		return Ok(user);
	}
}