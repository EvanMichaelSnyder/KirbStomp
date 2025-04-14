using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.Collision
{

    public struct CollisionContext
    {
        public CollisionObject Self;
        public CollisionObject Other;
        public HitboxTypeEnum SelfType;
        public HitboxTypeEnum OtherType;
        public Rectangle Intersection;
        public GameTime GameTime;

        public CollisionContext(CollisionObject self, CollisionObject other, HitboxTypeEnum selfType, HitboxTypeEnum otherType, Rectangle intersection)
        {
            Self = self;
            Other = other;
            SelfType = selfType;
            OtherType = otherType;
            Intersection = intersection;
            
        }
        public CollisionContext(CollisionObject self, CollisionObject other, HitboxTypeEnum selfType, HitboxTypeEnum otherType, Rectangle intersection, GameTime gameTime)
        {
            Self = self;
            Other = other;
            SelfType = selfType;
            OtherType = otherType;
            Intersection = intersection;
            GameTime = gameTime;
        }

        public CollisionContext SwapPerspective() => new CollisionContext(
            Other, Self, OtherType, SelfType, Intersection, GameTime
        );

        public override string ToString()
        {
            return $"CollisionContext: [Self={Self}, Other={Other}, SelfType={SelfType}, OtherType={OtherType}, Intersection={Intersection}]";
        }

    }

    public class CollisionObject : PhysicsComponent
    {
        protected Dictionary<(HitboxTypeEnum, HitboxTypeEnum), Action<CollisionObject, CollisionContext>>
        _collisionHandlers = new();

        public PhysicsComponent Physics { get; } = new PhysicsComponent();
        public bool IsActive { get; internal set; }
        public List<Carrier> Carriers { get; } = new List<Carrier>();
        public float ClockTime 
            ;

        public void RegisterCollisionResponse(HitboxTypeEnum selfType,
                                           HitboxTypeEnum otherType,
                                           Action<CollisionObject, CollisionContext> handler)
        {
            _collisionHandlers[(selfType, otherType)] = handler;
        }

        public virtual void HandleCollision(CollisionContext context)
        {
            /*
            Debug.WriteLine("Handling Collision");
            
            foreach (var key in _collisionHandlers.Keys)
            {
                Debug.WriteLine($"Registered handler: {key}");
            }
            */
            if (ClockTime <= 0)
            {
                if (_collisionHandlers.TryGetValue((context.SelfType, context.OtherType),
                    out var handler))
                {
                    handler(this, context);
                }
            }
        }
    }
}