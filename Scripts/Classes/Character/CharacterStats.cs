using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp
{
	struct MovementStats
	{
		float BaseWalkSpeed, BaseRunSpeed;
		float MaxRunSpeed;
		float WalkingAcceleration, RunningAcceleration, InAirAcceleration;
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
		float KnockBackScalar;
		float Gravity;
		float SpeedDecay;
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

	struct AvailableAttacks
	{
		bool GroundNeutral1, GroundNeutral2, GroundNeutral3;
		bool GroundUp, GroundDown, GroundFront, GroundBack;
		bool AerialNeutral;
		bool AerialUp, AerialDown, AerialFront, AerialBack;
		bool SpecialNeutral;
		bool SpecialUp, SpecialDown, SpecialFront, SpecialBack;
	}

}
