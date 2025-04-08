using ChessShared.Dtos;
using ChessShared.Models;
using Microsoft.AspNetCore.Identity;

namespace UsersAndAuth.Services;

public interface IAuthService
{
	Task<IdentityResult> RegisterAsync(RegisterDto model);
	Task<bool> LoginAsync(LoginDto model);
	Task LogoutAsync();
	Task<CurrentUser> GetCurrentUserAsync();
}