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
        [Command("setvw", "Użycie: /setvw [ID Gracza] [ID świata]", GreedyArg = true)]
        public void cmd_Setvw(Client player, string input)
        {
            string[] param = input.Split(null);
            if(param.Length != 2)
            {
                API.sendChatMessageToPlayer(player, "Użycie: /setvw [ID Gracza] [ID świata]");
                return;
            }

            int player_id = Int32.Parse(param[0]);
            int vw = Int32.Parse(param[1]);
            if(player.getData("admin") != 0)
            {
                Client target = PlayerClass.GetPlayerById(player_id);
                if(API.doesEntityExist(target))
                {
                    API.sendChatMessageToPlayer(player, player_id.ToString());
                    API.sendChatMessageToPlayer(player, vw.ToString());
                    API.setEntityDimension(target, vw);
                }
            }
        }

        [Command("av", "Użycie: /av [ stworz | usun | debug | przypisz ]", GreedyArg = true)]
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
                if(param[0] == "przypisz")  // av przypisz gracz 1 1
                {
                    if (param.Length != 4)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av przypisz [gracz/grupa] [ID Wozu] [ID Wlasciciela]");
                        return;
                    }

                    string towho = param[1];
                    int vehicle_id = Int32.Parse(param[2]);
                    int owner_id = Int32.Parse(param[3]);

                    if(towho == "gracz")
                    {
                        Client target = PlayerClass.GetPlayerById(owner_id);
                        NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);
                        
                        if(target.IsNull || vehicle.IsNull || !target.getData("logged"))
                        {
                            API.sendChatMessageToPlayer(player, "Błąd: Wystąpił błąd podczas przypisywania pojazdu.");
                            return;
                        }

                        VehicleClass vc = API.getEntityData(vehicle, "data");
                        PlayerClass pc = target.getData("data");
                        vc.ownertype = Config.VEHICLE_OWNER_PLAYER;
                        vc.owner = pc.uid;

                        VehicleClass.UpdateVehicleOwner(vc.uid, vc.ownertype, vc.owner);
                        API.sendChatMessageToPlayer(player, String.Format("Pomyślnie przepisano pojazd {0} graczowi {1} [{2}]", API.getVehicleDisplayName(vc.model), pc.displayName, pc.id));
                    }
                    if(towho == "grupa")
                    {
                        /// TODO
                    }
                }
                if(param[0] == "debug")
                {
                    NetHandle vehicle = API.getPlayerVehicle(player);
                    if(!vehicle.IsNull)
                    {
                        API.sendChatMessageToPlayer(player, String.Format("Paliwo: {0}", API.getVehicleFuelLevel(vehicle)));
                        API.sendChatMessageToPlayer(player, String.Format("Olej: {0}", API.getVehicleOilLevel(vehicle)));
                        API.sendChatMessageToPlayer(player, String.Format("HP i EngineHP: {0}, {1}", API.getVehicleHealth(vehicle), API.getVehicleEngineHealth(vehicle)));
                        
                    }
                }
                if(param[0] == "usun")
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
