using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp
{
	struct MovementStats
	{
		public float BaseWalkSpeed, BaseRunSpeed;
		public float MaxRunSpeed;
		public float WalkingAcceleration, RunningAcceleration, InAirAcceleration;
		public MovementStats(float baseWalkSpeed, float baseRunSpeed, float maxRunSpeed, float walkingAccel, float runningAccel, float inAirAccel)
		{
			BaseWalkSpeed = baseWalkSpeed;
			BaseRunSpeed = baseRunSpeed;
			MaxRunSpeed = maxRunSpeed;
			WalkingAcceleration = walkingAccel;
			RunningAcceleration = runningAccel;
			InAirAcceleration = inAirAccel;
		}
		public MovementStats()
		{
			BaseWalkSpeed = 40.0f;
			BaseRunSpeed = 200.0f;
			MaxRunSpeed = 400.0f;
			WalkingAcceleration = 50.0f;
			RunningAcceleration = 50.0f;
			InAirAcceleration = 50.0f;
		}
	}
	struct PhysicsStats
	{
		public float KnockBackScalar;
		public float Gravity;
		public float SpeedDecay;
		public PhysicsStats(float knockBackScale, float gravity, float speedDecay)
		{
			KnockBackScalar = knockBackScale;
			Gravity = gravity;
			SpeedDecay = speedDecay;
		}
		public PhysicsStats()
		{
			KnockBackScalar = 1.0f;
			Gravity = 1.0f;
			SpeedDecay = 0.8f;
		}
	}
	struct UIIconData
	{
		public string spriteSheet;
		public Rectangle sourceRectangle;
		public float scale;
		public UIIconData(string spriteSheet, Rectangle sourceRectangle, float scale = 0.2f)
		{
			this.spriteSheet = spriteSheet;
			this.sourceRectangle = sourceRectangle;
			this.scale = 0.2f;
		}
	}


	struct AvailableAttacks
	{
		bool GroundNeutral1, GroundNeutral2, GroundNeutral3;
		bool GroundUp, GroundDown, GroundFront, GroundBack;
		bool AerialNeutral;
		bool AerialUp, AerialDown, AerialFront, AerialBack;
		bool SpecialNeutral;
		bool SpecialUp, SpecialDown, SpecialFront, SpecialBack;
	}
	struct CharacterStats
	{
		public string name;
		public float health;
		public Vector2 position;
		public MovementStats movement;
		public PhysicsStats physics;
		public AvailableAttacks availableAttacks;
		public UIIconData stockIcon;
		public UIIconData portraitIcon; 
		public CharacterStats(string name, float health, MovementStats movement, PhysicsStats physics, AvailableAttacks available)
		{
			this.name = name.Replace(" ", string.Empty);
			this.health = health;
			this.movement = movement;
			this.physics = physics;
			this.availableAttacks = available;
			Debug.WriteLine("Character stats without UIIconData has been initialized");
		}
		
		public CharacterStats(string name, float health, Vector2 position, MovementStats movement, PhysicsStats physics, AvailableAttacks available, UIIconData stockIcon, UIIconData portraitIcon)
		{
			this.name = name.Replace(" ", string.Empty);
			this.health = health;
			this.position = position;
			this.movement = movement;
			this.physics = physics;
			this.availableAttacks = available;
			this.stockIcon = stockIcon;
			this.portraitIcon = portraitIcon;
		}
	}
}
