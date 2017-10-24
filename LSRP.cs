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
using lsrp_gamemode.Misc;
using lsrp_gamemode.Player;

namespace lsrp_gamemode
{
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

            API.consoleOutput(Utils.Sha256("chuj123"));

            // Events
            API.onServerResourceStart += API_onServerResourceStart;
            API.onClientEventTrigger += ClientEvents.API_OnClientEvent;
            API.onPlayerConnected += PlayerClass.OnPlayerConnected;
            API.onPlayerHealthChange += PlayerClass.OnPlayerHealthChangeHandler;
            API.onPlayerDisconnected += PlayerClass.OnPlayerDisconnectedHandler;
            API.onChatMessage += PlayerClass.OnChatMessageHandler;
            API.onChatCommand += PlayerClass.OnChatCommandHandler;
        }

        // OnServerResourceStart
        private void API_onServerResourceStart(string resource)
        {
            
        }
    }
}