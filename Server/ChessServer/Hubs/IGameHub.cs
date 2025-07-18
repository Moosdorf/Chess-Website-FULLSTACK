namespace ChessServer.Hubs;

public interface IGameHub
{
    Task ReceiveMessage(string user, string message);
}
