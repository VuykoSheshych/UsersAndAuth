using Microsoft.AspNetCore.Identity;

namespace UsersAndAuth.Models;

public class User : IdentityUser
{
	public virtual List<UserFriend> Friends { get; set; } = [];
	public virtual List<UserNotification> UserNotifications { get; set; } = [];

	public byte[]? Avatar { get; set; }
	public int EloRating { get; set; } = 2200;
}