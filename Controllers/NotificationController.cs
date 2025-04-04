using Microsoft.AspNetCore.Mvc;
using UsersAndAuth.Data.Dtos;
using UsersAndAuth.Services;

namespace UsersAndAuth.Controllers;

[Route("notifications")]
[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
	[HttpPost("send")]
	public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
	{
		if (request.ReceiverIds == null || request.ReceiverIds.Count == 0)
			return BadRequest("At least one receiver must be specified.");

		var notification = await notificationService.CreateNotificationAsync(request.Sender, request.Message, request.ReceiverIds);
		return Ok(notification);
	}

	[HttpGet("{userId}")]
	public async Task<IActionResult> GetNotifications(string userId)
	{
		var notifications = await notificationService.GetNotificationsForUserAsync(userId);
		return Ok(notifications);
	}

	[HttpPost("mark-as-read")]
	public async Task<IActionResult> MarkAsRead([FromBody] MarkAsReadRequest request)
	{
		await notificationService.MarkAsReadAsync(request.NotificationId, request.UserId);
		return Ok();
	}
}
