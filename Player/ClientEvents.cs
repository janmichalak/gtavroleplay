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
using lsrp_gamemode.Items;

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

            if(eventName == "item_selected_item")
            {
                int item_uid = Int32.Parse(arguments[0].ToString());
                int idx = Int32.Parse(arguments[1].ToString());

                if(item_uid > 0) 
                {
                    if (idx == 0)    // use
                        Items.Item.Use(player, item_uid);
                    if (idx == 1)    // drop
                        Items.Item.Drop(player, item_uid);

                }
                API.shared.triggerClientEvent(player, "hide_menu");
            }

            if(eventName == "item_select_item")
            {
                API.shared.triggerClientEvent(player, "hide_menu");
                API.shared.triggerClientEvent(player, "item_selected");
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

            if(eventName == "client_p")
            {
                Commands.cmd_P(player, "lista");
            }

            if(eventName == "client_e")
            {
                if(API.shared.isPlayerInAnyVehicle(player))
                {

                } else
                {
                    // Check is any free object in range.
                    Item item = Item.GetItemInRangeOfPlayer(player);
                    if(item != null)
                    {
                        PlayerClass pc = API.shared.getEntityData(player, "data");
                        item.place = Config.PLACE_ITEM_PLAYER;
                        item.owner = pc.uid;
                        item.posz = 0f;
                        item.posx = 0f;
                        item.posy = 0f;

                        Item.FloorItems.Remove(item);
                        Item.PlayerItems[player.handle].Add(item);
             
                        Commands.cmd_me(player, String.Format("podnosi przedmiot {0}.", item.name));
                        Item.Save(item.uid, (Config.ITEM_SAVE_OWNER | Config.ITEM_SAVE_POS));

                        API.shared.playPlayerAnimation(player, (int)(Config.AnimationFlags.AllowPlayerControl), "anim@mp_snowball", "pickup_snowball");
                        NetHandle label = API.shared.getEntityData(item.obj, "label");
                        API.shared.deleteEntity(label);
                        API.shared.deleteEntity(item.obj);
                        return;
                    }
                }
            }
        }
    }
}
