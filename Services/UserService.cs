using Microsoft.EntityFrameworkCore;
using UsersAndAuth.Data;
using UsersAndAuth.Models;

namespace UsersAndAuth.Services;

public class UserService(UserDbContext context) : IUserService
{
	private readonly UserDbContext _context = context;
	public async Task<List<User>> GetUsersAsync()
	{
		return await _context.Users.ToListAsync();
	}
	public async Task<User?> GetUserByIdAsync(string userId)
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
	}
	public async Task<User?> GetUserByUserNameAsync(string userName)
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
	}
}