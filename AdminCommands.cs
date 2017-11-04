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
        [Command("debugpistol")]
        public void cmd_Debugpistol(Client player)
        {
            if (player.getData("admin") > 0)
            {
                API.givePlayerWeapon(player, WeaponHash.Pistol, 200, false, false);
            }
        }

        [Command("goto", "Użycie: /goto [ID Gracza]", GreedyArg = true)]
        public void cmd_Goto(Client player, string input)
        {
            string[] param = input.Split(null);
            if (param.Length != 1)
            {
                API.sendChatMessageToPlayer(player, "Użycie: /goto [ID Gracza]");
                return;
            }

            int player_id = Int32.Parse(param[0]);

            if (player.getData("admin") != 0)
            {
                Client target = PlayerClass.GetPlayerById(player_id);
                if (API.doesEntityExist(target))
                {
                    Vector3 pos = target.position;
                    pos.X += 1;
                    pos.Y += 1;
                    API.setEntityDimension(player, target.dimension);
                    API.setEntityPosition(player, pos);
                }
            }
        }

        [Command("gethere", "Użycie: /gethere [ID Gracza]", GreedyArg = true)]
        public void cmd_Gethere(Client player, string input)
        {
            string[] param = input.Split(null);
            if(param.Length != 1)
            {
                API.sendChatMessageToPlayer(player, "Użycie: /gethere [ID Gracza]");
                return;
            }

            int player_id = Int32.Parse(param[0]);
            Vector3 pos = player.position;
            pos.X += 1;
            pos.Y += 1;

            if (player.getData("admin") != 0)
            {
                Client target = PlayerClass.GetPlayerById(player_id);
                if (API.doesEntityExist(target))
                {
                    API.setEntityDimension(target, player.dimension);
                    API.setEntityPosition(target, pos);
                }
            }
        }

        [Command("sethp", "Użycie: /sethp [ID Gracza] [HP]", GreedyArg = true)]
        public void cmd_Sethp(Client player, string input)
        {
            string[] param = input.Split(null);
            if (param.Length != 2)
            {
                API.sendChatMessageToPlayer(player, "Użycie: /sethp [ID Gracza] [HP]");
                return;
            }

            int player_id = Int32.Parse(param[0]);
            int hp = Int32.Parse(param[1]);
            if (hp > 100) hp = 100;
            if (player.getData("admin") != 0)
            {
                Client target = PlayerClass.GetPlayerById(player_id);
                if (API.doesEntityExist(target))
                {
                    target.health = hp;
                }
            }
        }

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
        [Command("ap", "Użycie: /ap [stworz | usun | nazwa | owner | value1 | value2]", GreedyArg = true)]
        public void cmd_Ap(Client player, string input)
        {
            if(player.getData("admin")>0)
            {
                string[] param = input.Split(null);
                if(param[0]== "stworz") //ap stworz nazwa typ value1 value2 value3
                {
                    if(param.Length!=6)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /ap stworz [nazwa] [typ] [value1] [value2] [value3]");
                        return;
                    }
                    // PRAWDPODOBNIE POWINNO BYC TU <LIST> ITEMS Z METODY CreateItem, chuj wie, pewnie sporo zjebalem kocham cie blint <3
                    PlayerClass pc = player.getData("data");
                    string name = param[1];
                    int type = Convert.ToInt32(param[2]), value1 = Convert.ToInt32(param[3]), value2 = Convert.ToInt32(param[4]) , value3 = Convert.ToInt32(param[5]);
                    Items.Item.CreateItem(player, pc.id, name, type, value1, value2, value3);
                }
            }
        }

        [Command("av", "Użycie: /av [ stworz | usun | debug | przypisz | fix | kolor ]", GreedyArg = true)]
        public void cmd_Av(Client player, string input)
        {
            if (player.getData("admin") > 0)
            {
                string[] param = input.Split(null);
                if(param[0] == "usun")      // av usun idwozu
                {
                    if(param.Length != 2)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av usun [ID pojazdu]");
                        return;
                    }

                    int vehicle_id = Convert.ToInt32(param[1]);
                    NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);

                    if (vehicle.IsNull)
                    {
                        API.sendNotificationToPlayer(player, "Nie znaleziono pojazdu o takim ID.");
                        return;
                    }

                    VehicleClass.DeleteVehicle(vehicle);
                    API.sendNotificationToPlayer(player, "Pomyślnie usunięto pojazd.");
                }
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
                if (param[0] == "kolor")    // av color IDveh IDC1 IDC2
                {
                    if(param.Length != 4)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av kolor [ID wozu] [kolor1] [kolor2]");
                        return;
                    }

                    int vehicle_id = Convert.ToInt32(param[1]);
                    int color1 = Convert.ToInt32(param[2]);
                    int color2 = Convert.ToInt32(param[3]);
                    NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);

                    if(vehicle.IsNull)
                    {
                        API.sendNotificationToPlayer(player, "Nie znaleziono pojazdu o takim ID.");
                        return;
                    }

                    VehicleClass.UpdateVehicleMainColors(vehicle, color1, color2);
                    API.sendNotificationToPlayer(player, "Pomyślnie zmieniono kolory pojazdu.");
                }
                if(param[0] == "fix")    // av fix ID
                {
                    if(param.Length != 2)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av fix [ID pojazdu]");
                        return;
                    }

                    int vehicle_id = Int32.Parse(param[1]);
                    NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);

                    if(vehicle.IsNull)
                    {
                        API.sendNotificationToPlayer(player, "Nie znaleziono pojazdu o takim ID.");
                        return;
                    }

                    API.repairVehicle(vehicle);
                    API.sendNotificationToPlayer(player, "Pomyślnie naprawiono pojazd.");
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
                    API.sendChatMessageToPlayer(i, Config.COLOR_GC, send);
                }
            }
        }
    }
}
