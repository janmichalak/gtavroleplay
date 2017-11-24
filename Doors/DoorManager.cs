using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Player;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Managers;
using lsrp_gamemode.Misc;
using GrandTheftMultiplayer.Server.Constant;
using MySql.Data.MySqlClient;

namespace lsrp_gamemode.Doors
{
	public class DoorManager : Script
	{
		// Class variables
		private static DoorManager Instance;
		private List<Door> DoorList;
		private Dictionary<Door, ColShape> DoorSpheres;

		// Constructor
		private DoorManager() {
			this.DoorList = new List<Door>();
			this.DoorSpheres = new Dictionary<Door, ColShape>();
		}

		// Singleton implementation
		public static DoorManager getInstance()
		{
			if(Instance == null)
			{
				Instance = new DoorManager();
			}
			return Instance;
		}

		/**
		 * Creates a brand new door.
		 *
		 * @param int type The door marker type
		 * @param Location loc The location of the door enterance.
		 * @param String name The name of the door.
		 * @return Door The instance of the newly created Door. Null if the creation failed.
		 */
		public Door Create(int type, Location loc, String name)
		{
			// Make sure there aren't any prohibited characters in the door name.
			name = Utils.SanitizeString(name);

			// Attempt creating the door in the Database
			Database.command.CommandText = "INSERT INTO doors (name, enterx, entery, enterz, entervw, markertype) ";
			Database.command.CommandText += String.Format("VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
				name, loc.pos.X, loc.pos.Y, loc.pos.Z, loc.vw, type);
			Database.command.ExecuteNonQuery();

			// Store the DB returned UID
			int uid = (int) Database.command.LastInsertedId;

			// Handle bad insert.
			if(uid <= 0)
			{
				return null;
			}

			if(LoadDoor(uid))
			{
				return Find(uid);
			} else
			{
				return null;
			}
		}

		/**
		 * Deletes a particular door entirely from both the world and the database.
		 *
		 * @param Door An instance of the door you're deleting.
		 */
		public void Delete(Door door)
		{
			Database.command.CommandText = String.Format("DELETE FROM doors WHERE uid = {0}", door.uid);
			Database.command.ExecuteNonQuery();

			// Destroy door collision shape
			DoorSpheres.Remove(door);
			// Destroy the marker
			API.shared.deleteEntity(door.marker);
			// Remove the door from the list
			DoorList.Remove(door);
		}

		/**
		 * Deletes a particular door entirely from both the world and the database.
		 *
		 * @param int The database UID of the door.
		 */
		public void Delete(int uid)
		{
			Door door = Find(uid);
			if (door != null)
				Delete(door);
		}

		public void Save(Door door)
		{

		}

		public void Save(int uid)
		{
			Door door = Find(uid);
			if (door != null)
				Save(door);
		}

		/**
		 * Searches for a Door with the particular UID
		 *
		 * @param int uid The database UID of the door.
		 * @return Door An instance of the Door or null if no door with this UID found.
		 */
		public Door Find(int uid)
		{
			foreach(Door door in DoorList)
			{
				if(door.uid == uid)
				{
					return door;
				}
			}

			return null;
		}

		/**
		 * Loads all doors from the DB.
		 */
		public void LoadAllDoors()
		{
			API.shared.consoleOutput("[load] Wczytuje drzwi...");
			Database.command.CommandText = String.Format("SELECT * FROM doors");
			Database.Reader = Database.command.ExecuteReader();

			var r = Database.Reader;
			while (r.Read())
			{
				ParseDoorFromDBData(r);
			}

			Database.Reader.Close();
			API.shared.consoleOutput("[load] Załadowano " + DoorList.Count + " drzwi.");
		}

		/**
		 * Loads a single door identified by the given DB UID
		 *
		 * @param int uid The database UID of the door.
		 * @return bool Indicates whether the loading process was successful.
		 */
		public bool LoadDoor(int uid)
		{
			API.shared.consoleOutput("[load] Wczytuje drzwi o UID: {0}", uid);
			Database.command.CommandText = String.Format("SELECT * FROM doors WHERE uid = {0} LIMIT 1", uid);
			Database.Reader = Database.command.ExecuteReader();

			MySqlDataReader r = Database.Reader;
			r.Read();

			bool result = ParseDoorFromDBData(r);

			Database.Reader.Close();
			return result;
		}

		/**
		 * Parses a reader object and calls the instantiation method to create a door.
		 *
		 * @param MySqlDataReader r An instance of MySQLDataReader which has all the door DB fields.
		 * @return bool Indicates whether the door was created correctly.
		 */
		private bool ParseDoorFromDBData(MySqlDataReader r)
		{
			// Get the UID, type and name
			int uid = r.GetInt32("uid");
			int type = r.GetInt32("markertype");
			String name = r.GetString("name");

			// Get the enterance of the door
			Location enter = new Location();
			enter.pos.X = r.GetFloat("enterx");
			enter.pos.Y = r.GetFloat("entery");
			enter.pos.Z = r.GetFloat("enterz");
			enter.vw = r.GetInt32("entervw");
			enter.angle = r.GetFloat("enterangle");

			// Get the exit of the door
			Location exit = new Location();
			exit.pos.X = r.GetFloat("exitx");
			exit.pos.Y = r.GetFloat("exity");
			exit.pos.Z = r.GetFloat("exitz");
			exit.vw = r.GetInt32("exitvw");
			exit.angle = r.GetFloat("exitangle");

			// Get the marker color of the door
			Color color = new Color();
			color.red = r.GetInt32("colr");
			color.green = r.GetInt32("colg");
			color.blue = r.GetInt32("colb");
			color.alpha = r.GetInt32("alpha");

			// Attempt to instantiate a door object
			Door door = InstantiateDoor(uid, type, enter, exit, color, name);

			// Handle doors that couldn't be instantiated.
			if (door == null)
			{
				API.shared.consoleOutput("[error] Nie udalo sie stworzyc instacji drzwi. UID: {0}", uid);
				return false;
			}

			// Set any other non-crucial door settings.
			door.owner = r.GetInt32("owner");
			door.ownertype = r.GetInt32("ownertype");

			return true;
		}

		/**
		 * Creates an actual instance of a Door and adds it to the pool of doors.
		 *
		 * @param int uid The database UID of the door.
		 * @param int type The marker type of the door.
		 * @param Location enter The location of the enterance to the door.
		 * @param Location exit The location of the exit of this door.
		 * @param Color color The color of the marker.
		 * @param String name The name of the door.
		 * @return Door In instance of the newly created door or null if creation failed.
		 */
		private Door InstantiateDoor(int uid, int type, Location enter, Location exit, Color color, String name)
		{
			// Prevent double instantiation
			foreach (Door d in DoorList)
			{
				if (d.uid == uid)
				{
					return null;
				}
			}

			// Create the Door object
			Door door = new Door(uid);
			door.name = name;
			door.enter = enter;
			door.exit = exit;
			door.color = color;
			door.markerType = type;

			// Create the door marker
			door.marker = API.shared.createMarker(type, enter.pos, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1),
				door.color.alpha, door.color.red, door.color.green, door.color.blue, enter.vw);

			// Create the collision shape
			SphereColShape entershape = API.shared.createSphereColShape(enter.pos, 2f);
			DoorSpheres.Add(door, entershape);

			// Add this door the list of all doors and return with the UID
			DoorList.Add(door);
			return door;
		}

		// *** STATIC *** //

		/**
		 * Finds a Door instance by the given database UID.
		 * Note: This is a static shortcut to the DoorManager.Find() method.
		 *
		 * @param int uid The database UID of the door.
		 * @return Door An instance of the Door or null if no door with this UID found.
		 */
		public static Door GetDoor(int uid)
		{
			DoorManager instance = getInstance();
			return instance.Find(uid);
		}

		/**
		 * Finds a Door instance that's closest to the given Location.
		 *
		 * NOTE: This method will perform a VW check unless loc.vw is set to ~0.
		 * 
		 * @param Location loc The location to which we're trying to find the closest door.
		 * @param float distance_limit The maximum distance a door can be away from the location. Defaults to infinity.
		 * @return Door An instance of the Door or null if none found.
		 */
		public static Door GetDoor(Location loc, float distance_limit = float.MaxValue)
		{
			DoorManager instance = getInstance();
			Door closest_door = null;
			float smallest_distance = float.MaxValue;

			foreach (Door d in instance.DoorList)
			{
				if (loc.vw != ~0 && d.enter.vw != loc.vw)
					continue;

				float calculated_distance = Vector3.Distance(d.enter.pos, loc.pos);

				if (calculated_distance > distance_limit)
					continue;

				if(calculated_distance < smallest_distance)
				{
					closest_door = d;
					smallest_distance = calculated_distance;
				}
			}

			return closest_door;
		}

		/**
		 * Finds a Door instance that's closest to the given Vector3 position.
		 *
		 * NOTE: This method will not perform a VW check.
		 *
		 * @param Vector3 pos The position of the door to which we're trying to find the closest door.
		 * @param float distance_limit The maximum distance a door can be away from the Vector3. Defaults to infinity.
		 * @return Door An instance of the Door or null if none found.
		 */
		public static Door GetDoor(Vector3 pos, float distance_limit = float.MaxValue)
		{
			Location loc = new Location();
			loc.pos = pos;
			loc.vw = ~0;
			return GetDoor(loc, distance_limit);
		}
	}
}
