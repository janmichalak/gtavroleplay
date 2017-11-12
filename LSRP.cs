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
using lsrp_gamemode.Misc;
using lsrp_gamemode.Player;
using lsrp_gamemode.Vehicles;
using lsrp_gamemode.Items;
using lsrp_gamemode.Doors;
using System.Timers;

namespace lsrp_gamemode
{
    public class LSRP : Script
    {
        // Private statics
        public static Database db = new Database();

        // Random
        private static Random Rnd = new Random();

        // Construct
        public LSRP()
        {
            // MySQL init
            db.Connect();

            // Events
            API.onServerResourceStart += API_onServerResourceStart;
            API.onClientEventTrigger += ClientEvents.API_OnClientEvent;
            API.onPlayerConnected += PlayerClass.OnPlayerConnected;
            API.onPlayerHealthChange += PlayerClass.OnPlayerHealthChangeHandler;
            API.onPlayerDisconnected += PlayerClass.OnPlayerDisconnectedHandler;
            API.onChatMessage += PlayerClass.OnChatMessageHandler;
            API.onChatCommand += PlayerClass.OnChatCommandHandler;
            API.onEntityEnterColShape += OnEntityEnterColShapeHandler;

            // Set world time
            API.setTime(20, 0);

            // Main server timers
            Timer secondTimer = new Timer();
            secondTimer.Elapsed += new ElapsedEventHandler(OnSecondTimer);
            secondTimer.Interval = 1000;
            secondTimer.Enabled = true;

            Timer minuteTimer = new Timer();
            minuteTimer.Elapsed += new ElapsedEventHandler(OnMinuteTimer);
            minuteTimer.Interval = 60000;
            minuteTimer.Enabled = true;

            // Loads
            VehicleClass.LoadVehicles();
            ItemManager.LoadItems();
            DoorManager.LoadDoors();
        }

        // OnMinuteTimer
        private static void OnMinuteTimer(object source, ElapsedEventArgs e)
        {
            // Add minute every minute tick
            TimeSpan time = API.shared.getTime();
            API.shared.setTime(time.Hours, (time.Minutes + 1));
        }

        // OnSecondTimer
        private static void OnSecondTimer(object source, ElapsedEventArgs e)
        {
            List<Client> players = API.shared.getAllPlayers();
            foreach(Client player in players)
            {
                bool logged = API.shared.getEntityData(player, "logged");
                if(logged)
                {
                    // Attach weapons
                    Item.ApplyPlayerAttachedWeapons(player);
                } else
                {
                    
                }
            }
        }

        // OnServerResourceStart
        private void API_onServerResourceStart(string resource)
        {
            
        }

        // OnEntityEnterColShapeHandler
        private void OnEntityEnterColShapeHandler(ColShape shape, NetHandle entity)
        {
            // Player entered colshape
            if (API.getEntityType(entity) == EntityType.Player)
            {
                Client player = API.getPlayerFromHandle(entity); // Fetch the Client
       
                // Is it a door
                if(Door.DoorSpheres.ContainsKey(shape))
                {
                    Door door = Door.DoorSpheres[shape];
                    if(door.entervw == player.dimension)
                    {
                        
                        API.shared.sendNotificationToPlayer(player, String.Format("Jesteœ w drzwiach {0}, wciœnij E aby wejœæ do œrodka.", door.name));
                    }
                }
            }
        }
    }
}