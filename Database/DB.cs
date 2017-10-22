using System;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace lsrp_gamemode.DBase
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
        public static Boolean loginPlayer(string login, string password)
        {
            command.CommandText = "SELECT * FROM users AS u JOIN characters AS c ON c.owner = u.id WHERE u.username = 'blint96' AND u.hash = ''";
            Reader = command.ExecuteReader();

            int id = 0;
            while (Reader.Read())
            {
                id = Reader.GetInt32("id");
            }

            // Close reader (Important !)
            Reader.Close();

            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}