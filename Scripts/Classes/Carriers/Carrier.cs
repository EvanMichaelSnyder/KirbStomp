using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Carriers
{

    public abstract class Carrier
    {
        public HitboxManager HitboxManager { get; protected set; }
        public CollisionObject Parent { get; set; }
    }

    public class BodyCarrier : Carrier
    {
        public BodyCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Body);
        }
    }

    public class AttackCarrier : Carrier
    {
        public int Damage = 10;
        public AttackCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.attackBoxSheet, HitboxTypeEnum.Attack);
        }
    }


    public class PlatformCarrier : Carrier
    {
        public PlatformCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Platform);
        }
    }

    public class ItemCarrier : Carrier
    {
        public ItemCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Item);
        }
    }
}
