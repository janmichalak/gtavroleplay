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
using lsrp_gamemode.Player;

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
            PlayerClass p = player.getData("data");
            player.setSkin(p.skin);
            player.name = p.name;
            player.nametag = p.displayName;
            player.health = p.health;
            Utils.SetMoney(player, p.cash);

            // GTAO: Character
            GTAOnlineCharacter.initializePedFace(player);
            GTAOnlineCharacter.initializeMyClothes(player);
            GTAOnlineCharacter.updatePlayerFace(player);

            if(crash)
            {
                API.shared.setEntityPosition(player, new Vector3(x, y, z));
                API.shared.setEntityDimension(player, p.vw);
                Database.UpdateCrash(player, false);
            }

            // Nametag
            player.nametag = p.displayName.Replace("_", " ") + " (" + p.id + ")";

            // Message (admin previlages)
            int admin = player.getData("admin");
            if(admin > 0)
            {
                API.shared.sendNotificationToPlayer(player, "~h~Wczytano uprawnienia ~r~administratora", true);
            }
            else if(admin < 0)
            {
                API.shared.sendNotificationToPlayer(player, "~h~Wczytano uprawnienia ~b~supportera", true);
            }

            player.setData("logged", true);
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