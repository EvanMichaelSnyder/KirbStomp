using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    internal class LuigiSpriteSheetMapping
    {
        public enum LuigiState
        {
            IdleRight,
            IdleLeft,
            RunningRight,
            RunningRight1,
            RunningRight2,
            RunningLeft,
            RunningLeft1,
            RunningLeft2,
            JumpingRight,
            JumpingLeft
        }
        //Mapping of Possible States to SpriteMap Locations
        public static Dictionary<LuigiState, Rectangle> StateToSpriteMap = new Dictionary<LuigiState, Rectangle>
        {
            { LuigiState.IdleRight, new Rectangle(209, 52, 16, 32) },
            { LuigiState.RunningRight, new Rectangle(239, 52, 16, 32) },
            { LuigiState.RunningRight1, new Rectangle(269, 52, 16, 32) },
            { LuigiState.RunningRight2, new Rectangle(299, 52, 16, 32) },
            { LuigiState.JumpingRight, new Rectangle(359, 52, 16, 32) },
            { LuigiState.RunningLeft, new Rectangle(150, 52, 16, 32) },
            { LuigiState.RunningLeft1, new Rectangle(120, 52, 16, 32) },
            { LuigiState.RunningLeft2, new Rectangle(90, 52, 16, 32) }

        };
        //Mapping of Possible States to SpriteMap Locations
        public static Dictionary<LuigiState, int> StateToAnimationSize = new Dictionary<LuigiState, int>
        {
            { LuigiState.IdleRight, 1},
            { LuigiState.RunningRight, 3},
            { LuigiState.JumpingRight, 1},
            { LuigiState.RunningLeft, 3}

        };
    }
}
