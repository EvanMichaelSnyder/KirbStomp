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
                { ("Mario", StateEnum.SpecialNeutral, 5), ("MarioFireBall", new Vector2(30, 10)) },
                { ("Link", StateEnum.SpecialNeutral, 12), ("LinkArrow", new Vector2(40, 20)) },
                { ("Link", StateEnum.SpecialForward, 3), ("Boomerang", new Vector2(40, 20)) },
                { ("Link", StateEnum.SpecialBack, 3), ("Boomerang", new Vector2(40, 50)) },
                { ("Link", StateEnum.SpecialDown, 3), ("Bomb", new Vector2(40, 50)) }, 
                { ("Link", StateEnum.AttackNeutral3, 5), ("LinkThrustSide", new Vector2(75, 10)) },
                { ("Link", StateEnum.AttackForward, 5), ("LinkSideSlash", new Vector2(90, -26)) },
                { ("Link", StateEnum.AttackBack, 5), ("LinkSideSlash", new Vector2(90, -26)) },
                { ("Link", StateEnum.AerialUp, 2), ("LinkUpThrust", new Vector2(0, -36)) },
                { ("Link", StateEnum.AerialDown, 7), ("LinkDownThrust", new Vector2(-15, 70)) }
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
