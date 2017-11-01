using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using Newtonsoft.Json;

namespace Interiors
{
    class Interiors : Script
    {
        List<Interior> interiors;

        public Interiors()
        {
            API.onResourceStart += onResourceStartHandler;
            API.onPlayerFinishedDownload += onPlayerFinishedDownloadHandler;
            API.onClientEventTrigger += onClientEventTriggerHandler;
        }

        public void onResourceStartHandler()
        {
            API.consoleOutput("Interiors browser started...");
            interiors = new List<Interior>();

            interiors.Add(new Interior("Online Bunkers Exterior", "DesertBunker", "gr_case0_bunkerclosed", new Vector3(848.6175, 2996.567, 45.81612)));
            interiors.Add(new Interior("Online Bunkers Exterior", "SmokeTreeBunker", "gr_case1_bunkerclosed", new Vector3(2126.785, 3335.04, 48.21422)));
            interiors.Add(new Interior("Online Bunkers Exterior", "ScrapyardBunker", "gr_case2_bunkerclosed", new Vector3(2493.654, 3140.399, 51.28789)));
            interiors.Add(new Interior("Online Bunkers Exterior", "OilfieldsBunker", "gr_case3_bunkerclosed", new Vector3(481.0465, 2995.135, 43.96672)));
            interiors.Add(new Interior("Online Bunkers Exterior", "RatonCanyonBunker", "gr_case4_bunkerclosed", new Vector3(-391.3216, 4363.728, 58.65862)));
            interiors.Add(new Interior("Online Bunkers Exterior", "GrapeseedBunker", "gr_case5_bunkerclosed", new Vector3(1823.961, 4708.14, 42.4991)));
            interiors.Add(new Interior("Online Bunkers Exterior", "FarmhouseBunker", "gr_case6_bunkerclosed", new Vector3(1570.372, 2254.549, 78.89397)));
            interiors.Add(new Interior("Online Bunkers Exterior", "PalletoBunker", "gr_case7_bunkerclosed", new Vector3(-783.0755, 5934.686, 24.31475)));
            interiors.Add(new Interior("Online Bunkers Exterior", "Route68Bunker", "gr_case9_bunkerclosed", new Vector3(24.43542, 2959.705, 58.35517)));
            interiors.Add(new Interior("Online Bunkers Exterior", "ZancudoBunker", "gr_case10_bunkerclosed", new Vector3(-3058.714, 3329.19, 12.5844)));
            interiors.Add(new Interior("Online Bunkers Exterior", "Route1Bunker", "gr_case11_bunkerclosed", new Vector3(-3180.466, 1374.192, 19.9597)));

            interiors.Add(new Interior("Online Apartments", "Modern 1 Apartment", "apa_v_mp_h_01_a", new Vector3(-786.8663, 315.7642, 217.6385)));
            interiors.Add(new Interior("Online Apartments", "Modern 2 Apartment", "apa_v_mp_h_01_c", new Vector3(-786.9563, 315.6229, 187.9136)));
            interiors.Add(new Interior("Online Apartments", "Modern 3 Apartment", "apa_v_mp_h_01_b", new Vector3(-774.0126, 342.0428, 196.6864)));
            interiors.Add(new Interior("Online Apartments", "Mody 1 Apartment", "apa_v_mp_h_02_a", new Vector3(-787.0749, 315.8198, 217.6386)));
            interiors.Add(new Interior("Online Apartments", "Mody 2 Apartment", "apa_v_mp_h_02_c", new Vector3(-786.8195, 315.5634, 187.9137)));
            interiors.Add(new Interior("Online Apartments", "Mody 3 Apartment", "apa_v_mp_h_02_b", new Vector3(-774.1382, 342.0316, 196.6864)));
            interiors.Add(new Interior("Online Apartments", "Vibrant 1 Apartment", "apa_v_mp_h_03_a", new Vector3(-786.6245, 315.6175, 217.6385)));
            interiors.Add(new Interior("Online Apartments", "Vibrant 2 Apartment", "apa_v_mp_h_03_c", new Vector3(-786.9584, 315.7974, 187.9135)));
            interiors.Add(new Interior("Online Apartments", "Vibrant 3 Apartment", "apa_v_mp_h_03_b", new Vector3(-774.0223, 342.1718, 196.6863)));
            interiors.Add(new Interior("Online Apartments", "Sharp 1 Apartment", "apa_v_mp_h_04_a", new Vector3(-787.0902, 315.7039, 217.6384)));
            interiors.Add(new Interior("Online Apartments", "Sharp 2 Apartment", "apa_v_mp_h_04_c", new Vector3(-787.0155, 315.7071, 187.9135)));
            interiors.Add(new Interior("Online Apartments", "Sharp 3 Apartment", "apa_v_mp_h_04_b", new Vector3(-773.8976, 342.1525, 196.6863)));
            interiors.Add(new Interior("Online Apartments", "Monochrome 1 Apartment", "apa_v_mp_h_05_a", new Vector3(-786.9887, 315.7393, 217.6386)));
            interiors.Add(new Interior("Online Apartments", "Monochrome 2 Apartment", "apa_v_mp_h_05_c", new Vector3(-786.8809, 315.6634, 187.9136)));
            interiors.Add(new Interior("Online Apartments", "Monochrome 3 Apartment", "apa_v_mp_h_05_b", new Vector3(-774.0675, 342.0773, 196.6864)));
            interiors.Add(new Interior("Online Apartments", "Seductive 1 Apartment", "apa_v_mp_h_06_a", new Vector3(-787.1423, 315.6943, 217.6384)));
            interiors.Add(new Interior("Online Apartments", "Seductive 2 Apartment", "apa_v_mp_h_06_c", new Vector3(-787.0961, 315.815, 187.9135)));
            interiors.Add(new Interior("Online Apartments", "Seductive 3 Apartment", "apa_v_mp_h_06_b", new Vector3(-773.9552, 341.9892, 196.6862)));
            interiors.Add(new Interior("Online Apartments", "Regal 1 Apartment", "apa_v_mp_h_07_a", new Vector3(-787.029, 315.7113, 217.6385)));
            interiors.Add(new Interior("Online Apartments", "Regal 2 Apartment", "apa_v_mp_h_07_c", new Vector3(-787.0574, 315.6567, 187.9135)));
            interiors.Add(new Interior("Online Apartments", "Regal 3 Apartment", "apa_v_mp_h_07_b", new Vector3(-774.0109, 342.0965, 196.6863)));
            interiors.Add(new Interior("Online Apartments", "Aqua 1 Apartment", "apa_v_mp_h_08_a", new Vector3(-786.9469, 315.5655, 217.6383)));
            interiors.Add(new Interior("Online Apartments", "Aqua 2 Apartment", "apa_v_mp_h_08_c", new Vector3(-786.9756, 315.723, 187.9134)));
            interiors.Add(new Interior("Online Apartments", "Aqua 3 Apartment", "apa_v_mp_h_08_b", new Vector3(-774.0349, 342.0296, 196.6862)));

            interiors.Add(new Interior("Arcadius Business Centre", "Executive Rich", "ex_dt1_02_office_02b", new Vector3(-141.1987, -620.913, 168.8205)));
            interiors.Add(new Interior("Arcadius Business Centre", "Executive Cool", "ex_dt1_02_office_02c", new Vector3(-141.5429, -620.9524, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Executive Contrast", "ex_dt1_02_office_02a", new Vector3(-141.2896, -620.9618, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Old Spice Warm", "ex_dt1_02_office_01a", new Vector3(-141.4966, -620.8292, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Old Spice Classical", "ex_dt1_02_office_01b", new Vector3(-141.3997, -620.9006, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Old Spice Vintage", "ex_dt1_02_office_01c", new Vector3(-141.5361, -620.9186, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Power Broker Ice", "ex_dt1_02_office_03a", new Vector3(-141.392, -621.0451, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Power Broker Conservative", "ex_dt1_02_office_03b", new Vector3(-141.1945, -620.8729, 168.8204)));
            interiors.Add(new Interior("Arcadius Business Centre", "Power Broker Polished", "ex_dt1_02_office_03c", new Vector3(-141.4924, -621.0035, 168.8205)));

            interiors.Add(new Interior("Maze Bank Building", "Executive Rich", "ex_dt1_11_office_02b", new Vector3(-75.8466, -826.9893, 243.3859)));
            interiors.Add(new Interior("Maze Bank Building", "Executive Cool", "ex_dt1_11_office_02c", new Vector3(-75.49945, -827.05, 243.386)));
            interiors.Add(new Interior("Maze Bank Building", "Executive Contrast", "ex_dt1_11_office_02a", new Vector3(-75.49827, -827.1889, 243.386)));
            interiors.Add(new Interior("Maze Bank Building", "Old Spice Warm", "ex_dt1_11_office_01a", new Vector3(-75.44054, -827.1487, 243.3859)));
            interiors.Add(new Interior("Maze Bank Building", "Old Spice Classical", "ex_dt1_11_office_01b", new Vector3(-75.63942, -827.1022, 243.3859)));
            interiors.Add(new Interior("Maze Bank Building", "Old Spice Vintage", "ex_dt1_11_office_01c", new Vector3(-75.47446, -827.2621, 243.386)));
            interiors.Add(new Interior("Maze Bank Building", "Power Broker Ice", "ex_dt1_11_office_03a", new Vector3(-75.56978, -827.1152, 243.3859)));
            interiors.Add(new Interior("Maze Bank Building", "Power Broker Conservative", "ex_dt1_11_office_03b", new Vector3(-75.51953, -827.0786, 243.3859)));
            interiors.Add(new Interior("Maze Bank Building", "Power Broker Polished", "ex_dt1_11_office_03c", new Vector3(-75.41915, -827.1118, 243.3858)));

            interiors.Add(new Interior("Lom Bank", "Executive Rich", "ex_sm_13_office_02b", new Vector3(-1579.756, -565.0661, 108.523)));
            interiors.Add(new Interior("Lom Bank", "Executive Cool", "ex_sm_13_office_02c", new Vector3(-1579.678, -565.0034, 108.5229)));
            interiors.Add(new Interior("Lom Bank", "Executive Contrast", "ex_sm_13_office_02a", new Vector3(-1579.583, -565.0399, 108.5229)));
            interiors.Add(new Interior("Lom Bank", "Old Spice Warm", "ex_sm_13_office_01a", new Vector3(-1579.702, -565.0366, 108.5229)));
            interiors.Add(new Interior("Lom Bank", "Old Spice Classical", "ex_sm_13_office_01b", new Vector3(-1579.643, -564.9685, 108.5229)));
            interiors.Add(new Interior("Lom Bank", "Old Spice Vintage", "ex_sm_13_office_01c", new Vector3(-1579.681, -565.0003, 108.523)));
            interiors.Add(new Interior("Lom Bank", "Power Broker Ice", "ex_sm_13_office_03a", new Vector3(-1579.677, -565.0689, 108.5229)));
            interiors.Add(new Interior("Lom Bank", "Power Broker Conservative", "ex_sm_13_office_03b", new Vector3(-1579.708, -564.9634, 108.5229)));
            interiors.Add(new Interior("Lom Bank", "Power Broker Polished", "ex_sm_13_office_03c", new Vector3(-1579.693, -564.8981, 108.5229)));

            interiors.Add(new Interior("Maze Bank West", "Executive Rich", "ex_sm_15_office_02b", new Vector3(-1392.667, -480.4736, 72.04217)));
            interiors.Add(new Interior("Maze Bank West", "Executive Cool", "ex_sm_15_office_02c", new Vector3(-1392.542, -480.4011, 72.04211)));
            interiors.Add(new Interior("Maze Bank West", "Executive Contrast", "ex_sm_15_office_02a", new Vector3(-1392.626, -480.4856, 72.04212)));
            interiors.Add(new Interior("Maze Bank West", "Old Spice Warm", "ex_sm_15_office_01a", new Vector3(-1392.617, -480.6363, 72.04208)));
            interiors.Add(new Interior("Maze Bank West", "Old Spice Classical", "ex_sm_15_office_01b", new Vector3(-1392.532, -480.7649, 72.04207)));
            interiors.Add(new Interior("Maze Bank West", "Old Spice Vintage", "ex_sm_15_office_01c", new Vector3(-1392.611, -480.5562, 72.04214)));
            interiors.Add(new Interior("Maze Bank West", "Power Broker Ice", "ex_sm_15_office_03a", new Vector3(-1392.563, -480.549, 72.0421)));
            interiors.Add(new Interior("Maze Bank West", "Power Broker Convservative", "ex_sm_15_office_03b", new Vector3(-1392.528, -480.475, 72.04206)));
            interiors.Add(new Interior("Maze Bank West", "Power Broker Polished", "ex_sm_15_office_03c", new Vector3(-1392.416, -480.7485, 72.04207)));

            interiors.Add(new Interior("Club & Warehouses", "Clubhouse 1", "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo", new Vector3(1107.04, -3157.399, -37.51859)));
            interiors.Add(new Interior("Club & Warehouses", "Clubhouse 2", "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo", new Vector3(998.4809, -3164.711, -38.90733)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse 1", "bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo", new Vector3(1009.5, -3196.6, -38.99682)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse 2", "bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo", new Vector3(1051.491, -3196.536, -39.14842)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse 3", "bkr_biker_interior_placement_interior_4_biker_dlc_int_ware03_milo", new Vector3(1093.6, -3196.6, -38.99841)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse 4", "bkr_biker_interior_placement_interior_5_biker_dlc_int_ware04_milo", new Vector3(1121.897, -3195.338, -40.4025)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse 5", "bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo", new Vector3(1165, -3196.6, -39.01306)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse Small", "ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo", new Vector3(1094.988, -3101.776, -39.00363)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse Medium", "ex_exec_warehouse_placement_interior_0_int_warehouse_m_dlc_milo", new Vector3(1056.486, -3105.724, -39.00439)));
            interiors.Add(new Interior("Club & Warehouses", "Warehouse Large", "ex_exec_warehouse_placement_interior_2_int_warehouse_l_dlc_milo", new Vector3(1006.967, -3102.079, -39.0035)));
            interiors.Add(new Interior("Club & Warehouses", "Cargarage", "imp_impexp_interior_placement_interior_1_impexp_intwaremed_milo", new Vector3(994.5925, -3002.594, -39.64699)));

            interiors.Add(new Interior("Special Locations", "Normal Cargo Ship", "cargoship", new Vector3(-163.3628, -2385.161, 5.999994)));
            interiors.Add(new Interior("Special Locations", "Sunken Cargo Ship", "sunkcargoship", new Vector3(-163.3628, -2385.161, 5.999994)));
            interiors.Add(new Interior("Special Locations", "Burning Cargo Ship", "SUNK_SHIP_FIRE", new Vector3(-163.3628, -2385.161, 5.999994)));
            interiors.Add(new Interior("Special Locations", "Red Carpet", "redCarpet", new Vector3(300.5927, 300.5927, 104.3776)));
            interiors.Add(new Interior("Special Locations", "Rekt Stilthouse Destroyed", "DES_StiltHouse_imapend", new Vector3(-1020.518, 663.27, 153.5167)));
            interiors.Add(new Interior("Special Locations", "Rekt Stilthouse Rebuild", "DES_stilthouse_rebuild", new Vector3(-1020.518, 663.27, 153.5167)));
            interiors.Add(new Interior("Special Locations", "Union Depository", "FINBANK", new Vector3(2.6968, -667.0166, 16.13061)));
            interiors.Add(new Interior("Special Locations", "Trevors Trailer Dirty", "TrevorsMP", new Vector3(1975.552, 3820.538, 33.44833)));
            interiors.Add(new Interior("Special Locations", "Trevors Trailer Clean", "TrevorsTrailerTidy", new Vector3(1975.552, 3820.538, 33.44833)));
            interiors.Add(new Interior("Special Locations", "Stadium", "SP1_10_real_interior", new Vector3(-248.6731, -2010.603, 30.14562)));
            interiors.Add(new Interior("Special Locations", "Max Renda Shop", "refit_unload", new Vector3(-585.8247, -282.72, 35.45475)));
            interiors.Add(new Interior("Special Locations", "Jewel Store", "post_hiest_unload", new Vector3(-630.07, -236.332, 38.05704)));
            interiors.Add(new Interior("Special Locations", "FIB Lobby", "FIBlobby", new Vector3(110.4, -744.2, 45.7496)));

            API.consoleOutput("Loaded " + interiors.Count() + " interiors!");
        }

        public void onClientEventTriggerHandler(Client player, String eventName, params object[] arguments)
        {
            if (eventName == "requestIplForPlayer")
            {
                Vector3 position = JsonConvert.DeserializeObject<Vector3>((string)arguments[1]);
                API.requestIpl(arguments[0].ToString());
                API.setEntityPosition(player, position);
            }
        }
        public void onPlayerFinishedDownloadHandler(Client player)
        {
            var interiorsList = JsonConvert.SerializeObject(interiors);
            API.triggerClientEvent(player, "registerInteriorsData", interiorsList);
        }
        [Command("interiors")]
        public void ListInteriors(Client player)
        {
            if (player.getData("admin") > 0)
            {
                API.triggerClientEvent(player, "showInteriorLocations");
            }
        }

    }
}
