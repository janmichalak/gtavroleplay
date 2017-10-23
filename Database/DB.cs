using System;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using lsrp_gamemode.Misc;
using lsrp_gamemode.Player;
using System.Collections.Generic;
using System.Text;

namespace lsrp_gamemode
{
    public class Database : Script
    {
        public static string myConnectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3}", Config.DB_HOST,Config.DB_DB,Config.DB_USER,Config.DB_PASS);
        public static MySqlConnection connection;
        public static MySqlCommand command;
        public static MySqlDataReader Reader;

        /**
            *	Connect MySQL
            */
        public void Connect()
        {
            connection = new MySqlConnection(myConnectionString);
            command = connection.CreateCommand();

            try
            {
                connection.Open();
                API.consoleOutput("[MySQL] Database connection OK!");
            }
            catch
            {
                API.consoleOutput("[MySQL] Database connection FAIL!");
            }
        }

        /**
            *	Disconnect MySQL
            */
        public void Disconnect()
        {
            try
            {
                connection.Close();
                API.consoleOutput("[MySQL] Database disconnect OK!");
            }
            catch
            {
                API.consoleOutput("[MySQL] Database disconnect FAIL!");
            }
        }

        /// <summary>
        /// Check is player exists
        /// </summary>
        /// <param name="player"></param>
        /// <param name="password"></param>
        /// <param name="API"></param>
        /// <returns></returns>
        public static Boolean loginPlayer(Client player, string password, API API)
        {
            command.CommandText = "SELECT * FROM users AS u JOIN characters AS c ON c.owner = u.id WHERE u.username = '" + player.name + "' AND u.hash = '" + Utils.Sha256(password) + "'";
            Reader = command.ExecuteReader();

            int id = 0;
            Dictionary<int, string> characters = new Dictionary<int, string>();

            while (Reader.Read())
            {
                id = Reader.GetInt32("id");
                characters.Add(Reader.GetInt32("cid"), Reader.GetString("name"));
            }

            // Close reader (Important !)
            Reader.Close();

            if (id > 0)
            {
                Login.ShowCharacters(player, characters, API);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  TODO: Nie wykonuje sie to gówno
        /// </summary>
        /// <param name="player"></param>
        /// <param name="value"></param>
        public static void UpdateCrash(Client player, Boolean value)
        {
            int crash = 0;
            if (value) crash = 1;
            API.shared.consoleOutput(crash.ToString());
            command.CommandText = "UPDATE characters SET crash = " + crash + " WHERE cid = " + player.getData("pUID");

            try
            {
                command.ExecuteNonQuery();
                return;
            } catch (MySqlException ex) { }
        }

        /// <summary>
        /// Save player to DB.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="save_pos"></param>
        /// <returns></returns>
        public static Boolean SavePlayer(Client player, Boolean save_pos = false)
        {
            command.CommandText = "UPDATE characters SET money = " + player.getData("pCash") + ", health = " + player.health;
            if(save_pos)
            {
                Vector3 pos = player.position;
                command.CommandText += ", crash = 1, posx = '" + pos.X + "', posy = '" + pos.Y + "', posz = '" + pos.Z + "', dimension = " + player.getData("pVW");
            }
            command.CommandText += " WHERE cid = " + player.getData("pUID");

            try
            {
                command.ExecuteNonQuery();
                return true;
            }   catch (MySqlException ex) { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="char_id"></param>
        public static void GetCharacterData(Client player, int char_id)
        {
            command.CommandText = "SELECT * FROM characters WHERE cid = " + char_id;
            Reader = command.ExecuteReader();

            double posX = 0.0, posY = 0.0, posZ = 0.0;
            bool is_crash = false;
            while(Reader.Read())
            {
                player.setData("pUID", Reader.GetInt32("cid"));
                player.setData("pSkin", (PedHash) Reader.GetInt32("skin"));
                player.setData("pName", Reader.GetString("name"));
                player.setData("pDisplayName", Reader.GetString("name"));
                player.setData("pCash", Reader.GetInt32("money"));
                player.setData("pVW", Reader.GetInt32("dimension"));

                posX = Reader.GetDouble("posx");
                posY = Reader.GetDouble("posy");
                posZ = Reader.GetDouble("posz");

                if(Reader.GetInt32("crash") == 1)
                {
                    is_crash = true;
                }
            }

            // Unfreeze
            API.shared.sendChatMessageToPlayer(player, "Pomyœlnie zalogowano!");
            API.shared.setEntityInvincible(player, false);
            API.shared.freezePlayer(player, false);

            // Position
            API.shared.triggerClientEvent(player, "hide_menu");
            player.position = new Vector3(102.5816, -1944.02, 20.80372);
            API.shared.setEntityDimension(player, 0);

            Login.OnPlayerLogin(player, posX, posY, posZ, is_crash);
            Reader.Close();
            return;
        }
    }
}