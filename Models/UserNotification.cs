namespace UsersAndAuth.Models;

public class UserNotification
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public required string ReceiverId { get; set; }
	public virtual User Receiver { get; set; } = null!;
	public bool IsRead { get; set; } = false;

	public Guid NotificationId { get; set; }
	public virtual Notification Notification { get; set; } = null!;
}