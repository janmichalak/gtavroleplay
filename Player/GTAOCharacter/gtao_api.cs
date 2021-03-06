﻿using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

public class GTAOnlineCharacter : Script
{
    // Exported

    public static void initializeMyClothes(Client player)
    {
        PedHash model = (PedHash)API.shared.getEntityModel(player);
        if (model != PedHash.FreemodeFemale01 && model != PedHash.FreemodeMale01) return;

        API.shared.setPlayerClothes(player, 0, 45, 0);      // face
        API.shared.setPlayerClothes(player, 2, 4, 0);      // hair
        API.shared.setPlayerClothes(player, 3, 15, 0);      // torso
        API.shared.setPlayerClothes(player, 8, 3, 1);       // undershirt
        API.shared.setPlayerClothes(player, 4, 75, 0);      // pants (15)
        API.shared.setPlayerClothes(player, 6, 35, 0);      // shoes (19)
        API.shared.setPlayerClothes(player, 11, 5, 7);     // top (107)
        API.shared.setPlayerAccessory(player, 1, 11, 0);     // glasses

        API.shared.setEntitySyncedData(player, "GTAO_SHAPE_FIRST_ID", 33);  // twarz id
    }

    public static void initializePedFace(NetHandle ent)
    {
        API.shared.setEntitySyncedData(ent, "GTAO_HAS_CHARACTER_DATA", true);

        API.shared.setEntitySyncedData(ent, "GTAO_SHAPE_FIRST_ID", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_SHAPE_SECOND_ID", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_SKIN_FIRST_ID", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_SKIN_SECOND_ID", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_SHAPE_MIX", 0f);
        API.shared.setEntitySyncedData(ent, "GTAO_SKIN_MIX", 0f);
        API.shared.setEntitySyncedData(ent, "GTAO_HAIR_COLOR", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_HAIR_HIGHLIGHT_COLOR", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_EYE_COLOR", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_EYEBROWS", 0);

        //API.setEntitySyncedData(ent, "GTAO_MAKEUP", 0); // No lipstick by default. 
        //API.setEntitySyncedData(ent, "GTAO_LIPSTICK", 0); // No makeup by default.

        API.shared.setEntitySyncedData(ent, "GTAO_EYEBROWS_COLOR", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_MAKEUP_COLOR", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_LIPSTICK_COLOR", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_EYEBROWS_COLOR2", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_MAKEUP_COLOR2", 0);
        API.shared.setEntitySyncedData(ent, "GTAO_LIPSTICK_COLOR2", 0);

        var list = new float[21];

        for (var i = 0; i < 21; i++)
        {
            list[i] = 0f;
        }

        API.shared.setEntitySyncedData(ent, "GTAO_FACE_FEATURES_LIST", list);
    }

    public static void removePedFace(NetHandle ent)
    {
        API.shared.setEntitySyncedData(ent, "GTAO_HAS_CHARACTER_DATA", false);
        API.shared.resetEntitySyncedData(ent, "GTAO_SHAPE_FIRST_ID");
        API.shared.resetEntitySyncedData(ent, "GTAO_SHAPE_SECOND_ID");
        API.shared.resetEntitySyncedData(ent, "GTAO_SKIN_FIRST_ID");
        API.shared.resetEntitySyncedData(ent, "GTAO_SKIN_SECOND_ID");
        API.shared.resetEntitySyncedData(ent, "GTAO_SHAPE_MIX");
        API.shared.resetEntitySyncedData(ent, "GTAO_SKIN_MIX");
        API.shared.resetEntitySyncedData(ent, "GTAO_HAIR_COLOR");
        API.shared.resetEntitySyncedData(ent, "GTAO_HAIR_HIGHLIGHT_COLOR");
        API.shared.resetEntitySyncedData(ent, "GTAO_EYE_COLOR");
        API.shared.resetEntitySyncedData(ent, "GTAO_EYEBROWS");
        API.shared.resetEntitySyncedData(ent, "GTAO_MAKEUP");
        API.shared.resetEntitySyncedData(ent, "GTAO_LIPSTICK");
        API.shared.resetEntitySyncedData(ent, "GTAO_EYEBROWS_COLOR");
        API.shared.resetEntitySyncedData(ent, "GTAO_MAKEUP_COLOR");
        API.shared.resetEntitySyncedData(ent, "GTAO_LIPSTICK_COLOR");
        API.shared.resetEntitySyncedData(ent, "GTAO_EYEBROWS_COLOR2");
        API.shared.resetEntitySyncedData(ent, "GTAO_MAKEUP_COLOR2");
        API.shared.resetEntitySyncedData(ent, "GTAO_LIPSTICK_COLOR2");
        API.shared.resetEntitySyncedData(ent, "GTAO_FACE_FEATURES_LIST");
    }

    public static bool isPlayerFaceValid(NetHandle ent)
    {
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_SHAPE_FIRST_ID")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_SHAPE_SECOND_ID")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_SKIN_FIRST_ID")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_SKIN_SECOND_ID")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_SHAPE_MIX")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_SKIN_MIX")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_HAIR_COLOR")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_HAIR_HIGHLIGHT_COLOR")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_EYE_COLOR")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_EYEBROWS")) return false;
        //if (!API.hasEntitySyncedData(ent, "GTAO_MAKEUP")) return false; // Player may have no makeup
        //if (!API.hasEntitySyncedData(ent, "GTAO_LIPSTICK")) return false; // Player may have no lipstick
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_EYEBROWS_COLOR")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_MAKEUP_COLOR")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_LIPSTICK_COLOR")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_EYEBROWS_COLOR2")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_MAKEUP_COLOR2")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_LIPSTICK_COLOR2")) return false;
        if (!API.shared.hasEntitySyncedData(ent, "GTAO_FACE_FEATURES_LIST")) return false;

        return true;
    }

    public static void updatePlayerFace(NetHandle player)
    {
        API.shared.triggerClientEventForAll("UPDATE_CHARACTER", player);
    }
}