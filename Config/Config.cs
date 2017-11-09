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

        // Vehicle owner types
        public static readonly int VEHICLE_OWNER_NONE = 0;
        public static readonly int VEHICLE_OWNER_PLAYER = 1;
        public static readonly int VEHICLE_OWNER_GROUP = 2;

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