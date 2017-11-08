using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lsrp_gamemode.Misc;

namespace lsrp_gamemode.Player
{
    public class PlayerClass
    {
        public int id = -1;
        public int uid = 0;
        public PedHash skin = 0;

        public string name = "";
        public string displayName = "";

        public int cash = 0;
        public int bankCash = 0;

        public int vw = 1;
        public int health = 100;

        /// <summary>
        /// Clear cache
        /// </summary>
        /// <param name="player"></param>
        public static void ClearCache(Client player)
        {
            player.setData("admin", 0);
            player.setData("gid", 0);
            player.setData("logged", false);
            player.setData("dl", false);
            player.setData("data", new PlayerClass());
        }

        #region Player ID System
        /// <summary>
        /// Get player by ID (Return Client or null)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Client GetPlayerById(int id)
        {
            foreach(var p in API.shared.getAllPlayers())
            {
                PlayerClass pc = p.getData("data");
                if(pc.id == id)
                {
                    return p;
                }
            }
            return null;
        }

        public static int GetPlayerID(Client player)
        {
            PlayerClass pc = player.getData("data");
            return pc.id;
        }

        public static int GetFreeID()
        {
            for(var i = 0; i < API.shared.getMaxPlayers(); i++)
            {
                if(GetPlayerById(i) == null)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        public static void OnPlayerConnected(Client player)
        {
            ClearCache(player);
            API.shared.setEntityDimension(player, -1);
            API.shared.setEntityInvincible(player, true);
            API.shared.freezePlayer(player, true);
            API.shared.sendChatMessageToPlayer(player, "Wpisz /login [hasło] żeby się zalogować!");
        }

        /// <summary>
        /// API Change Health
        /// </summary>
        /// <param name="player"></param>
        /// <param name="oldValue"></param>
        public static void OnPlayerHealthChangeHandler(Client player, int oldValue)
        {
            try
            {
                PlayerClass p = player.getData("data");
                if(p.uid > 0)
                {
                    p.health = player.health;
                }
            } catch
            {
                return;
            }
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        /// <param name="player"></param>
        /// <param name="reason"></param>
        public static void OnPlayerDisconnectedHandler(Client player, string reason)
        {
            API.shared.consoleOutput(player.name + " has left the server. (" + reason + ")");
        }

        public static void OnChatMessageHandler(Client player, string message, CancelEventArgs e)
        {
            // Roleplay Chat
            Utils.ChatMethod(player, 15, message, Config.COLOR_CHAT1, Config.COLOR_CHAT2, Config.COLOR_CHAT3, Config.COLOR_CHAT4);
            e.Cancel = true;
        }

        public static void OnChatCommandHandler(Client player, string command, CancelEventArgs e)
        {
            if(!command.Contains("/login") && player.getData("logged") == false)
            {
                e.Cancel = true;
            }
        }
    }
}
