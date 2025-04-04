namespace UsersAndAuth.Models;

public class UserFriend
{
	public string UserId { get; set; } = null!;
	public virtual User User { get; set; } = null!;

	public string FriendId { get; set; } = null!;
	public virtual User Friend { get; set; } = null!;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}