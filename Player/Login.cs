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
        public static void ShowCharacters(Client player, Dictionary<int, string> characters, API API)
        {
            if (characters.Count == 0)
            {
                API.sendChatMessageToPlayer(player, "Nie posiadasz żadnej postaci, utwórz ją w panelu gry!");
                return;
            }

            //string json = Utils.DictionaryToJSON(characters);
            string json = API.toJson(characters);
            API.triggerClientEvent(player, "menu_character_select", "character_selected", "Postacie", null, false, json);
        }
    }
}