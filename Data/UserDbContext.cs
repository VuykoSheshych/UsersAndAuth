using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UsersAndAuth.Data.Models;

namespace UsersAndAuth.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<User>(options)
{
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<UserNotification> UserNotifications { get; set; }
	public DbSet<UserFriend> UserFriends { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<UserNotification>()
			.HasKey(un => un.Id);

		modelBuilder.Entity<UserNotification>()
			.HasOne(un => un.Receiver)
			.WithMany(u => u.UserNotifications)
			.HasForeignKey(un => un.ReceiverId);

		modelBuilder.Entity<UserNotification>()
			.HasOne(un => un.Notification)
			.WithMany(n => n.UserNotifications)
			.HasForeignKey(un => un.NotificationId);

		modelBuilder.Entity<UserFriend>()
			.HasKey(uf => new { uf.UserId, uf.FriendId });

		modelBuilder.Entity<UserFriend>()
			.HasOne(uf => uf.User)
			.WithMany(u => u.Friends)
			.HasForeignKey(uf => uf.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<UserFriend>()
			.HasOne(uf => uf.Friend)
			.WithMany()
			.HasForeignKey(uf => uf.FriendId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}