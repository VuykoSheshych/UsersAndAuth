namespace UsersAndAuth.Data.Dtos;

public record MarkAsReadRequest(string UserId, Guid NotificationId);