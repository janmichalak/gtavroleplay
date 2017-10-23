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
using lsrp_gamemode.Player;

namespace lsrp_gamemode
{
    public class Commands : Script
    {
        public Commands()
        {
        }

        /**
         * COMMANDS
         **/

        [Command("pos")]
        public void cmd_Pos(Client player)
        {
            Vector3 playerPosition = API.getEntityPosition(player);
            API.sendChatMessageToPlayer(player, "X: " + playerPosition.X + " Y: " + playerPosition.Y + " Z: " + playerPosition.Z);
        }

        [Command("qs")]
        public void cmd_Qs(Client player)
        {
            Login.LSRP_DisconnectPlayer(player, true);
        }

        [Command("login", "Użycie: /login [hasło]", SensitiveInfo = true, GreedyArg = true)]
        public void cmd_Login(Client player, string password)
        {
            // Check login
            if (!Database.loginPlayer(player, password, API))
            {
                API.sendChatMessageToPlayer(player, "Podałeś błędne hasło!");
                return;
            }
        }
        [Command("me", "Użycie: /me [akcja]", GreedyArg = true)]
        public void cmd_me(Client player, string action)
        {
            List<Client> playerList = API.getPlayersInRadiusOfPlayer(10, player);
            foreach (Client p in playerList)
            {
                API.sendChatMessageToPlayer(p, Config.COLOR_ME, "** " + player.name + " " + action);
            }
        }
        [Command("do", "Użycie: /do [akcja]", GreedyArg = true)]
        public void cmd_do(Client player, string action)
        {
            List<Client> playerList = API.getPlayersInRadiusOfPlayer(10, player);
            foreach (Client p in playerList)
            {
                API.sendChatMessageToPlayer(p, Config.COLOR_DO, "** " + action + " (( " + player.name + " ))");
            }
        }
    }
}