using System;
using MySql.Data.MySqlClient;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using lsrp_gamemode.Misc;

namespace lsrp_gamemode.Player
{
    public class Login : Script
    {
        public static Boolean LSRP_DisconnectPlayer(Client player, Boolean save_pos = false)
        {
            if(Database.SavePlayer(player, save_pos))
            {
                API.shared.kickPlayer(player);
            }
            return true;
        }

        public static Boolean OnPlayerLogin(Client player, double x, double y, double z, bool crash)
        {
            player.setSkin(player.getData("pSkin"));
            player.name = player.getData("name");
            player.nametag = player.getData("name");
            Utils.SetMoney(player, player.getData("pCash"));

            if(crash)
            {
                API.shared.setEntityPosition(player, new Vector3(x, y, z));
                API.shared.setEntityDimension(player, player.getData("pVW"));
                Database.UpdateCrash(player, false);
            }

            return true;
        }

        public static void ShowCharacters(Client player, Dictionary<int, string> characters, API API)
        {
            if (characters.Count == 0)
            {
                API.sendChatMessageToPlayer(player, "Nie posiadasz żadnej postaci, utwórz ją w panelu gry!");
                return;
            }

            string json = API.toJson(characters);
            API.triggerClientEvent(player, "menu_character_select", "character_selected", "Postacie", null, false, json);
        }
    }
}