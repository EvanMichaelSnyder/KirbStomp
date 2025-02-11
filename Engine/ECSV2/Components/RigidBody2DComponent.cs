using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.Components.IComponents;
using Microsoft.Xna.Framework;

namespace KirbStomp.Engine.ECSV2.Components
{
    internal class RigidBody2DComponent : Component, IUpdatableECSComponent
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;

        public RigidBody2DComponent(Vector2 initialPosition, Vector2 initialVelocity, Vector2 initialAcceleration)
        {
            position = initialPosition;
            velocity = initialVelocity;
            acceleration = initialAcceleration;
        }

        public void Update(float deltaTime)
        {
            position += velocity * deltaTime + (float)0.5 * acceleration * deltaTime * deltaTime;
            velocity += acceleration * deltaTime;
			//Debug.WriteLine($"{position.ToString()}");
        }
    }
}
