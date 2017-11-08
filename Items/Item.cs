using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;

namespace lsrp_gamemode.Items
{
    public class Item
    {
        // All player items
        public static Dictionary<NetHandle, List<Item>> PlayerItems = new Dictionary<NetHandle, List<Item>>();
        public static List<Item> FloorItems = new List<Item>();

        #region item fields
        public int uid = 0;
        public int type = Config.ITEM_TYPE_NONE;

        public int owner = 0;
        public int place = Config.PLACE_ITEM_NONE;

        public string name = "";

        public int value1 = 0;
        public int value2 = 0;
        public int value3 = 0;

        public string string1 = "";
        public string string2 = "";

        public float posx = 0f;
        public float posy = 0f;
        public float posz = 0f;

        public int dimension = 0;

        public int use = 0;

        public NetHandle obj = new NetHandle();
        #endregion

        #region methods
        /// <summary>
        /// Drop item
        /// </summary>
        /// <param name="player"></param>
        /// <param name="item_uid"></param>
        public static void Drop(Client player, int item_uid)
        {
            Item item = GetByUid(item_uid);
            if (item.uid == 0)
            {
                API.shared.sendChatMessageToPlayer(player, "Wystąpił błąd podczas używania przedmiotu!");
                return;
            }

            Vector3 pos = player.position;
            pos.Z -= 1.0f;
            item.place = Config.PLACE_ITEM_NONE;
            item.owner = 0;
            item.posx = pos.X;
            item.posy = pos.Y;
            item.posz = pos.Z;
            item.dimension = API.shared.getEntityDimension(player);

            item.obj = API.shared.createObject(Config.DEFAULT_ITEM_OBJECT_ID, pos, new Vector3(0, 0, 0), player.dimension);
            Save(item_uid, (Config.ITEM_SAVE_OWNER | Config.ITEM_SAVE_POS));

            Commands.cmd_me(player, "odkłada przedmiot na ziemię.");
            var items = PlayerItems[player.handle];
            items.Remove(item);
            FloorItems.Add(item);
        }

        /// <summary>
        /// Use item
        /// </summary>
        /// <param name="player"></param>
        /// <param name="item_uid"></param>
        public static void Use(Client player, int item_uid)
        {
            Item item = GetByUid(item_uid);
            if(item.uid == 0)
            {
                API.shared.sendChatMessageToPlayer(player, "Wystąpił błąd podczas używania przedmiotu!");
                return;
            }

            // FOOD
            if(item.type == Config.ITEM_TYPE_FOOD)
            {
                int value = item.value1;
                int health = player.health;
                string name = item.name;

                health += value;
                if (health > 100) health = 100;

                player.health = health;
                ItemManager.DeleteItem(item);

                Commands.cmd_me(player, String.Format("spożywa {0}.", name));
            }

            // WEAPON
            if(item.type == Config.ITEM_TYPE_WEAPON)
            {
                WeaponHash type = (WeaponHash)item.value1;
                if (item.use == 0)
                {
                    int ammo = item.value2;
                    Commands.cmd_me(player, "wyjmuje " + item.name + ".");
                    API.shared.givePlayerWeapon(player, type, ammo, true, true);
                    item.use = 1;
                }
                else
                {
                    item.value2 = API.shared.getPlayerWeaponAmmo(player, type);
                    API.shared.removePlayerWeapon(player, type);
                    Commands.cmd_me(player, "chowa " + item.name + ".");
                    Item.Save(item.uid, Config.ITEM_SAVE_BASIC);
                    item.use = 0;
                }
            }

            // CLOTH
            if(item.type == Config.ITEM_TYPE_CLOTH)
            {
                PedHash skin = (PedHash)item.value1;
                API.shared.setPlayerSkin(player, skin);

                if(skin == PedHash.FreemodeFemale01 || skin == PedHash.FreemodeMale01)
                {
                    // pobrac konfiguracje skina i zaladowac
                }
            }

            // WATCH
            if(item.type == Config.ITEM_TYPE_WATCH)
            {
                TimeSpan time = API.shared.getTime();
                API.shared.sendChatMessageToPlayer(player, String.Format("{0}Aktualna godzina to {1}:{2}", 
                    Config.COLOR_B, time.Hours, (time.Minutes > 9 ? time.Minutes.ToString() : "0" + time.Minutes.ToString())));
            }
        }
        #endregion

        #region misc methods
        public static Item GetByUid(int item_uid)
        {
            foreach(KeyValuePair<NetHandle, List<Item>> pi in PlayerItems)
            {
                foreach(Item i in pi.Value)
                {
                    if(i.uid == item_uid)
                    {
                        return i;
                    }
                }
            }
            return new Item();
        }

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="player"></param>
        /// <param name="owner_id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="value3"></param>
        /// <returns>Created item</returns>
        public static Item Create(Client player, int owner_id, string name, int type, int value1, int value2, int value3)
        {
            List<Item> ItemList = Item.PlayerItems[player.handle];
            Database.command.CommandText = "INSERT INTO items (type, owner, place, name, value1, value2, value3) ";
            Database.command.CommandText += String.Format("VALUES('{0}', '{1}', '2', '{2}', '{3}', '{4}', '{5}')",
                type, owner_id, name, value1, value2, value3);
            Database.command.ExecuteNonQuery();
            Item item = new Item();
            item.uid = (int)Database.command.LastInsertedId;
            item.type = type;
            item.name = name;
            item.owner = owner_id;
            item.value1 = value1;
            item.value2 = value2;
            item.value3 = value3;
            item.use = 0;
            ItemList.Add(item);
            return item;
        }

        /// <summary>
        /// Sync item with DB.
        /// </summary>
        /// <param name="item_uid"></param>
        public static void Save(int item_uid, int savetype = Config.ITEM_SAVE_BASIC)
        {
            Item item = GetByUid(item_uid);
            string update = String.Format("UPDATE items SET ");
            if((savetype & Config.ITEM_SAVE_BASIC) != 0)
            {
                update += String.Format("value1='{0}', value2='{1}', value3='{2}', ", item.value1, item.value2, item.value3);
            }
            if((savetype & Config.ITEM_SAVE_POS) != 0)
            {
                update += String.Format("posx='{0}', posy='{1}', posz='{2}', dimension='{3}', ", 
                    item.posx.ToString().Replace(",", "."), item.posy.ToString().Replace(",", "."), item.posz.ToString().Replace(",", "."), item.dimension);
            }
            if((savetype & Config.ITEM_SAVE_OWNER) != 0)
            {
                update += String.Format("owner='{0}', place='{1}', ", item.owner, item.place);
            }

            update = update.Remove(update.Length - 2, 1);
            update += String.Format("WHERE uid='{0}'", item.uid);

            Database.command.CommandText = update;
            Database.command.ExecuteNonQuery();
        }
        #endregion
    }
}
