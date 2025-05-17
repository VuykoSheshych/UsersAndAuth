using ChessShared.Dtos;
using Microsoft.AspNetCore.Mvc;
using UsersAndAuth.Data.Models;
using UsersAndAuth.Services;

namespace UsersAndAuth.Controllers;

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

		return Ok(userService.CreateUserDto(user));
	}

	[HttpPost("update-user/{userId}")]
	public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserDto userDto)
	{
		if (userId != userDto.Id) return BadRequest("User ID mismatch");

		await userService.UpdateUserAsync(userDto);

		return Ok();
	}

	[HttpPost("feedback")]
	public async Task<IActionResult> SaveFeedback([FromBody] Feedback feedback)
	{
		await userService.SaveFeedback(feedback);
		return Ok();
	}

	[HttpGet("feedbacks")]
	public async Task<IActionResult> GetFeedbacks()
	{
		return Ok(await userService.GetFeedbacksAsync());
	}
}