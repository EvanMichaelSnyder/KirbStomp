using System;
using System.Collections.Generic;
using KirbStomp;
using static KirbStomp.StateEnum;
using static KirbStomp.EventType;
using static KirbStomp.DirectionEnum;
using static KirbStomp.StateMachine.CharacterState;
using System.Diagnostics;
using KirbStomp.StateMachine;

namespace KirbStomp{
	public class CharacterStateMachine
	{
		private Dictionary<(StateEnum, EventType), Action<CharacterState>> _transitions =
			new Dictionary<(StateEnum, EventType), Action<CharacterState>>();

		public CharacterState State = new CharacterState();

		public CharacterStateMachine()
		{
			InitializeTransitions();
			State.CurrentState = StateEnum.Idle; // Initial state
		}

		public void PerformBehavior()
		{
			Console.WriteLine("Performing Behavior of State: " + State.CurrentState + " on Frame: " + State.GetFrameIndex() + "\n");
		}

		/*  Template Include all Events and what the expected result is
			AddTransition(, TryAttack, EnterAttack);
			AddTransition(, TrySpecial, EnterSpecial);
			AddTransition(, TryJump, EnterJump);
			AddTransition(, TryMove, EnterMovement);
			AddTransition(, GotHit, EnterKnockedBack);
			AddTransition(, HitGround, EnterLanding);
			AddTransition(, EndOfState, EnterIdle);
		*/

		private void InitializeTransitions()
		{
			#region Movement
			AddTransition(Idle, TryAttack, EnterAttack);
			AddTransition(Idle, TrySpecial, EnterSpecial);
			AddTransition(Idle, TryJump, EnterJump);
			AddTransition(Idle, TryMove, EnterMovement);//goes up a move level/change direction
			AddTransition(Idle, GotHit, EnterKnockedBack);

			AddTransition(Walk, TryAttack, EnterAttack);
			AddTransition(Walk, TrySpecial, EnterSpecial);
			AddTransition(Walk, TryJump, EnterJump);
			//AddTransition(Walk, TryMove, AlterMovement);//goes up or down a move level/change direction
			AddTransition(Walk, GotHit, EnterKnockedBack);

			AddTransition(Run, TryAttack, EnterAttack);
			AddTransition(Run, TrySpecial, EnterSpecial);
			AddTransition(Run, TryJump, EnterJump);
			//AddTransition(Run, TryMove, AlterMovement);//goes up a move level/change direction
			AddTransition(Run, GotHit, EnterKnockedBack);

			AddTransition(Sprint, TryAttack, EnterAttack);
			AddTransition(Sprint, TrySpecial, EnterSpecial);
			AddTransition(Sprint, TryJump, EnterJump);
			//AddTransition(Sprint, TryMove, AlterMovement);//only can be used for new direction but if buggy could try to go up a move level
			AddTransition(Sprint, GotHit, EnterKnockedBack);

			AddTransition(SlideTurn, GotHit, EnterKnockedBack);
			AddTransition(SlideTurn, EndOfState, EnterIdle);//This might need to change
			//AddTransition(SlideTurn, TryMove, EnterMovement);

			//No crouching

			AddTransition(Jump, GotHit, EnterKnockedBack);
			AddTransition(Jump, HitGround, EnterLanding);
			AddTransition(Jump, EndOfState, EnterIdle); //this may seem confusing but if at the end of a frame we go from jump to idle while holding a direction on the next frame we immediately go into a movement
			AddTransition(Jump, TryMove, EnterMovement);
			AddTransition(Jump, TryAttack, EnterAttack);
			AddTransition(Jump, TrySpecial, EnterSpecial);
			AddTransition(Jump, TryJump, EnterJump);

			AddTransition(AirIdle, TryAttack, EnterAttack);
			AddTransition(AirIdle, TrySpecial, EnterSpecial);
			AddTransition(AirIdle, TryJump, EnterJump);
			AddTransition(AirIdle, TryMove, EnterMovement);
			AddTransition(AirIdle, GotHit, EnterKnockedBack);
			AddTransition(AirIdle, HitGround, EnterLanding);

			AddTransition(AirMove, TryAttack, EnterAttack);
			AddTransition(AirMove, TrySpecial, EnterSpecial);
			AddTransition(AirMove, TryJump, EnterJump);
			AddTransition(AirMove, TryMove, AlterMovement);//for direction
			AddTransition(AirMove, GotHit, EnterKnockedBack);
			AddTransition(AirMove, HitGround, EnterLanding);

			AddTransition(Landing, GotHit, EnterKnockedBack);
			AddTransition(Landing, EndOfState, EnterIdle);
            AddTransition(Landing, TryMove, EnterMovement);//for direction

            #endregion

            #region Attack
            //neutral
            AddTransition(AttackNeutral, TryAttack, EnterAttack);
			AddTransition(AttackNeutral, GotHit, EnterKnockedBack);
			AddTransition(AttackNeutral, EndOfState, EnterIdle);

			AddTransition(AttackNeutral2, TryAttack, EnterAttack);
			AddTransition(AttackNeutral2, GotHit, EnterKnockedBack);
			AddTransition(AttackNeutral2, EndOfState, EnterIdle);

			AddTransition(AttackNeutral3, GotHit, EnterKnockedBack);
			AddTransition(AttackNeutral3, EndOfState, EnterIdle);

			//NonNeutral
			AddTransition(AttackForward, GotHit, EnterKnockedBack);
			AddTransition(AttackForward, EndOfState, EnterIdle);

			AddTransition(AttackBack, GotHit, EnterKnockedBack);
			AddTransition(AttackBack, EndOfState, EnterIdle);

			AddTransition(AttackUp, GotHit, EnterKnockedBack);
			AddTransition(AttackUp, EndOfState, EnterIdle);

			AddTransition(AttackDown, GotHit, EnterKnockedBack);
			AddTransition(AttackDown, EndOfState, EnterIdle);

			AddTransition(AttackDash, GotHit, EnterKnockedBack);
			AddTransition(AttackDash, EndOfState, EnterIdle);

			#endregion

			#region Aerial
			AddTransition(AerialNeutral, HitGround, EnterLanding);
			AddTransition(AerialNeutral, GotHit, EnterKnockedBack);
			AddTransition(AerialNeutral, EndOfState, EnterIdle);

			AddTransition(AerialForward, HitGround, EnterLanding);
			AddTransition(AerialForward, GotHit, EnterKnockedBack);
			AddTransition(AerialForward, EndOfState, EnterIdle);

			AddTransition(AerialBack, HitGround, EnterLanding);
			AddTransition(AerialBack, GotHit, EnterKnockedBack);
			AddTransition(AerialBack, EndOfState, EnterIdle);

			AddTransition(AerialUp, HitGround, EnterLanding);
			AddTransition(AerialUp, GotHit, EnterKnockedBack);
			AddTransition(AerialUp, EndOfState, EnterIdle);

			AddTransition(AerialDown, HitGround, EnterLanding);
			AddTransition(AerialDown, GotHit, EnterKnockedBack);
			AddTransition(AerialDown, EndOfState, EnterIdle);
			#endregion

			#region Special
			AddTransition(SpecialNeutral, GotHit, EnterKnockedBack);
			AddTransition(SpecialNeutral, EndOfState, EnterIdle);

			AddTransition(SpecialForward, GotHit, EnterKnockedBack);
			AddTransition(SpecialForward, EndOfState, EnterIdle);

			AddTransition(SpecialBack, GotHit, EnterKnockedBack);
			AddTransition(SpecialBack, EndOfState, EnterIdle);

			AddTransition(SpecialUp, GotHit, EnterKnockedBack);
			AddTransition(SpecialUp, EndOfState, EnterFalling);	//VERY IMPORTANT

			AddTransition(SpecialDown, GotHit, EnterKnockedBack);
			AddTransition(SpecialDown, EndOfState, EnterIdle);
			#endregion

			#region Hits
			//CANNOT MOVE OR DO ANYTHING
			//AddTransition(KnockedBack, HitGround, EnterLanding);
			AddTransition(KnockedBack, EndOfState, EnterIdle);

			AddTransition(Ragdolled, TryJump, EnterJump);
			//AddTransition(KnockedBack, TryMove, EnterMovement);
			AddTransition(Ragdolled, GotHit, EnterKnockedBack);
			AddTransition(Ragdolled, HitGround, EnterLanding);
			

			AddTransition(LayingDown, TryAttack, EnterRecover);
			AddTransition(LayingDown, TrySpecial, EnterRecover);
			AddTransition(LayingDown, TryJump, EnterRecover);
			AddTransition(LayingDown, TryMove, EnterRecover);//enters recover
			AddTransition(LayingDown, GotHit, EnterKnockedBack);

			AddTransition(Recover, GotHit, EnterKnockedBack);
			AddTransition(Recover, EndOfState, EnterIdle);


			//AddTransition(FreeFall, TryMove, EnterMovement);//this likely should work
			AddTransition(FreeFall, GotHit, EnterKnockedBack);
			AddTransition(FreeFall, HitGround, EnterLanding);//laying down
			#endregion
		}

		private void AddTransition(StateEnum from, EventType eventType, Action<CharacterState> handler)
		{
			_transitions[(from, eventType)] = handler;
		}

		private void ApplyHitGround()
		{
			if (State.CurrentState != StateEnum.SpecialUp && State.CurrentState != StateEnum.Jump)//this is so that early frames of jump where you are still on the ground dont reset jumps
			{
				State.IsGrounded = true;
				State.ResetJumps();
				// Console.WriteLine("EventHitGround: This may not necessarily result in a new Enum State");
			}
			else
			{
				// Console.WriteLine("EventHitGround: Effects not applied because you are in odd state for this event");
			}

		}
		private void ApplyEndOfState()
		{
			// Console.WriteLine("EventEndOfState: This may not necessarily result in a new Enum State but it really should");
		}

		public void HandleEvent(EventType eventType)
		{
			var key = (State.CurrentState, eventType);
			if (_transitions.TryGetValue(key, out var handler))
			{
				handler(State);
			}
			else
			{
				// Console.WriteLine("\n Could not find suitable mapping for " + key + "\n");
			}
			// Handle non-permissive events last this way
			if (eventType == EventType.EndOfState) { ApplyEndOfState(); }//possibly unused but you never know
			if (eventType == EventType.HitGround) { ApplyHitGround(); }//basic collision snapping
		}
	}
}
