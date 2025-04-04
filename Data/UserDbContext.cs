using Microsoft.EntityFrameworkCore;
using UserService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UserService.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<User>(options)
{
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<UserNotification> UserNotifications { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<UserNotification>()
			.HasKey(un => new { un.ReceiverId, un.NotificationId });

		modelBuilder.Entity<UserNotification>()
			.HasOne(un => un.Receiver)
			.WithMany(u => u.UserNotifications)
			.HasForeignKey(un => un.ReceiverId);

		modelBuilder.Entity<UserNotification>()
			.HasOne(un => un.Notification)
			.WithMany(n => n.UserNotifications)
			.HasForeignKey(un => un.NotificationId);
	}
}