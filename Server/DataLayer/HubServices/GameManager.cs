using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.HubServices;
using DataLayer.Models.Chess;
using System.Text.Json;

public interface IGameManager
{
    GameSession? FindWaitingGame();
    GameSession CreateGame(string player);
    GameSession JoinGame(string gameId, string player);
    string? GetSessionId(string username);
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

    public GameSession JoinGame(string gameId, string player)
    {
        var game = _games.First(g => g.Id == gameId);
        game.Player2 = player;
        game.Initialize();
        return game;
    }

    public string? GetSessionId(string username)
    {
        var session = _games.FirstOrDefault(x => (x.Player1 ==  username || x.Player2 == username));

        if (session != null) return session.Id;

        return null;
    }

    private GameSession? GetSession(string username)
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
