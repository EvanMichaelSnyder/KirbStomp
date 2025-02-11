using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework;

namespace KirbStomp.Engine.ECSV2.Components
{
    public class RigidBody2DComponent : Component
    {
        public enum BodyType
        {
            STATIC,
            DYNAMIC,
        }
        private Vector2 velocity;
        private Vector2 acceleration;
        private BodyType bodyType;

        public RigidBody2DComponent(BodyType type = BodyType.DYNAMIC)
        {
            this.bodyType = type;
            this.acceleration = new Vector2 (0, 0);
            this.velocity = new Vector2 (0, 0);
        }

        public void Update(float deltaTime)
        {
            if(this.bodyType == BodyType.DYNAMIC)
            {
                Vector2 currentPos = this.entity.getPosition();
                currentPos += velocity * deltaTime + (float)0.5 * acceleration * deltaTime * deltaTime;
                this.entity.setPosition(currentPos);
                velocity += acceleration * deltaTime;
            }
			//Debug.WriteLine($"{position.ToString()}");
        }

        public void setVelocity(Vector2 velocity)
        {
            this.velocity.X = velocity.X;
            this.velocity.Y = velocity.Y;
        }

        public void setAcceleration(Vector2 acceleration)
        {
            this.acceleration.X = acceleration.X;
            this.acceleration.Y = acceleration.Y;
        }

        public void setXAcceleration(float xAcceleration)
        {
            this.acceleration.X = xAcceleration;
        }

        public void setYAcceleration(float yAcceleration)
        {
            this.acceleration.Y = yAcceleration;
        }

        public void setXVelocity(float xVelocity)
        {
            this.velocity.X = xVelocity;
        }
        public void setYVelocity(float yVelocity)
        {
            this.velocity.Y = yVelocity;
        }

        public Vector2 getAcceleration()
        {
            //copy of accel, dont allow to adjust vec
            return new Vector2(this.acceleration.X, this.acceleration.Y);
        }

        public Vector2 getVelocity()
        {
            //copy
            return new Vector2(this.velocity.X, this.velocity.Y);
        }

        public void setBodyType(BodyType type)
        {
            this.bodyType = type;
        }

        public BodyType getBodyType()
        {
            return this.bodyType;
        }
    }
}
