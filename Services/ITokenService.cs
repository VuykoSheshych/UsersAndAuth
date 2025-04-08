using UsersAndAuth.Data.Models;

namespace UsersAndAuth.Services;

public interface ITokenService
{
	string GenerateToken(User user);
}