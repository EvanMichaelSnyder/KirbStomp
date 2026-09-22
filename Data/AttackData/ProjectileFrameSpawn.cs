using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KirbStomp.Data.AttackData
{
    public static class ProjectileFrameSpawn
    {
        public static Dictionary<(string name, StateEnum state, int frame), (string projectileName, Vector2 offset)> projectileInfoDictionary
            = new Dictionary<(string, StateEnum, int), (string, Vector2)>()
            {
                { ("Mario", StateEnum.SpecialNeutral, 4), ("MarioFireBall", new Vector2(30, 10)) },
                { ("Link", StateEnum.SpecialNeutral, 11), ("LinkArrow", new Vector2(40, 20)) },
                { ("Link", StateEnum.SpecialForward, 2), ("Boomerang", new Vector2(40, 20)) },
                { ("Link", StateEnum.SpecialBack, 2), ("Boomerang", new Vector2(40, 50)) },
                { ("Link", StateEnum.SpecialDown, 2), ("Bomb", new Vector2(60, 20)) }, 
                { ("Link", StateEnum.AttackNeutral3, 4), ("LinkThrustSide", new Vector2(75, 10)) },
                { ("Link", StateEnum.AttackForward, 4), ("LinkSideSlash", new Vector2(90, -26)) },
                { ("Link", StateEnum.AttackBack, 4), ("LinkSideSlash", new Vector2(90, -26)) },
                { ("Link", StateEnum.AerialUp, 1), ("LinkUpThrust", new Vector2(0, -36)) },
                { ("Link", StateEnum.AerialDown, 6), ("LinkDownThrust", new Vector2(-15, 70)) },
                { ("Kirby", StateEnum.SpecialNeutral, 4), ("KirbyAirEffect", new Vector2(40, -20)) },
                { ("MegaMan", StateEnum.AttackNeutral, 1), ("MegaManMegaBuster", new Vector2(40, 20)) },
                { ("MegaMan", StateEnum.AttackNeutral, 4), ("MegaManMegaBuster", new Vector2(40, 20)) },
                { ("MegaMan", StateEnum.AttackNeutral2, 1), ("MegaManMegaBuster", new Vector2(40, 20)) },
                { ("MegaMan", StateEnum.AttackNeutral2, 4), ("MegaManMegaBuster", new Vector2(40, 20)) },
                { ("MegaMan", StateEnum.AttackNeutral2, 7), ("MegaManMegaBuster", new Vector2(40, 20)) },
                { ("MegaMan", StateEnum.AttackNeutral3, 1), ("MegaManMegaBuster", new Vector2(40, 12)) },
                { ("MegaMan", StateEnum.AttackNeutral3, 4), ("MegaManMegaBuster", new Vector2(40, 28)) },
                { ("MegaMan", StateEnum.AttackForward, 0), ("MegaManChargeShot", new Vector2(90, 0)) },
                { ("MegaMan", StateEnum.AttackBack, 0), ("MegaManChargeShot", new Vector2(90, 0)) },
                { ("MegaMan", StateEnum.AerialNeutral, 1), ("MegaManMegaBuster", new Vector2(40, 12)) },
                { ("MegaMan", StateEnum.AttackDash, 2), ("MegaManSpinEffect", new Vector2(90, -15)) },
                { ("MegaMan", StateEnum.SpecialForward, 0), ("MegaManCrashBomb", new Vector2(75, 22)) },
                { ("MegaMan", StateEnum.SpecialBack, 0), ("MegaManCrashBomb", new Vector2(75, 22)) },
                { ("MegaMan", StateEnum.SpecialDown, 4), ("MegaManLeafShield", new Vector2(50, 20)) },
                { ("MegaMan", StateEnum.SpecialNeutral, 1), ("MegaManMetalBlade", new Vector2(50, 15)) },
                { ("MegaMan", StateEnum.AerialUp, 2), ("MegaManTornado", new Vector2(90, 4)) },
                { ("MegaMan", StateEnum.AerialBack, 3), ("MegaManSlashEffect", new Vector2(75, 25)) }
            };

        // Static method to retrieve projectile info by name, state, and frame
        public static (string projectileName, Vector2 offsets) GetProjectileInfo(string name, StateEnum state, int frame)
        {
            if (projectileInfoDictionary.TryGetValue((name, state, frame), out var projectileInfo))
            {
                return projectileInfo;
            }
            else
            {
                return ("No Projectile", new Vector2(0, 0));
            }
        }
    }
}
