using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lsrp_gamemode.Vehicles
{
    public class VehicleClass
    {
        #region Vehicle Fields
        public int id = -1;
        public int uid = 0;

        public int color1 = 0;
        public int color2 = 0;

        public int owner = 0;
        public int ownertype = Config.VEHICLE_OWNER_NONE;

        public VehicleHash model = 0;
        #endregion

        #region Database CRUD
        /// <summary>
        /// Create Vehicle
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <param name="vw"></param>
        /// <returns></returns>
        public static NetHandle CreateVehicle(VehicleHash model, Vector3 pos, Vector3 rot, int col1, int col2, int vw = 1)
        {
            NetHandle vehicle = API.shared.createVehicle(model, pos, rot, col1, col2, vw);
            API.shared.setVehicleEngineStatus(vehicle, false);

            VehicleClass vc = new VehicleClass();
            vc.id = GetFreeID();
            vc.model = model;
            vc.color1 = col1;
            vc.color2 = col2;
            vc.ownertype = Config.VEHICLE_OWNER_NONE;
            vc.owner = 0;

            // Insert to SQL
            Database.command.CommandText = "INSERT INTO vehicles (veh_model, veh_posx, veh_posy, veh_posz, veh_rotx, veh_roty, veh_rotz, veh_col1, veh_col2, veh_vw) ";
            Database.command.CommandText += String.Format("VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", 
                (int)model, pos.X.ToString().Replace(",", "."), pos.Y.ToString().Replace(",", "."), pos.Z.ToString().Replace(",", "."), 
                rot.X.ToString().Replace(",", "."), rot.Y.ToString().Replace(",", "."), rot.Z.ToString().Replace(",", "."), col1, col2, vw);
            Database.command.ExecuteNonQuery();

            vc.uid = (int)Database.command.LastInsertedId;
            API.shared.consoleOutput(String.Format("Utworzono pojazd marki {0} ID:{1}, UID:{2}", model, vc.id, vc.uid));

            API.shared.setVehicleNumberPlate(vehicle, String.Format("LS {0}", vc.uid));
            API.shared.setEntityData(vehicle, "data", vc);
            API.shared.setEntitySyncedData(vehicle, "id", vc.id);
            return vehicle;
        }

        /// <summary>
        /// Load all vehicles on start
        /// </summary>
        public static void LoadVehicles()
        {
            API.shared.consoleOutput("[load] Rozpoczynam wczytywanie pojazdów...");
            Database.command.CommandText = String.Format("SELECT * FROM vehicles WHERE veh_ownertype != {0}", Config.VEHICLE_OWNER_PLAYER);
            Database.Reader = Database.command.ExecuteReader();

            var r = Database.Reader; int loaded = 0;
            while(r.Read())
            {
                Vector3 pos = new Vector3(r.GetFloat("veh_posx"), r.GetFloat("veh_posy"), r.GetFloat("veh_posz"));
                Vector3 rot = new Vector3(r.GetFloat("veh_rotx"), r.GetFloat("veh_roty"), r.GetFloat("veh_rotz"));
                NetHandle vehicle = API.shared.createVehicle(r.GetInt32("veh_model"), pos, rot, r.GetInt32("veh_col1"), r.GetInt32("veh_col2"), r.GetInt32("veh_vw"));
                API.shared.setVehicleEngineStatus(vehicle, false);

                VehicleClass vc = new VehicleClass();
                vc.id = GetFreeID();
                vc.uid = r.GetInt32("veh_id");
                vc.model = (VehicleHash)r.GetInt32("veh_model");
                vc.color1 = r.GetInt32("veh_col1");
                vc.color2 = r.GetInt32("veh_col2");
                vc.ownertype = r.GetInt32("veh_ownertype");
                vc.owner = r.GetInt32("veh_owner");

                API.shared.setVehicleNumberPlate(vehicle, String.Format("LS {0}", vc.uid));
                API.shared.setEntityData(vehicle, "data", vc);
                API.shared.setEntitySyncedData(vehicle, "id", vc.id);
                loaded += 1;
            }

            Database.Reader.Close();
            API.shared.consoleOutput("[load] Załadowano " + loaded + " pojazdów.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="veh_uid"></param>
        /// <param name="ownertype"></param>
        /// <param name="owner"></param>
        public static void UpdateVehicleOwner(int veh_uid, int ownertype, int owner)
        {
            Database.command.CommandText = String.Format("UPDATE vehicles SET veh_owner = {0}, veh_ownertype = {1} WHERE veh_id = {2}", owner, ownertype, veh_uid);
            Database.command.ExecuteNonQuery();
        }
        #endregion

        /// <summary>
        /// Change engine state
        /// </summary>
        /// <param name="player"></param>
        public static void StartStopEngine(Client player)
        {
            if (API.shared.isPlayerInAnyVehicle(player))
            {
                bool can = false;
                NetHandle vehicle = API.shared.getPlayerVehicle(player);
                if (player.getData("admin") > 0)
                {
                    can = true;
                }
                else
                {

                }

                if (can)
                {
                    API.shared.setVehicleEngineStatus(vehicle, !API.shared.getVehicleEngineStatus(vehicle));
                    API.shared.consoleOutput(String.Format("[debug] Gracz {0} {1} pojazd ID: {2}", player.getData("data").name, (API.shared.getVehicleEngineStatus(vehicle) == true ? "odpala" : "gasi"), GetVehicleID(vehicle)));
                }
            }
        }

        #region Vehicle ID System
        /// <summary>
        /// Return NetHandle vehicle or netHandle.IsNull
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NetHandle GetVehicleById(int id)
        {
            foreach (var v in API.shared.getAllVehicles())
            {
                try
                {
                    VehicleClass vc = API.shared.getEntityData(API.shared.getEntityFromHandle<Vehicle>(v), "data");
                    if (vc.id == id)
                    {
                        return v;
                    }
                }
                catch { }
            }
            return new NetHandle();
        }

        public static int GetVehicleID(NetHandle vehicle)
        {
            VehicleClass vc = API.shared.getEntityData(vehicle, "data");
            return vc.id;
        }

        public static int GetFreeID()
        {
            for (var i = 0; i < API.shared.getAllVehicles().Count; i++)
            {
                if (GetVehicleById(i).IsNull)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
    }
}
