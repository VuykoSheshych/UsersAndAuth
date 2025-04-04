using UsersAndAuth.Models;

namespace UsersAndAuth.Services;

public interface INotificationService
{
	Task<Notification> CreateNotificationAsync(string sender, string message, List<string> receiverIds);
	Task MarkAsReadAsync(Guid notificationId, string receiverId);
	Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId);
}
