using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Vehicles;

namespace lsrp_gamemode.Player
{
    public class ClientEvents : Script
    {
        public static void API_OnClientEvent(Client player, string eventName, params object[] arguments) //arguments param can contain multiple params
        {
            if (eventName == "menu_handler_select_item") // ON MENU CLICK
            {
                string callbackId = arguments[0].ToString(); 
                if(callbackId == "character_selected") // SELECT CHARACTER ON LOGIN
                {
                    string char_name = arguments[1].ToString();
                    string char_id = arguments[2].ToString();

                    Database.GetCharacterData(player, Int32.Parse(char_id));
                    API.shared.consoleOutput("Gracz " + char_name.ToString() + " (UID: " + char_id.ToString() + ") zalogował się.");
                }
            }

            if (eventName == "vehicle_selected_item")
            {
                int vehicle_uid = Int32.Parse(arguments[0].ToString());
                NetHandle vehicle = VehicleClass.GetVehicleByUid(vehicle_uid);

                if (vehicle.IsNull)
                {
                    VehicleClass.LoadVehicle(vehicle_uid);
                    API.shared.sendChatMessageToPlayer(player, "Pomyślnie zespawnowano pojazd.");
                }
                else
                {
                    VehicleClass.UnloadVehicle(vehicle);
                    API.shared.sendChatMessageToPlayer(player, "Pomyślnie odspawnowano pojazd.");
                }

                API.shared.triggerClientEvent(player, "hide_menu");
            }

            if(eventName == "vehicle_select_item")
            {
                API.shared.triggerClientEvent(player, "hide_menu");
                API.shared.triggerClientEvent(player, "vehicle_selected");
            }

            if(eventName == "start_stop_engine") // START STOP ENGINE
            {
                VehicleClass.StartStopEngine(player);
            }
        }
    }
}
