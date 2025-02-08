using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework;

namespace KirbStomp.ECSV2.Components
{
    internal class RigidBody2DComponent : IECSComponent, IUpdatableECSComponent
    {
        public UInt32 entityID;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;

        public RigidBody2DComponent(UInt32 entityID, Vector2 initialPosition, Vector2 initialVelocity, Vector2 initialAcceleration)
        {
            this.entityID = entityID;
            position = initialPosition;
            velocity = initialVelocity;
            acceleration = initialAcceleration;
        }

        public void Update(float deltaTime)
        {
            position += velocity * deltaTime + (float)0.5 * acceleration * deltaTime * deltaTime;
        }
    }
}
