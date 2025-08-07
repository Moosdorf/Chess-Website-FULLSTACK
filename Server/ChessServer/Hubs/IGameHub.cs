using DataLayer.Models.Chess;

/// <summary>
/// Interface defining SignalR hub methods for the chess game.
/// </summary>
public interface IGameHub
{
    /// <summary>
    /// Called when a user or system sends a chat message to the group.
    /// </summary>
    /// <param name="user">The username of the sender.</param>
    /// <param name="message">The chat message.</param>
    Task ReceiveMessage(string user, string message);

    /// <summary>
    /// Broadcasts the latest chess state to all clients in the group.
    /// </summary>
    /// <param name="chessState">Current chess state object.</param>
    Task ReceiveMove(ChessModel chessState);

    /// <summary>
    /// Notifies the client that the game is waiting for an opponent.
    /// </summary>
    Task WaitingForOpponent();

    /// <summary>
    /// Notifies clients that the game is ready and provides the initial game state.
    /// </summary>
    /// <param name="chessState">Initial chess game state object.</param>
    Task GameReady(ChessModel chessState);

    /// <summary>
    /// Notifies the client of a bad or invalid move attempt.
    /// </summary>
    /// <param name="message">Error message describing the problem.</param>
    Task BadMove(string message);

    /// <summary>
    /// Notifies the client that the queue has been stopped.
    /// </summary>
    /// <param name="message">Queue stopped message.</param>
    Task QueueStopped(string message);

    /// <summary>
    /// Notifies the client that the game has ended.
    /// </summary>
    /// <param name="chessState">State of the last position.</param>
    Task EndGame(ChessModel chessState);
}
