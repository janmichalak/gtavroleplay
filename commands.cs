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
using lsrp_gamemode.Vehicles;

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

        [Command("v", "Użycie: /v [ lista | zamknij | zaparkuj ]", GreedyArg = true)]
        public void cmd_V(Client player, string input)
        {
            string[] param = input.Split(null);

            if(param[0] == "lista")
            {
                Dictionary<int, int> vehicles = VehicleClass.ListPlayerVehicles(player);
                if(vehicles.Count > 0)
                {
                    string json = API.toJson(vehicles);
                    API.triggerClientEvent(player, "vehicle_select", json);
                }
                else
                {
                    API.sendNotificationToPlayer(player, "Nie posiadasz żadnego pojazdu.");
                    return;
                }
            }
            if(param[0] == "zaparkuj")
            {
                if(!API.isPlayerInAnyVehicle(player))
                {
                    API.sendNotificationToPlayer(player, "Nie znajdujesz się w żadnym pojeździe.");
                    return;
                }

                NetHandle vehicle = API.getPlayerVehicle(player);
                VehicleClass vc = API.getEntityData(vehicle, "data");

                // funkcja do sprawdzenia czy gracz ma permy do ustawiania takich rzeczy
                VehicleClass.ParkVehicle(vehicle);
                API.sendNotificationToPlayer(player, "Pomyślnie zaparkowano pojazd.");
            }
        }

        [Command("myid")]
        public void cmd_Myid(Client player)
        {
            PlayerClass pc = player.getData("data");
            API.shared.sendChatMessageToPlayer(player, "Moje ID: " + pc.id.ToString());
        }

        [Command("dl")]
        public void cmd_Dl(Client player)
        {
            bool dl = player.getData("dl");
            if(dl == true)
            {
                API.triggerClientEvent(player, "toggle_dl", false);
            }
            else
            {
                API.triggerClientEvent(player, "toggle_dl", true);
            }
            player.setData("dl", !dl);
        }

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

        [Command("silnik")]
        public void cmd_Silnik(Client player)
        {
            VehicleClass.StartStopEngine(player);
        }

        [Command("login", "Użycie: /login [hasło]", SensitiveInfo = true, GreedyArg = true)]
        public void cmd_Login(Client player, string password)
        {
            if (player.getData("logged") == true)
            {
                API.sendChatMessageToPlayer(player, "Jesteś już zalogowany.");
                return;
            }
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
                API.sendChatMessageToPlayer(p, Config.COLOR_ME, "** " + player.getData("data").displayName + " " + action);
            }
        }

        [Command("do", "Użycie: /do [akcja]", GreedyArg = true)]
        public void cmd_do(Client player, string action)
        {
            List<Client> playerList = API.getPlayersInRadiusOfPlayer(10, player);
            foreach (Client p in playerList)
            {
                API.sendChatMessageToPlayer(p, Config.COLOR_DO, "** " + action + " (( " + player.getData("data").displayName + " ))");
            }
        }

        [Command("b", "Użycie: /b [wiadomość]", GreedyArg = true)]
        public void cmd_b(Client player, string msg)
        {
            List<Client> playerList = API.getPlayersInRadiusOfPlayer(10, player);
            foreach (Client p in playerList)
            {
                API.sendChatMessageToPlayer(p, Config.COLOR_B, "(( " + player.getData("globalname") + " [" + player.getData("data").id + "]: " + msg + " ))");
            }
        }

        [Command("w", "Użycie: /w [ID] [wiadomość]", GreedyArg = true)]
        public void cmd_w(Client player, int id, string msg)
        {
            Client target = PlayerClass.GetPlayerById(id);
            if (target != null && PlayerClass.GetPlayerID(player) == id)
            {
                API.sendChatMessageToPlayer(player, "~#ffc973~", "(( " + target.getData("globalname") + " [" + PlayerClass.GetPlayerID(target.getData("data").id) + "]: " + msg + " ))");
                API.sendChatMessageToPlayer(target, "~#fdae33~", "(( " + player.getData("globalname") + " [" + PlayerClass.GetPlayerID(player) + "]: " + msg + " ))");
            }
        }
    }
}