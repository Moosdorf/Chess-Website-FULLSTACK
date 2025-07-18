using Microsoft.AspNetCore.SignalR;

namespace ChessServer.Hubs;

public class GameHub : Hub<IGameHub>
{
    public async Task SendMessageToAll(string user, string message)
    {
        await Clients.All.ReceiveMessage(user, message);
    }

}