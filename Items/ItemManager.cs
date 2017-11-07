using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Player;
using System.Collections.Generic;

namespace lsrp_gamemode.Items
{
    public class ItemManager : Script
    {
        // Constructor
        public ItemManager()
        {
            API.onPlayerFinishedDownload += Items_PlayerJoin;
        }

        // TODO -> on resource start
        public static void LoadItems()
        {

        }

        public static void DeleteItem(Item item)
        {
            Database.command.CommandText = String.Format("DELETE FROM items WHERE uid = {0}", item.uid);
            Database.command.ExecuteNonQuery();

            foreach (KeyValuePair<NetHandle, List<Item>> pi in Item.PlayerItems)
            {
                for(int i = pi.Value.Count - 1; i >= 0; i--)
                {
                    if(pi.Value[i] == item)
                    {
                        pi.Value.Remove(pi.Value[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Load player items to array
        /// </summary>
        /// <param name="player"></param>
        public static void LoadPlayerItems(Client player)
        {
            NetHandle handle = player.handle;
            PlayerClass pc = player.getData("data");
            API.shared.consoleOutput(String.Format("[load] Wczytuje przedmioty gracza {0} ({1})", pc.displayName, pc.uid));

            Database.command.CommandText = String.Format("SELECT * FROM items WHERE owner = {0} AND place = {1}", pc.uid, Config.PLACE_ITEM_PLAYER);
            Database.Reader = Database.command.ExecuteReader();

            var r = Database.Reader; int loaded = 0;
            List<Item> ItemList = Item.PlayerItems[handle];
            while(r.Read())
            {
                Item i = new Item();
                i.uid = r.GetInt32("uid");
                i.type = r.GetInt32("type");
                i.owner = r.GetInt32("owner");
                i.place = r.GetInt32("place");
                i.name = r.GetString("name");
                i.value1 = r.GetInt32("value1");
                i.value2 = r.GetInt32("value2");
                i.value3 = r.GetInt32("value3");
                i.string1 = r.GetString("str1");
                i.string2 = r.GetString("str2");
                i.posx = r.GetFloat("posx");
                i.posy = r.GetFloat("posy");
                i.posz = r.GetFloat("posz");
                i.dimension = r.GetInt32("dimension");
                i.use = r.GetInt32("use");
                ItemList.Add(i);
                loaded += 1;
            }

            Database.Reader.Close();
            API.shared.consoleOutput("[load] Załadowano " + loaded + " przedmiotów.");
        }

        /// <summary>
        /// Get player items
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static List<Item> GetPlayerItems(Client player)
        {
            if(Item.PlayerItems.ContainsKey(player.handle))
            {
                return Item.PlayerItems[player.handle];
            }
            return new List<Item>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public static void Debug_ListPlayerItems(Client player)
        {
            if(Item.PlayerItems.ContainsKey(player.handle))
            {
                List<Item> items = Item.PlayerItems[player.handle];
                foreach(Item i in items)
                {
                    API.shared.consoleOutput(String.Format("Przedmiot: {0}, UID: {1}", i.name, i.uid));
                }
            }
        }

        #region events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void Items_PlayerJoin(Client player)
        {
            NetHandle handle = player.handle;
            if(Item.PlayerItems.ContainsKey(handle))
            {
                Item.PlayerItems.Remove(player.handle);
            }
            Item.PlayerItems.Add(player.handle, new List<Item>());
        }
        #endregion
    }
}
