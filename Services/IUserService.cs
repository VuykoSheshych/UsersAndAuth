using UserService.Models;

namespace UserService.Services;

public interface IUserService
{
	Task<List<User>> GetUsersAsync();
	Task<User?> GetUserByIdAsync(string userId);
	Task<User?> GetUserByUserNameAsync(string userName);
}