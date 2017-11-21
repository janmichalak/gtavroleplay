using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lsrp_gamemode.Doors
{
    public class Door
    {
        // Door List
        public static List<Door> DoorList = new List<Door>();
        public static Dictionary<ColShape, Door> DoorSpheres = new Dictionary<ColShape, Door>();

        #region fields
        public int uid = 0;

        public int owner = 0;
        public int ownertype = Config.OWNER_NONE;

        public String name = "Drzwi";

        public float enterx = 0f;
        public float entery = 0f;
        public float enterz = 0f;

        public float exitx = 0f;
        public float exity = 0f;
        public float exitz = 0f;

        public int entervw = 1;
        public int exitvw = 1;

        public float enterangle = 0f;
        public float exitangle = 0f;

        public Marker marker = null;
        public int markerType = 0;

        public int colr = 153;
        public int colg = 255;
        public int colb = 51;
        public int alpha = 255;

        //public SphereColShape entershape = null;
        //public SphereColShape exitshape = null;
        #endregion

        #region methods
        public static Door Create(int type, String name, Vector3 pos, int vw)
        {
            Door door = new Door();
            Database.command.CommandText = "INSERT INTO doors (name, enterx, entery, enterz, entervw, markertype) ";
            Database.command.CommandText += String.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                name, pos.X.ToString().Replace(",", "."), pos.Y.ToString().Replace(",", "."), pos.Z.ToString().Replace(",", "."), vw, type);
            Database.command.ExecuteNonQuery();
            door.uid = (int)Database.command.LastInsertedId;
            door.name = name;
            door.enterx = pos.X;
            door.entery = pos.Y;
            door.enterz = pos.Z;
            door.entervw = vw;
            door.markerType = type;
            Marker marker = API.shared.createMarker(type, pos, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1),
            door.alpha, door.colr, door.colg, door.colb, vw);
            door.marker = marker;
            SphereColShape entershape = API.shared.createSphereColShape(new Vector3(door.enterx, door.entery, door.enterz), 2f);
            Door.DoorSpheres.Add(entershape, door);
            DoorList.Add(door);
            return door;
        }
        public static void Delete(Door door)
        {
            Database.command.CommandText = String.Format("DELETE FROM doors WHERE uid ={0}", door.uid);
            Database.command.ExecuteNonQuery();
            var item = Door.DoorSpheres.First(kvp => kvp.Value == door);
            Door.DoorSpheres.Remove(item.Key);
            API.shared.deleteEntity(door.marker);
            DoorList.Remove(door);
        }
        public static Door GetDoorByID(int id)
        {
            foreach (var door in DoorList)
            {
                if (door.uid == id)
                {
                    return door;
                }
            }
            return null;
        }
        #endregion
    }
}
