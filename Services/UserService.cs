using ChessShared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersAndAuth.Data;
using UsersAndAuth.Data.Models;

namespace UsersAndAuth.Services;

public class UserService(UserManager<User> _userManager) : IUserService
{
	public async Task<List<User>> GetUsersAsync()
	{
		return await _userManager.Users.ToListAsync();
	}
	public async Task<User?> GetUserByIdAsync(string userId)
	{
		return await _userManager.FindByIdAsync(userId);
	}
	public async Task<User?> GetUserByUserNameAsync(string userName)
	{
		return await _userManager.FindByNameAsync(userName);
	}
	public async Task UpdateUserAsync(UserDto userDto)
	{
		var user = await GetUserByIdAsync(userDto.Id);
		if (user is null) return;

		if (userDto.Name != null && !userDto.Name.Equals(user.UserName))
		{
			await _userManager.SetUserNameAsync(user, userDto.Name);
			await _userManager.UpdateNormalizedUserNameAsync(user);
		}

		user.EloRating = userDto.EloRating;

		user.Avatar = userDto.Avatar;
		await _userManager.UpdateAsync(user);
	}
	public UserDto CreateUserDto(User user)
	{
		return new UserDto(user.Id, user.UserName!, user.EloRating, user.Avatar);
	}
}