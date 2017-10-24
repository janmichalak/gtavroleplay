using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lsrp_gamemode.Player;
using GrandTheftMultiplayer.Shared;
using lsrp_gamemode.Vehicles;
using GrandTheftMultiplayer.Shared.Math;

namespace lsrp_gamemode
{
    public class AdminCommands : Script
    {
        [Command("av", "Użycie: /av [ stworz | usun ]", GreedyArg = true)]
        public void cmd_Av(Client player, string input)
        {
            if (player.getData("admin") > 0)
            {
                string[] param = input.Split(null);
                if(param[0] == "stworz")    // av stworz sentinel 0 1
                {
                    if(param.Length != 4)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av stworz [model] [kolor1] [kolor2]");
                        return;
                    }

                    VehicleHash model = API.vehicleNameToModel(param[1]);
                    int col1 = Int32.Parse(param[2]), col2 = Int32.Parse(param[3]);

                    if(model == 0)
                    {
                        API.sendNotificationToPlayer(player, "~r~Nie znaleziono ~w~pojazdu o takim modelu!");
                        return;
                    }

                    Vector3 new_pos = player.position;
                    new_pos.X += 2;
                    new_pos.Y += 2;

                    NetHandle vehicle = VehicleClass.CreateVehicle(model, new_pos, new Vector3(0, 0, 0), col1, col2, player.dimension);
                    API.sendNotificationToPlayer(player, "Pomyślnie utworzono pojazd marki " + param[1]);
                }
                if(param[1] == "usun")
                {

                }
            }
        }

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
