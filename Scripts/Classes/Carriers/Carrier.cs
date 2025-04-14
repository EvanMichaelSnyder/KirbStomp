using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private CollisionObject _parent;
    
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
        public float Damage { get; private set; }
        public Vector2 Impulse { get; private set; }
        public bool CanFlipX { get; private set; }
        public bool CanFlipY { get; private set; }
        public bool HitboxAngleShearing { get; private set; }
        public float ClockTime { get; private set; }

        //then some special function

        public float getDamage()
        {
            return Damage;
        }

        public void SetDamage(float amt)
        {
            this.Damage = amt;
        }
        public void SetClock(float amt)
        {
            this.Damage = amt;
        }

        public AttackCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.attackBoxSheet, HitboxTypeEnum.Attack);
        }


        public void assignAttackDataFromXML(string name, StateEnum state)
        {
            AttackDataRepository.AttackData data = AttackDataRepository.GetAttackData(name, state);
            if (data.hasData)
            {
                Damage = data.Damage;
                Impulse = data.Impulse;
                CanFlipX = data.CanFlipX;
                CanFlipY = data.CanFlipY;
                HitboxAngleShearing = data.HitboxAngleShearing;
                ClockTime = data.ClockTime;
            }
        }

        //returns a resulting vector for collision after being fed all relevant information
        public (Vector2 impulse, DirectionEnum direction) GetImpulseVector(BodyCarrier otherObjectBodyCarrier)
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
            if (false)
            {
                ResultingImpulse = AverageWeightedRelevance * Impulse;
            }

            /*
            if(CanFlipX)
            { 
                if(otherPosition.X < ownPosition.X) 
                { 
                    ResultingImpulse.X = Math.Abs(ResultingImpulse.X) * -; 
                }
                else
                {
                    ResultingImpulse.X = Math.Abs(ResultingImpulse.X);
                }
            }
            else if (this.Parent.GetType() == typeof(Character))
            {
                if (((Character)(this.Parent)).StateMachine.State.FacingDirection == DirectionEnum.Left)
                {
                    Debug.WriteLine(((Character)(this.Parent)).StateMachine.State.FacingDirection);
                    ResultingImpulse.X = (Math.Abs(ResultingImpulse.X));
                }
                else if (((Character)(this.Parent)).StateMachine.State.FacingDirection == DirectionEnum.Right)
                {
                    Debug.WriteLine(((Character)(this.Parent)).StateMachine.State.FacingDirection);
                    ResultingImpulse.X = (Math.Abs(ResultingImpulse.X)) * -1;
                }
            }
            if (CanFlipY)
            {
                if (otherPosition.Y > ownPosition.Y) { ResultingImpulse.X *= -1; }
            }
            */
            if (this.Parent.GetType() == typeof(Character))
            {
                if (((Character)(this.Parent)).StateMachine.State.FacingDirection == DirectionEnum.Left)
                {
                    ResultingImpulse.X *= -1;
                }
            }
            Debug.WriteLine(ResultingImpulse);
            //y velocity is flipped its annoying but whatever;
            ResultingImpulse.Y *= -1;
            return (ResultingImpulse, ((Character)(this.Parent)).StateMachine.State.FacingDirection);
        }

    }


    public class PlatformCarrier : Carrier
    {
        public PlatformCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Platform);
        }
    }

    public class BoundaryCarrier : Carrier
    {
        public BoundaryCarrier()
        {
            HitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Boundary);
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
