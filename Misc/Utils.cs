using System;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace lsrp_gamemode.Misc
{
    public class Utils : Script
    {
        public static void ChatMethod(Client player, float radius, string message, string color1, string color2, string color3, string color4)
        {
            float distance;
            string send = String.Format("{0} mówi: {1}", player.getData("data").displayName, message);
            foreach (var p in API.shared.getAllPlayers())
            {
                distance = Vector3.Distance(player.position, p.position);
                API.shared.consoleOutput(distance.ToString());
                if(distance <= radius)
                {
                    if((distance/radius) > 0.8 && (distance/radius) <= 1.0)
                    {
                        API.shared.sendChatMessageToPlayer(p, color4, send);
                    }
                    else if((distance/radius) > 0.5 && (distance/radius) <= 0.8)
                    {
                        API.shared.sendChatMessageToPlayer(p, color3, send);
                    }
                    else if((distance/radius) > 0.2 && (distance/radius) <= 0.5)
                    {
                        API.shared.sendChatMessageToPlayer(p, color2, send);
                    }
                    else
                    {
                        API.shared.sendChatMessageToPlayer(p, color1, send);
                    }
                }
            }
        }

        public static void ProxDetector(Client player, float radius, string color, string message)
        {
            foreach (Client p in API.shared.getPlayersInRadiusOfPlayer(radius, player))
            {
                API.shared.sendChatMessageToPlayer(p, String.Format("~#{0}~", color), message);
            }
        }

        public static void SetMoney(Client player, int value)
        {
            player.setData("pCash", value);
            API.shared.triggerClientEvent(player, "update_money_display", value);
        }

        public static string Sha256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string DictionaryToJSON(Dictionary<int, string> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }
    }
}