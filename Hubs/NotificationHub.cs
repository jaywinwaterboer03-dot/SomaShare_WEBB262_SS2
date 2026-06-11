using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SomaShare.Hubs
{
    /// <summary>
    /// SignalR hub for real-time notifications
    /// </summary>
    [Authorize]
    public class NotificationHub : Hub
    {
        private static readonly Dictionary<string, string> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value ?? Context.User?.FindFirst("nameid")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                if (!UserConnections.ContainsKey(userId))
                {
                    UserConnections[userId] = Context.ConnectionId;
                }
                else
                {
                    UserConnections[userId] = Context.ConnectionId;
                }

                // Add user to a group for their notifications
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst("sub")?.Value ?? Context.User?.FindFirst("nameid")?.Value;

            if (!string.IsNullOrEmpty(userId) && UserConnections.ContainsKey(userId))
            {
                UserConnections.Remove(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Send a notification to a specific user
        /// </summary>
        public async Task SendNotificationToUser(string userId, string title, string message, string? type = null)
        {
            await Clients.Group($"user-{userId}")
                .SendAsync("ReceiveNotification", new { title, message, type, timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Broadcast notification to multiple users
        /// </summary>
        public async Task SendNotificationToUsers(List<string> userIds, string title, string message)
        {
            foreach (var userId in userIds)
            {
                await SendNotificationToUser(userId, title, message);
            }
        }

        /// <summary>
        /// Send to all connected clients
        /// </summary>
        public async Task BroadcastNotification(string title, string message)
        {
            await Clients.All
                .SendAsync("ReceiveBroadcast", new { title, message, timestamp = DateTime.UtcNow });
        }
    }
}
