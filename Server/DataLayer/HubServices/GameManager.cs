using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.HubServices;
using DataLayer.Models.Chess;
using System.Text.Json;

public interface IGameManager
{
    GameSession? FindWaitingGame();
    GameSession CreateGame(string player);
    GameSession JoinGameSession(string sessionId, string player);
    GameSession JoinBotGame(string sessionId, ChessGame game, string player);
    string? GetSessionId(string username);
    GameSession? GetSession(string username);
    void RemoveUserFromSession(string username);

}

public class GameManager : IGameManager
{
    private readonly List<GameSession> _games = new();

    public GameSession? FindWaitingGame()
        => _games.FirstOrDefault(g => !g.IsReady);

    public GameSession CreateGame(string player)
    {
        var session = new GameSession { Player1 = player };
        _games.Add(session);
        return session;
    }

    public GameSession JoinGameSession(string sessionId, string player)
    {
        var session = _games.First(g => g.Id == sessionId);
        session.Player2 = player;
        session.Initialize();
        return session;
    }

    public GameSession JoinBotGame(string sessionId, ChessGame game, string player)
    {
        var session = _games.First(g => g.Id == sessionId);
        session.GameId = game.Id;
        session.Player2 = player;
        session.WhitePlayer = game.WhiteUsername;
        session.BlackPlayer = game.BlackUsername;
        return session;
    }

    public string? GetSessionId(string username)
    {
        var session = _games.FirstOrDefault(x => (x.Player1 ==  username || x.Player2 == username));

        if (session != null) return session.Id;

        return null;
    }

    public GameSession? GetSession(string username)
    {
        var session = _games.FirstOrDefault(x => (x.Player1 == username || x.Player2 == username));

        if (session != null) return session;

        return null;
    }

    public void RemoveUserFromSession(string username)
    {
        var sessionId = GetSession(username);
        if (sessionId != null) _games.Remove(sessionId);
    }

}
