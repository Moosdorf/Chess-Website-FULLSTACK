using DataLayer.DataServices;
using DataLayer.Entities.Chess;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.HubServices;

public class GameSession
{

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Player1 { get; set; } = null!;
    public string? Player2 { get; set; } = null;
    public string WhitePlayer { get; private set; } = string.Empty;
    public string BlackPlayer { get; private set; } = string.Empty;
    public bool IsReady => Player2 != null;


    public bool Initialize()
    {
        if (Player2 == null) return false;

        // Assign white/black randomly
        if (new Random().Next(2) == 0)
        {
            WhitePlayer = Player1;
            BlackPlayer = Player2;
        }
        else
        {
            WhitePlayer = Player2;
            BlackPlayer = Player1;
        }


        return true;
    }

}
