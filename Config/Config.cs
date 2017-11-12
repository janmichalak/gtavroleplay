using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lsrp_gamemode
{
    public class Config
    {
        // Debug
        public static readonly bool DEBUG_MODE = true;

        // Colors
        public static readonly string COLOR_ME = "~#C2A2DA~";
        public static readonly string COLOR_DO = "~#9A9CCD~";
        public static readonly string COLOR_B = "~#AFAFAF~";
        public static readonly string COLOR_DARKBLUE = "~#2010E0~";
        public static readonly string COLOR_GC = "~#56cbd3~";
        public static readonly string COLOR_WHITE1 = "~#DDDDDD~";
        public static readonly string COLOR_LIGHTBLUE = "~#3399FF~";

        // Chat Colors
        public static readonly string COLOR_CHAT1 = "~#FFFFFF~";
        public static readonly string COLOR_CHAT2 = "~#BBBBBB~";
        public static readonly string COLOR_CHAT3 = "~#999999~";
        public static readonly string COLOR_CHAT4 = "~#555555~";

        // Owner types
        public static readonly int OWNER_NONE = 0;
        public static readonly int OWNER_PLAYER = 1;
        public static readonly int OWNER_GROUP = 2;

        // Item places
        public static readonly int PLACE_ITEM_NONE = 0;
        public static readonly int PLACE_ITEM_DOOR = 1;
        public static readonly int PLACE_ITEM_PLAYER = 2;
        public static readonly int PLACE_ITEM_VEHICLE = 3;

        // Item types
        public static readonly int ITEM_TYPE_NONE = 0;
        public static readonly int ITEM_TYPE_FOOD = 1;
        public static readonly int ITEM_TYPE_WEAPON = 2;
        public static readonly int ITEM_TYPE_CLOTH = 3;
        public static readonly int ITEM_TYPE_WATCH = 4;

        // Item save types
        public const int ITEM_SAVE_NONE = 0;
        public const int ITEM_SAVE_BASIC = 1;
        public const int ITEM_SAVE_POS = 2;
        public const int ITEM_SAVE_NAME = 4;
        public const int ITEM_SAVE_OWNER = 8;

        // Default item object
        public static readonly int DEFAULT_ITEM_OBJECT_ID = -719727517;

        // Weapon objects
        public static readonly Dictionary<int, int> WeaponObjects = new Dictionary<int, int>
        {
            // Melee
            { -1716189206, 170053282  }, //Knife
            { 1737195953, -1634978236 }, // Nightstick
            { -1786099057, 32653987 }, // Bat
            { -2067956739, 495450405 }, // Crowbar
            { -102973651,  1653948529 }, // hatchet
            { 419712736, 10555072 }, // Wrench

            // Handguns
            { 453432689, 1467525553 }, // Pistol
            { 1593441988, 403140669 }, // Combat pistol
            { -1716589765, -178484015}, // Pistol 50
            { -1076751822, 339962010 }, // SNSPistol
            { -771403250, 1927398017 }, // HeavyPistol
            { 137902532, -1124046276 }, // VintagePistol
            { 584646201, 905830540 }, // APPistol
            { 911657153, 1609356763 }, // StunGun
            { 1198879012, 1349014803 }, // FlareGun

            // Machine Guns
            { 324215364, -1056713654 }, // MicroSMG
            { 736523883,  -500057996 }, // SMG
            {-270015777, -473574177 }, // AssaultSMG
            { -1660422300,  -2056364402 }, // MG
            { 2144741730 ,  -739394447}, // CombatMG
            { 1627465347, 574348740 }, // Gusenberg

            // Assault Rifles
            { -1074790547, 273925117 }, // AssaultRifle
            { -2084633992, 1026431720 }, // CarbineRifle
            { -1357824103, -1707584974 }, // AdvancedRifle
            { -1063057011, -1745643757 }, //SpecialCarbine
            { 2132975508, -1288559573 }, //Bullpuprifle

            // Sniper Rifles
            { 100416529, 346403307 }, // SniperRifle
            { 205991906, -746966080 }, // HeavySniper
            { -952879014, -1711248638 }, // MarksmanRifle

            // Shotguns
            { 487013001, 689760839 }, // PumpShotgun
            { 2017895192, -675841386 }, // SawnoffShotgun
            { -1654528753, -1598212834 }, // BullpupShotgun
            { -494615257, 1255410010 }, // AssaultShotgun
            { -1466123874, 1652015642 }, // Musket
            { 984333226, -1209868881 } // HeavyShotgun

        };

        [Flags]
        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }
    }
}