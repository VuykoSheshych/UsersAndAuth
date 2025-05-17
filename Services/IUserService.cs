using ChessShared.Dtos;
using UsersAndAuth.Data.Models;

namespace UsersAndAuth.Services;

public interface IUserService
{
	Task<List<User>> GetUsersAsync();
	Task<User?> GetUserByIdAsync(string userId);
	Task<User?> GetUserByUserNameAsync(string userName);
	UserDto CreateUserDto(User user);
	Task UpdateUserAsync(UserDto userDto);
	Task SaveFeedback(Feedback feedback);
	Task<List<Feedback>> GetFeedbacksAsync();
}