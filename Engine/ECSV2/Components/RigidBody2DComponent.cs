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
			this.acceleration = new Vector2(0, 0);
			this.velocity = new Vector2(0, 0);
		}

		public void Update(float deltaTime)
		{
			if (this.bodyType == BodyType.DYNAMIC)
			{
				Vector2 currentPos = this.entity.GetPosition();
				currentPos += velocity * deltaTime + (float)0.5 * acceleration * deltaTime * deltaTime;
				this.entity.SetPosition(currentPos);
				velocity += acceleration * deltaTime;
			}
			//Debug.WriteLine($"{position.ToString()}");
		}

		public void SetVelocity(Vector2 velocity)
		{
			this.velocity.X = velocity.X;
			this.velocity.Y = velocity.Y;
		}

		public void SetAcceleration(Vector2 acceleration)
		{
			this.acceleration.X = acceleration.X;
			this.acceleration.Y = acceleration.Y;
		}

		public void SetXAcceleration(float xAcceleration)
		{
			this.acceleration.X = xAcceleration;
		}

		public void SetYAcceleration(float yAcceleration)
		{
			this.acceleration.Y = yAcceleration;
		}

		public void SetXVelocity(float xVelocity)
		{
			this.velocity.X = xVelocity;
		}
		public void SetYVelocity(float yVelocity)
		{
			this.velocity.Y = yVelocity;
		}

		public Vector2 GetAcceleration()
		{
			//copy of accel, dont allow to adjust vec
			return new Vector2(this.acceleration.X, this.acceleration.Y);
		}
		public float GetYAcceleration()
		{
			return this.acceleration.Y;
		}
		public float GetXAcceleration()
		{
			return this.acceleration.X;
		}
		public Vector2 GetVelocity()
		{
			//copy
			return new Vector2(this.velocity.X, this.velocity.Y);
		}
		public float GetYVelocity()
		{
			return this.velocity.Y;
		}
		public float GetXVelocity()
		{
			return this.velocity.X;
		}

		public void SetBodyType(BodyType type)
		{
			this.bodyType = type;
		}

		public BodyType GetBodyType()
		{
			return this.bodyType;
		}
	}
}