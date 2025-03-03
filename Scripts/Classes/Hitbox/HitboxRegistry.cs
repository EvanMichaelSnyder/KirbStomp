using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Scripts.Classes.Hitbox
{
    // Global Hitbox Registry to track all active hitboxes
    public static class HitboxRegistry
    {
        private static Dictionary<HitboxTypeEnum, List<HitboxManager>> _hitboxManagers =
            new Dictionary<HitboxTypeEnum, List<HitboxManager>>();

        static HitboxRegistry()
        {
            foreach (HitboxTypeEnum type in Enum.GetValues(typeof(HitboxTypeEnum)))
            {
                _hitboxManagers[type] = new List<HitboxManager>();
            }
        }

        public static void Register(HitboxManager manager)
        {
            HitboxTypeEnum type = manager.GetHitboxTypeEnum();
            _hitboxManagers[type].Add(manager);
        }

        public static void Unregister(HitboxManager manager)
        {
            HitboxTypeEnum type = manager.GetHitboxTypeEnum();
            _hitboxManagers[type].Remove(manager);
        }

        public static List<HitboxManager> GetHitboxManagersOfType(HitboxTypeEnum type)
        {
            return _hitboxManagers[type];
        }
    }
}
