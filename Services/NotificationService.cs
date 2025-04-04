using Microsoft.EntityFrameworkCore;
using UsersAndAuth.Data;
using UsersAndAuth.Models;

namespace UsersAndAuth.Services;

public class NotificationService(UserDbContext _context) : INotificationService
{
	public async Task<Notification> CreateNotificationAsync(string sender, string message, List<string> receiverIds)
	{
		var notification = new Notification
		{
			Sender = sender,
			Message = message,
			CreatedAt = DateTime.UtcNow
		};

		_context.Notifications.Add(notification);
		await _context.SaveChangesAsync();

		foreach (var receiverId in receiverIds)
		{
			var userNotification = new UserNotification
			{
				ReceiverId = receiverId,
				NotificationId = notification.Id
			};
			_context.UserNotifications.Add(userNotification);
		}

		await _context.SaveChangesAsync();
		return notification;
	}

	public async Task MarkAsReadAsync(Guid notificationId, string receiverId)
	{
		var userNotification = await _context.UserNotifications
			.FirstOrDefaultAsync(un => un.NotificationId == notificationId && un.ReceiverId == receiverId);

		if (userNotification != null)
		{
			userNotification.IsRead = true;
			await _context.SaveChangesAsync();
		}
	}

	public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId)
	{
		var userNotifications = await _context.UserNotifications
			.Where(un => un.ReceiverId == userId)
			.Include(un => un.Notification)
			.OrderByDescending(un => un.Notification.CreatedAt)
			.ToListAsync();

		return userNotifications.Select(un => un.Notification);
	}
}
