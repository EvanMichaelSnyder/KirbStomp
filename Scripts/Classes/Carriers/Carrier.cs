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
        internal float Damage = 10f;
        internal float impulseX = 100f;
        internal float impulseY = 100f;

        //can flip is used for bidirectional attacks where the facing direction of the parent cannot be used to determine behavior
        internal bool CanFlipX = true;
        internal bool CanFlipY = true;

        //then some special function


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
