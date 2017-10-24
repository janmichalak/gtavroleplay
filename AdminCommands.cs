using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lsrp_gamemode.Player;

namespace lsrp_gamemode
{
    public class AdminCommands : Script
    {
        [Command("gc", "Użycie: /gc [wiadomość]", GreedyArg = true)]
        public void cmd_Gc(Client player, string msg)
        {
            string send = String.Format("(( {0} [{1}]: {2} ))", player.getData("globalname"), player.getData("data").id, msg);
            foreach(var i in API.getAllPlayers())
            {
                if(i.getData("admin") > 0 || i.getData("admin") < 0)
                {
                    API.sendChatMessageToPlayer(i, Config.COLOR_DARKBLUE, send);
                }
            }
        }
    }
}
