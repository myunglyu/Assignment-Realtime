using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace SignalRProject.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            string username;
            if (Context.User.Identity.IsAuthenticated)
                username = Context.User.Identity.Name;
            else
                username = "Guest";
            ConnectedUsers.TryAdd(Context.ConnectionId, username);
            await Clients.All.SendAsync("UserConnected", username);
            await Clients.All.SendAsync("UpdateUserList", ConnectedUsers.Values);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (ConnectedUsers.TryRemove(Context.ConnectionId, out string? username))
            {
                await Clients.All.SendAsync("UserDisconnected", username);
                await Clients.All.SendAsync("UpdateUserList", ConnectedUsers.Values);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetConnectedUsers()
        {
            await Clients.Caller.SendAsync("UpdateUserList", ConnectedUsers.Values);
        }
    }
}