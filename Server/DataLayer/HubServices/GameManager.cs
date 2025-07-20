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

}
