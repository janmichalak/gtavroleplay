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
        public static string myConnectionString = "SERVER=localhost;" + "DATABASE=lsrpv;" + "UID=lsrpv;" + "PASSWORD=nEenaYKMJtkS;";
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

        /** 
          *	Check is player exists
          */
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

        /**
         * Get character data if logged.
         * */
        public static void GetCharacterData(Client player, int char_id)
        {
            command.CommandText = "SELECT * FROM characters WHERE cid = " + char_id;
            Reader = command.ExecuteReader();

            while(Reader.Read())
            {
                player.setSkin((PedHash) Reader.GetInt32("skin"));
            }

            // Unfreeze
            API.shared.sendChatMessageToPlayer(player, "Pomyœlnie zalogowano!");
            API.shared.setEntityInvincible(player, false);
            API.shared.freezePlayer(player, false);

            // Position
            API.shared.triggerClientEvent(player, "hide_menu");
            player.position = new Vector3(102.5816, -1944.02, 20.80372);
            API.shared.setEntityDimension(player, 0);

            Reader.Close();
            return;
        }
    }
}