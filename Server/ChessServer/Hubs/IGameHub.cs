using DataLayer.Models.Chess;

namespace ChessServer.Hubs;

public interface IGameHub
{
    Task ReceiveMessage(string user, string message);
    Task ReceiveMove(ChessModel chessState);
    Task WaitingForOpponent();
    Task GameReady(ChessModel chessState);
    Task BadMove(string message);
    Task QueueStopped(string message);
}
