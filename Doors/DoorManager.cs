using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Player;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Managers;

namespace lsrp_gamemode.Doors
{
    public class DoorManager : Script
    {
        // Load all doors
        public static void LoadDoors()
        {
            API.shared.consoleOutput("[load] Wczytuje drzwi...");
            Database.command.CommandText = String.Format("SELECT * FROM doors");
            Database.Reader = Database.command.ExecuteReader();

            var r = Database.Reader;
            while (r.Read())
            {
                Door door = new Door();
                door.id = Door.GetFreeID();
                door.uid = r.GetInt32("uid");
                door.owner = r.GetInt32("owner");
                door.ownertype = r.GetInt32("ownertype");
                door.name = r.GetString("name");
                door.enterx = r.GetFloat("enterx");
                door.entery = r.GetFloat("entery");
                door.enterz = r.GetFloat("enterz");
                door.exitx = r.GetFloat("exitx");
                door.exity = r.GetFloat("exity");
                door.exitz = r.GetFloat("exitz");
                door.entervw = r.GetInt32("entervw");
                door.exitvw = r.GetInt32("exitvw");
                door.enterangle = r.GetFloat("enterangle");
                door.exitangle = r.GetFloat("exitangle");
                door.markerType = r.GetInt32("markertype");
                door.colr = r.GetInt32("colr");
                door.colg = r.GetInt32("colg");
                door.colb = r.GetInt32("colb");
                door.alpha = r.GetInt32("alpha");
                door.marker = API.shared.createMarker(door.markerType, new Vector3(door.enterx, door.entery, door.enterz),
                    new Vector3(), new Vector3(), new Vector3(1, 1, 1), door.alpha, door.colr, door.colg, door.colb, door.entervw);

                SphereColShape entershape = API.shared.createSphereColShape(new Vector3(door.enterx, door.entery, door.enterz), 2f);
                Door.DoorSpheres.Add(entershape, door);
                Door.DoorList.Add(door);
            }

            Database.Reader.Close();
            API.shared.consoleOutput("[load] Załadowano " + Door.DoorList.Count + " drzwi.");
        }
    }
}
