using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;


public class LSRP : Script
{
    // Private statics
    public static Database db = new Database();

    // Random
    private static Random Rnd = new Random();

    // Construct
    public LSRP()
    {
        // MySQL init
        db.Connect();

        API.consoleOutput(Database.myConnectionString);
        API.consoleOutput(Misc.Sha256("test123"));

        // Events
        API.onPlayerConnected += API_onPlayerConnected;
    }

    // OnPlayerConnected
    private void API_onPlayerConnected(Client player)
    {
        API.setEntityDimension(player, -1);
        API.setEntityInvincible(player, true);
        API.freezePlayer(player, true);
        API.sendChatMessageToPlayer(player, "Wpisz /login [hasło] żeby się zalogować!");
    }
}