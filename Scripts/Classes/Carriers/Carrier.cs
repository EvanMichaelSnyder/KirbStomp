using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Carriers
{

    public abstract class Carrier
    {
        public HitboxManager HitboxManager { get; protected set; }
        public CollisionObject Parent { get; set; }

        public bool IsDisabled { get; set; } = false;
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
        private float Damage = 10f;
        private Vector2 Impulse = new Vector2(100, 300);


        //can flip is used for bidirectional attacks where the facing direction of the parent cannot be used to determine behavior
        private bool CanFlipX = true;
        private bool CanFlipY = false;
        private bool hitboxAngleShearing = true;

        //then some special function

        public float getDamage()
        {
            return Damage;
        }

        public AttackCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.attackBoxSheet, HitboxTypeEnum.Attack);
        }

        //returns a resulting vector for collision after being fed all relevant information
        public Vector2 GetImpulseVector(BodyCarrier otherObjectBodyCarrier)
        {
            Vector2 WeightedRelevance = Impulse;
            WeightedRelevance.Normalize();
            Vector2 ownPosition = this.HitboxManager.getCentralizedPosition();
            Vector2 otherPosition = otherObjectBodyCarrier.HitboxManager.getCentralizedPosition();
            Vector2 differenceInPosition = otherPosition - ownPosition;
            Vector2 WeightedRelevanceOther = differenceInPosition;
            WeightedRelevanceOther.Normalize();


            Vector2 AverageWeightedRelevance = (WeightedRelevance + WeightedRelevanceOther) / 2.0f;

            Vector2 ResultingImpulse = Impulse;
            if (hitboxAngleShearing)
            {
                ResultingImpulse = AverageWeightedRelevance * Impulse;
            }

            if(CanFlipX)
                { 
                    if(otherPosition.X < ownPosition.X) { ResultingImpulse.X = Math.Abs(ResultingImpulse.X) * -1; } }
            else
            {
                if (otherObjectBodyCarrier.Parent.GetType() == typeof(Character))
                {
                    if(((Character)(otherObjectBodyCarrier.Parent)).StateMachine.State.FacingDirection == DirectionEnum.Left)
                    {
                        ResultingImpulse.X *= -1;
                    }
                }
            }
            if (CanFlipY)
            {
                if (otherPosition.Y > ownPosition.Y) { ResultingImpulse.X *= -1; }
            }


            //y velocity is flipped its annoying but whatever;
            ResultingImpulse.Y *= -1;
            return ResultingImpulse;
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
