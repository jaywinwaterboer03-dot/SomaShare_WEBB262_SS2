using Microsoft.EntityFrameworkCore;

namespace SomaShare.Data.Services
{
    /// <summary>
    /// Service for managing user notifications
    /// </summary>
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false);
        Task<int> GetUnreadCountAsync(string userId);
        Task CreateNotificationAsync(string userId, string title, string message, string? type = null, int? relatedEntityId = null);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(string userId);
        Task DeleteNotificationAsync(int notificationId);
        Task ClearOldNotificationsAsync(int daysOld = 30);
    }

    public class NotificationService(ApplicationDbContext context) : INotificationService
    {
        public async Task<List<Notification>> GetUserNotificationsAsync(string userId, bool unreadOnly = false)
        {
            var query = context.Notifications
                .Where(n => n.UserId == userId);

            if (unreadOnly)
                query = query.Where(n => !n.IsRead);

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(string userId) =>
            await context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);

        public async Task CreateNotificationAsync(string userId, string title, string message, string? type = null, int? relatedEntityId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                RelatedEntityId = relatedEntityId,
                CreatedAt = DateTime.UtcNow
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }

            if (notifications.Count > 0)
                await context.SaveChangesAsync();
        }

        public async Task DeleteNotificationAsync(int notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                context.Notifications.Remove(notification);
                await context.SaveChangesAsync();
            }
        }

        public async Task ClearOldNotificationsAsync(int daysOld = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            var oldNotifications = await context.Notifications
                .Where(n => n.CreatedAt < cutoffDate)
                .ToListAsync();

            if (oldNotifications.Count > 0)
            {
                context.Notifications.RemoveRange(oldNotifications);
                await context.SaveChangesAsync();
            }
        }
    }
}
