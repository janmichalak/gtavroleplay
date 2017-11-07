using GrandTheftMultiplayer.Server.API;
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
        #endregion

        #region methods
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
            //WEAPON
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
                    Item.SaveItem(item.uid);
                    item.use = 0;
                }
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
        public static Item CreateItem(Client player, int owner_id, string name, int type, int value1, int value2, int value3)
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
        public static void SaveItem(int item_uid)
        {
            Item item = GetByUid(item_uid);
            Database.command.CommandText = String.Format("UPDATE items SET value1='{0}', value2='{1}', value3='{2}' WHERE uid='{3}' ", item.value1, item.value2, item.value3, item.uid);
            Database.command.ExecuteNonQuery();
        }
        #endregion
    }
}
