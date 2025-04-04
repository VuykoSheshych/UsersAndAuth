namespace UsersAndAuth.Data.Dtos;

public record SendNotificationRequest(string Sender, string Message, List<string> ReceiverIds);