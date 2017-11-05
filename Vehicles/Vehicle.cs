using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lsrp_gamemode.Player;

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

        public string plate;

        public int vw = 0;

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

            API.shared.setVehicleFuelLevel(vehicle, 50.0f);
            API.shared.setVehicleOilLevel(vehicle, 2.5f);
            API.shared.setVehicleNumberPlate(vehicle, String.Format("LS {0}", vc.uid));
            API.shared.setEntityData(vehicle, "data", vc);
            API.shared.setEntitySyncedData(vehicle, "id", vc.id);
            return vehicle;
        }

        /// <summary>
        /// Delete vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public static void DeleteVehicle(NetHandle vehicle)
        {
            VehicleClass vc = API.shared.getEntityData(vehicle, "data");
            Database.command.CommandText = String.Format("DELETE FROM vehicles WHERE veh_id = {0}", vc.uid);
            Database.command.ExecuteNonQuery();

            API.shared.deleteEntity(vehicle);
        }

        /// <summary>
        /// Unload vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public static void UnloadVehicle(NetHandle vehicle)
        {
            // Get damages
            int tyre_0 = Convert.ToInt32(API.shared.isVehicleTyrePopped(vehicle, 0));
            int tyre_1 = Convert.ToInt32(API.shared.isVehicleTyrePopped(vehicle, 1));
            int tyre_2 = Convert.ToInt32(API.shared.isVehicleTyrePopped(vehicle, 2));
            int tyre_3 = Convert.ToInt32(API.shared.isVehicleTyrePopped(vehicle, 3));
            int tyre_4 = Convert.ToInt32(API.shared.isVehicleTyrePopped(vehicle, 4));
            int tyre_5 = Convert.ToInt32(API.shared.isVehicleTyrePopped(vehicle, 5));
            string tyres = String.Format("{0},{1},{2},{3},{4},{5}", tyre_0, tyre_1, tyre_2, tyre_3, tyre_4, tyre_5);

            VehicleClass vc = API.shared.getEntityData(vehicle, "data");
            Database.command.CommandText = String.Format("UPDATE vehicles SET veh_fuel = '{0}', veh_hp = '{1}', veh_oil = '{2}', veh_tyres = '{3}' WHERE veh_id = {4}",
                API.shared.getVehicleFuelLevel(vehicle),
                API.shared.getVehicleHealth(vehicle),
                API.shared.getVehicleOilLevel(vehicle),
                tyres,
                vc.uid);
            Database.command.ExecuteNonQuery();
            API.shared.deleteEntity(vehicle);
        }

        /// <summary>
        /// Load vehicle by uid
        /// </summary>
        /// <param name="uid"></param>
        public static void LoadVehicle(int uid)
        {
            API.shared.consoleOutput("[load] Wczytuję pojazd uid: " + uid);
            Database.command.CommandText = String.Format("SELECT * FROM vehicles WHERE veh_id = {0}", uid);
            Database.Reader = Database.command.ExecuteReader();

            var r = Database.Reader;
            while (r.Read())
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
                vc.vw = r.GetInt32("veh_vw");
                vc.plate = r.GetString("veh_plate");
                #region VehicleDamage_OnLoad
                string tyres = r.GetString("veh_tyres");
                string[] tyre = tyres.Split(',');

                API.shared.popVehicleTyre(vehicle, 0, (tyre[0] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 1, (tyre[1] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 2, (tyre[2] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 3, (tyre[3] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 4, (tyre[4] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 5, (tyre[5] == "1" ? true : false));
                #endregion
                if (String.IsNullOrEmpty(vc.plate) == true)
                {
                    API.shared.setVehicleNumberPlate(vehicle, String.Format("LS {0}", vc.uid));
                }
                else
                {
                    API.shared.setVehicleNumberPlate(vehicle, String.Format("LS {0}", vc.plate));
                }
                API.shared.setVehicleLocked(vehicle, true);
                API.shared.setVehicleHealth(vehicle, r.GetFloat("veh_hp"));
                API.shared.setVehicleFuelLevel(vehicle, r.GetFloat("veh_fuel"));
                API.shared.setVehicleOilLevel(vehicle, r.GetFloat("veh_oil"));
                API.shared.setEntityData(vehicle, "data", vc);
                API.shared.setEntitySyncedData(vehicle, "id", vc.id);
                API.shared.setEntityDimension(vehicle, vc.vw);
            }

            Database.Reader.Close();
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
            while (r.Read())
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
                vc.vw = r.GetInt32("veh_vw");
          

                #region VehicleDamage_OnLoad
                string tyres = r.GetString("veh_tyres");
                string[] tyre = tyres.Split(',');

                API.shared.popVehicleTyre(vehicle, 0, (tyre[0] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 1, (tyre[1] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 2, (tyre[2] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 3, (tyre[3] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 4, (tyre[4] == "1" ? true : false));
                API.shared.popVehicleTyre(vehicle, 5, (tyre[5] == "1" ? true : false));
                #endregion

                API.shared.setVehicleHealth(vehicle, r.GetFloat("veh_hp"));
                API.shared.setVehicleFuelLevel(vehicle, r.GetFloat("veh_fuel"));
                API.shared.setVehicleOilLevel(vehicle, r.GetFloat("veh_oil"));
                API.shared.setVehicleNumberPlate(vehicle, String.Format("LS {0}", vc.uid));
                API.shared.setEntityData(vehicle, "data", vc);
                API.shared.setEntitySyncedData(vehicle, "id", vc.id);
                API.shared.setEntityDimension(vehicle, vc.vw);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static Dictionary<int, int> ListPlayerVehicles(Client player)
        {
            PlayerClass pc = player.getData("data");
            Database.command.CommandText = String.Format("SELECT * FROM vehicles WHERE veh_ownertype = {0} AND veh_owner = {1}", Config.VEHICLE_OWNER_PLAYER, pc.uid);
            Database.Reader = Database.command.ExecuteReader();

            var r = Database.Reader;
            Dictionary<int, int> vehicles = new Dictionary<int, int>();
            while (r.Read())
            {
                vehicles.Add(r.GetInt32("veh_id"), r.GetInt32("veh_model"));
            }
            Database.Reader.Close();
            return vehicles;
        }

        /// <summary>
        /// Park Vehicle
        /// </summary>
        /// <param name="vehicle"></param>
        public static void ParkVehicle(NetHandle vehicle)
        {
            Vector3 pos = API.shared.getEntityPosition(vehicle);
            Vector3 rot = API.shared.getEntityRotation(vehicle);
            VehicleClass vc = API.shared.getEntityData(vehicle, "data");
            Database.command.CommandText = String.Format("UPDATE vehicles SET veh_posx = '{0}', veh_posy = '{1}', veh_posz = '{2}', veh_rotx = '{3}', veh_roty = '{4}', veh_rotz = '{5}', veh_vw = '{6}' WHERE veh_id = {7}",
                pos.X.ToString().Replace(",", "."), pos.Y.ToString().Replace(",", "."), pos.Z.ToString().Replace(",", "."),
                rot.X.ToString().Replace(",", "."), rot.Y.ToString().Replace(",", "."), rot.Z.ToString().Replace(",", "."),
                vc.vw ,vc.uid);
            Database.command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update vehicle main colors
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        public static void UpdateVehicleMainColors(NetHandle vehicle, int color1, int color2)
        {
            VehicleClass vc = API.shared.getEntityData(vehicle, "data");
            vc.color1 = color1;
            vc.color2 = color2;

            Database.command.CommandText = String.Format("UPDATE vehicles SET veh_col1 = {0}, veh_col2 = {1} WHERE veh_id = {2}", color1, color2, vc.uid);
            Database.command.ExecuteNonQuery();

            API.shared.setVehiclePrimaryColor(vehicle, color1);
            API.shared.setVehicleSecondaryColor(vehicle, color2);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static NetHandle GetNearestVehicle(Vector3 pos, float radius)
        {
            float current = radius;
            NetHandle vehicle = new NetHandle();
            foreach (var i in API.shared.getAllVehicles())
            {
                float distance = API.shared.getEntityPosition(i).DistanceTo(pos);
                if (distance < current)
                {
                    current = distance;
                    vehicle = i;
                }
            }

            return vehicle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<NetHandle> GetVehiclesInRadiusOfPos(Vector3 pos, float radius)
        {
            List<NetHandle> vehicles = new List<NetHandle>();
            foreach (var i in API.shared.getAllVehicles())
            {
                if (API.shared.getEntityPosition(i).DistanceTo(pos) <= radius)
                {
                    vehicles.Add(i);
                }
            }

            return vehicles;
        }

        /// <summary>
        /// Czy ma uprawnienia do pojazdu
        /// </summary>
        /// <param name="player"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public static bool IsPlayerHasPermForVehicle(Client player, NetHandle vehicle)
        {
            PlayerClass pc = player.getData("data");
            VehicleClass vc = API.shared.getEntityData(vehicle, "data");
            if (vc.ownertype == Config.VEHICLE_OWNER_NONE && player.getData("admin") > 0)
                return true;
            if (vc.ownertype == Config.VEHICLE_OWNER_PLAYER && (player.getData("admin") > 0 || vc.owner == pc.uid))
                return true;
            if (vc.ownertype == Config.VEHICLE_OWNER_GROUP && (player.getData("admin") > 0))
                return true;
            return false;
        }

        /// <summary>
        /// Change engine state
        /// </summary>
        /// <param name="player"></param>
        public static void StartStopEngine(Client player)
        {
            if (API.shared.isPlayerInAnyVehicle(player))
            {
                NetHandle vehicle = API.shared.getPlayerVehicle(player);

                if (!IsPlayerHasPermForVehicle(player, vehicle))
                {
                    // throw error
                    return;
                }

                API.shared.setVehicleEngineStatus(vehicle, !API.shared.getVehicleEngineStatus(vehicle));
                if (Config.DEBUG_MODE == true)
                {
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

        public static NetHandle GetVehicleByUid(int uid)
        {
            foreach (var v in API.shared.getAllVehicles())
            {
                try
                {
                    VehicleClass vc = API.shared.getEntityData(API.shared.getEntityFromHandle<Vehicle>(v), "data");
                    if (vc.uid == uid)
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
