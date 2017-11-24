using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lsrp_gamemode.Doors
{
	public class Door
	{
		// Identifiables
		public int uid;
		public String name = "Drzwi";

		// Ownership
		public int owner = 0;
		public int ownertype = Config.OWNER_NONE;

		// Location
		public Location enter;
		public Location exit;
		
		// World Objects
		public Marker marker = null;
		public int markerType = 0;
		public Color color;
	
		/**
		 * Constructor which takes a UID at minimum.
		 * 
		 * @param int uid The database UID of the door we're dealing with.
		 */
		public Door(int uid)
		{
			this.uid = uid;
		}

		/**
		 * Updates all the door parameters in the database.
		 */
		public void Save()
		{
			DoorManager dm = DoorManager.getInstance();
			Door self = dm.Find(this.uid);
			dm.Save(self);
		}

		// *** Mutators *** //

		/**
		 * Sets the Door enterance location.
		 * @param Location loc The new location of the enterance.
		 */
		public void SetEnterance(Location loc)
		{
			this.enter = loc;
			this.Save();
		}

		/**
		 * Sets the Door enterance position.
		 *
		 * NOTE: This will not update the VW or the angle of the enterance.
		 *
		 * @param Location loc The new location of the enterance.
		 * @param Vector3 pos The new position of the enterance.
		 */
		public void SetEnterance(Vector3 pos)
		{
			this.enter.pos = pos;
			this.Save();
		}

		/**
		 * Sets the Door exit location.
		 * @param Location loc The new location of the exit.
		 */
		public void SetExit(Location loc)
		{
			this.exit = loc;
			this.Save();
		}

		/**
		 * Sets the Door exit position.
		 *
		 * NOTE: This will not update the VW or the angle of the exit.
		 *
		 * @param Location loc The new location of the exit.
		 * @param Vector3 pos The new position of the exit.
		 */
		public void SetExit(Vector3 pos)
		{
			this.exit.pos = pos;
			this.Save();
		}

		/** 
		 * Renames the door.
		 *
		 * @param String new_name New name given to this door.
		 */
		public void Rename(String new_name)
		{
			this.name = Utils.SanitizeString(new_name);
			this.Save();
		}

		// *** Utilities *** //

		/**
		 * Measure the distance between this and another door.
		 *
		 * @param Door door An instance of a door to which we're measuring the distance.
		 * @return float The distance between the two doors.
		 */
		public float Distance(Door door)
		{
			return Vector3.Distance(this.enter.pos, door.enter.pos);
		}

	}
}
