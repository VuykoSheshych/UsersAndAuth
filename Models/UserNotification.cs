namespace UserService.Models;

public class UserNotification
{
	public required string ReceiverId { get; set; }
	public virtual User Receiver { get; set; } = null!;
	public bool IsRead { get; set; } = false;

	public Guid NotificationId { get; set; }
	public virtual Notification Notification { get; set; } = null!;
}