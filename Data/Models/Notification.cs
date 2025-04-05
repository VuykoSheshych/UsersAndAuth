namespace UsersAndAuth.Data.Models;

public class Notification
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public required string Sender { get; set; }
	public required string Message { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public virtual List<UserNotification> UserNotifications { get; set; } = [];
}