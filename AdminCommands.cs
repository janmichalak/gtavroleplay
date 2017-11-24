using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lsrp_gamemode.Player;
using lsrp_gamemode.Vehicles;
using lsrp_gamemode.Doors;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Misc;

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
            if (param.Length != 1)
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
            if (param.Length != 2)
            {
                API.sendChatMessageToPlayer(player, "Użycie: /setvw [ID Gracza] [ID świata]");
                return;
            }

            int player_id = Int32.Parse(param[0]);
            int vw = Int32.Parse(param[1]);
            if (player.getData("admin") != 0)
            {
                Client target = PlayerClass.GetPlayerById(player_id);
                if (API.doesEntityExist(target))
                {
                    API.sendChatMessageToPlayer(player, player_id.ToString());
                    API.sendChatMessageToPlayer(player, vw.ToString());
                    API.setEntityDimension(target, vw);
                }
            }
        }

		[Command("ad", "Użycie: /ad [stworz | usun | nazwa | model]", GreedyArg = true)]
		public void cmd_Ad(Client player, string input)
		{
			if(player.getData("admin") < 1)
			{
				API.shared.sendNotificationToPlayer(player, "Brak uprawnień!");
				return;
			}

			string[] param = input.Split(null);

			if(param[0] == "stworz") // ad stworz nazwa model
			{
				if(param.Length != 3)
				{
					API.sendChatMessageToPlayer(player, "Użycie: /ad stworz [model] [nazwa]");
					return;
				}

				int model = Int32.Parse(param[1]);
				String name = param[2];

				Location loc = new Location();
				loc.pos = player.position;
				loc.vw = player.dimension;

				DoorManager dm = DoorManager.getInstance();
				Door door = dm.Create(model, loc, name);

				if(door != null)
				{
					API.sendNotificationToPlayer(player, "Pomyślnie utworzono drzwi!");
					return;
				} else
				{
					API.sendNotificationToPlayer(player, "Wystąpił problem podczas tworzenia drzwi!");
					return;
				}
			}
			if(param[0] == "usun")
			{
				int uid = Convert.ToInt32(param[1]);
				if (param.Length != 2)
				{
					API.sendChatMessageToPlayer(player, "Użycie: /ad usun [UID]");
					return;
				}

				DoorManager dm = DoorManager.getInstance();
				Door door = dm.Find(uid);

				if(door == null) { 
					API.sendChatMessageToPlayer(player, "Nie ma drzwi o takim UID.");
					return;
				}
				
				API.shared.consoleOutput("Administrator {0} usunął drzwi {1} (UID:{2})", player.name, door.name, Convert.ToString(uid));
				dm.Delete(door);
				return;
			}
		}

        [Command("ap", "Użycie: /ap [stworz | usun | nazwa | owner | value1 | value2]", GreedyArg = true)]
        public void cmd_Ap(Client player, string input)
        {
            if (player.getData("admin") > 0)
            {
                string[] param = input.Split(null);
                if (param[0] == "stworz") //ap stworz nazwa typ value1 value2 value3
                {
                    if (param.Length != 6)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /ap stworz [nazwa] [typ] [value1] [value2] [value3]");
                        return;
                    }

                    PlayerClass pc = player.getData("data");
                    string name = param[1];
                    int type = Convert.ToInt32(param[2]), value1 = Convert.ToInt32(param[3]), value2 = Convert.ToInt32(param[4]), value3 = Convert.ToInt32(param[5]);
                    Items.Item.Create(player, pc.uid, name, type, value1, value2, value3);
                    API.shared.sendChatMessageToPlayer(player, "Pomyślnie utworzono przedmiot!");
                }
            }
        }

        [Command("av", "Użycie: /av [ stworz | usun | debug | przypisz | fix | kolor | goto | gethere | zaparkuj]", GreedyArg = true)]
        public void cmd_Av(Client player, string input)
        {
            if (player.getData("admin") > 0)
            {
                string[] param = input.Split(null);
                if (param[0] == "usun")      // av usun idwozu
                {
                    if (param.Length != 2)
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
                if (param[0] == "stworz")    // av stworz sentinel 0 1
                {
                    if (param.Length != 4)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av stworz [model] [kolor1] [kolor2]");
                        return;
                    }

                    VehicleHash model = API.vehicleNameToModel(param[1]);
                    int col1 = Int32.Parse(param[2]), col2 = Int32.Parse(param[3]);

                    if (model == 0)
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
                    if (param.Length != 4)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av kolor [ID wozu] [kolor1] [kolor2]");
                        return;
                    }

                    int vehicle_id = Convert.ToInt32(param[1]);
                    int color1 = Convert.ToInt32(param[2]);
                    int color2 = Convert.ToInt32(param[3]);
                    NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);

                    if (vehicle.IsNull)
                    {
                        API.sendNotificationToPlayer(player, "Nie znaleziono pojazdu o takim ID.");
                        return;
                    }

                    VehicleClass.UpdateVehicleMainColors(vehicle, color1, color2);
                    API.sendNotificationToPlayer(player, "Pomyślnie zmieniono kolory pojazdu.");
                }
                if (param[0] == "fix")    // av fix ID
                {
                    if (param.Length != 2)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av fix [ID pojazdu]");
                        return;
                    }

                    int vehicle_id = Int32.Parse(param[1]);
                    NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);

                    if (vehicle.IsNull)
                    {
                        API.sendNotificationToPlayer(player, "Nie znaleziono pojazdu o takim ID.");
                        return;
                    }

                    API.repairVehicle(vehicle);
                    API.sendNotificationToPlayer(player, "Pomyślnie naprawiono pojazd.");
                }
                if (param[0] == "przypisz")  // av przypisz gracz 1 1
                {
                    if (param.Length != 4)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av przypisz [gracz/grupa] [ID Wozu] [ID Wlasciciela]");
                        return;
                    }

                    string towho = param[1];
                    int vehicle_id = Int32.Parse(param[2]);
                    int owner_id = Int32.Parse(param[3]);

                    if (towho == "gracz")
                    {
                        Client target = PlayerClass.GetPlayerById(owner_id);
                        NetHandle vehicle = VehicleClass.GetVehicleById(vehicle_id);

                        if (target.IsNull || vehicle.IsNull || !target.getData("logged"))
                        {
                            API.sendChatMessageToPlayer(player, "Błąd: Wystąpił błąd podczas przypisywania pojazdu.");
                            return;
                        }

                        VehicleClass vc = API.getEntityData(vehicle, "data");
                        PlayerClass pc = target.getData("data");
                        vc.ownertype = Config.OWNER_PLAYER;
                        vc.owner = pc.uid;

                        VehicleClass.UpdateVehicleOwner(vc.uid, vc.ownertype, vc.owner);
                        API.sendChatMessageToPlayer(player, String.Format("Pomyślnie przepisano pojazd {0} graczowi {1} [{2}]", API.getVehicleDisplayName(vc.model), pc.displayName, pc.id));
                    }
                    if (towho == "grupa")
                    {
                        /// TODO
                    }
                }
                if (param[0] == "debug")
                {
                    NetHandle vehicle = API.getPlayerVehicle(player);
                    if (!vehicle.IsNull)
                    {
                        API.sendChatMessageToPlayer(player, String.Format("Paliwo: {0}", API.getVehicleFuelLevel(vehicle)));
                        API.sendChatMessageToPlayer(player, String.Format("Olej: {0}", API.getVehicleOilLevel(vehicle)));
                        API.sendChatMessageToPlayer(player, String.Format("HP i EngineHP: {0}, {1}", API.getVehicleHealth(vehicle), API.getVehicleEngineHealth(vehicle)));

                    }
                }

                if (param[0] == "goto")
                {
                    if (param.Length != 2)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av goto [ID]");
                        return;
                    }
                    if (VehicleClass.GetVehicleById(Convert.ToInt32(param[1])).IsNull)
                    {
                        API.sendChatMessageToPlayer(player, "Nie ma pojazdu o takim ID.");
                        return;
                    }
                    NetHandle vehicle = VehicleClass.GetVehicleById(Convert.ToInt32(param[1]));
                    Vector3 new_pos = API.getEntityPosition(vehicle);
                    new_pos.X += 2;
                    new_pos.Y += 2;
                    API.setEntityDimension(player, API.getEntityDimension(vehicle));
                    API.setEntityPosition(player, new_pos);
                }

                if (param[0] == "gethere")
                {
                    if (param.Length != 2)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av gethere [ID]");
                        return;
                    }
                    if (VehicleClass.GetVehicleById(Convert.ToInt32(param[1])).IsNull)
                    {
                        API.sendChatMessageToPlayer(player, "Nie ma pojazdu o takim ID.");
                        return;
                    }
                    NetHandle vehicle = VehicleClass.GetVehicleById(Convert.ToInt32(param[1]));
                    Vector3 new_pos = player.position;
                    new_pos.X += 2;
                    new_pos.Y += 2;
                    API.setEntityDimension(vehicle, player.dimension);
                    API.setEntityPosition(vehicle, new_pos);
                }

                if (param[0] == "zaparkuj")
                {
                    if (param.Length != 2)
                    {
                        API.sendChatMessageToPlayer(player, "Użycie: /av zaparkuj [ID]");
                        return;
                    }
                    if (VehicleClass.GetVehicleById(Convert.ToInt32(param[1])).IsNull)
                    {
                        API.sendChatMessageToPlayer(player, "Nie ma pojazdu o takim ID.");
                        return;
                    }
                    NetHandle vehicle = VehicleClass.GetVehicleById(Convert.ToInt32(param[1]));
                    VehicleClass.ParkVehicle(vehicle);
                    API.sendNotificationToPlayer(player, "Przeparkowałeś pojazd o ID: " + param[1]);
                }
            }
        }

        [Command("gc", "Użycie: /gc [wiadomość]", GreedyArg = true)]
        public void cmd_Gc(Client player, string msg)
        {
            string send = String.Format("(( {0} [{1}]: {2} ))", player.getData("globalname"), player.getData("data").id, msg);
            foreach (var i in API.getAllPlayers())
            {
                if (i.getData("admin") > 0 || i.getData("admin") < 0)
                {
                    API.sendChatMessageToPlayer(i, Config.COLOR_GC, send);
                }
            }
        }
           [Command("vehmod")]
           public void cmd_Vehmod(Client player)
           {
            VehicleHash model = (VehicleHash)API.getEntityModel(API.getPlayerVehicle(player));
            System.IO.StreamWriter file = new System.IO.StreamWriter(@API.getVehicleDisplayName(model)+".txt");
            foreach (var modtype in API.getVehicleValidMods(model))
               {
                 file.WriteLine("MODTYPE"+Convert.ToString(modtype.Key));
                   foreach (var values in modtype.Value)
                   {
                       file.WriteLine("MOD KEY: "+Convert.ToString(values.Key) + " " + values.Value);
                   }
               }
           }
        [Command("oc", "Użycie: /oc [ID]", GreedyArg =true)]
        public void cmd_Oc(Client player, int model)
        {
            Vector3 pos = API.getEntityPosition(player);
            Vector3 rot = new Vector3 (0, 0, 0);
            var entityToAttach = API.createObject(-2054442544, new Vector3(), new Vector3(), 0);
            API.attachEntityToEntity(entityToAttach, player, "IK_L_Hand", new Vector3(0.08, 0.06, 0.03), new Vector3(180, 0, 90));
            /*API.createObject(model, pos, rot);
            if(Config.DEBUG_MODE==true)
            {
                API.shared.consoleOutput("[debug] Administrator " + Convert.ToString(player.nametag) + " stworzył obiekt: " + Convert.ToString(model));
            }*/
        }
    }
}
