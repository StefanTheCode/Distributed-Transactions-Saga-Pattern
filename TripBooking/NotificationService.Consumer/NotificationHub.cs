using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Consumer;
public class NotificationHub : Hub
{
    public async override Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Connected", "connected");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}