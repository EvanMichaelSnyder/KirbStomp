using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.Managers
{
    public static class MagicNum
    {
        public static class LevelManagerMagic
        {
            public static readonly int MAX_HEIGHT_PLATFORM_APART = 350;
            public static readonly int MIN_HEIGHT_PLATFORM_APART = 250;
            public static readonly int PLATFORM_HEIGHT = 30;
            public static readonly int MIN_PLATFORM_WIDTH = 100;
            public static readonly int MAX_PLATFORM_WIDTH = 700;
            public static readonly int DIST_SPAWN_ABOVE = -100;
            public static readonly int TRIGGER_WIDTH_DOUBLE_SPAWN = 300;
            public static readonly float TIME_TO_LIVE = 20f;
            public static readonly int PLAYER_PLATFORM_WIDTH = 300;
            public static readonly Rectangle PLATFORM_SRC = new Rectangle(4, 4, 240, 140);
        }

    }
}
