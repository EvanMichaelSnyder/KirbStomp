using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static KirbStomp.StateEnum;
using static KirbStomp.EventType;
using static KirbStomp.DirectionEnum;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace KirbStomp.StateMachine
{
	public class CharacterState
	{
		private StateEnum _currentState = StateEnum.Idle;
		private int _animationFrame = 0;
		private float _elapsedTime = 0;
		private DirectionEnum _facingDirection = DirectionEnum.Right;
		private DirectionEnum _movementDirection = DirectionEnum.None;
		private bool _isGrounded = true;
		private int _jumpsLeft = 2;
		private DirectionEnum _desiredAttackDirection = DirectionEnum.None;
		private DirectionEnum _desiredMovementDirection = DirectionEnum.None;
        internal bool fancyPlatformCollisionFlag = false;

        public DirectionEnum DesiredMovementDirection { get; set; }
		public DirectionEnum DesiredAttackDirection { get; set; }

		public bool continueMoving = false;

		public StateEnum CurrentState
		{
			get => _currentState;
			internal set  // Only allow modification within state machine
			{
				if (_currentState != value)
				{
					_animationFrame = 0;  // Reset frame on state change
					_currentState = value;
				}
			}
		}
		public int GetFrameIndex()
		{
			return _animationFrame;
		}
		public void ResetFrameIndex()
		{
			_animationFrame = 0;
		}
		public void IncrementFrameIndex()
		{
			_animationFrame++;
		}


		public float getElapsedTime()
		{
			return _elapsedTime;
		}
		public void resetElapsedTime()
		{
			_elapsedTime = 0;
		}
		public void addToElapsedTime(float time)
		{
			_elapsedTime += time;
		}

		public DirectionEnum FacingDirection
		{
			get => _facingDirection;
			set
			{
				_facingDirection = value;
			}
		}

		public DirectionEnum MovementDirection
		{
			get => _movementDirection;
			internal set => _movementDirection = value;  // Only state machine controls this
		}

		public bool IsGrounded
		{
			get => _isGrounded;
			internal set => _isGrounded = value;
		}

		public int JumpsLeft
		{
			get => _jumpsLeft;
			private set => _jumpsLeft = Math.Max(0, value);  // Prevent negative jumps
		}
		internal void DecrementJumps() { JumpsLeft--; }

		// Intents (publicly settable but validated)

		public void ResetJumps() => JumpsLeft = 2;

		public DirectionEnum ConvertToRelativeAttackDirection(DirectionEnum direction)
		{
			//takes an input direction like left or right and the current facing direction and tells you if it is forward or backward
			if (direction == DirectionEnum.Right)
			{
				if (_facingDirection == direction) { return DirectionEnum.Forward; }
				else { return DirectionEnum.Back; }
			}
			else if (direction == DirectionEnum.Left)
			{
				if (_facingDirection == direction) { return DirectionEnum.Forward; }
				else { return DirectionEnum.Back; }
			}
			else
			{
				return direction;
			}


		}
		public DirectionEnum ConvertToRelativeMovementDirection(DirectionEnum direction)
		{
			//takes an input direction like left or right and the current movement direction and tells you if it is forward or backward
			if (direction == DirectionEnum.Right)
			{
				if (_movementDirection == direction) { return DirectionEnum.Forward; }
				else { return DirectionEnum.Back; }
			}
			else if (direction == DirectionEnum.Left)
			{
				if (_movementDirection == direction) { return DirectionEnum.Forward; }
				else { return DirectionEnum.Back; }
			}
			else
			{
				return direction;
			}


		}


		#region Transition Handlers
		public static void EnterAttack(CharacterState current)
		{
			if (current.IsGrounded)//on ground
			{
				//combo stuffs
				if (current.CurrentState == Sprint ) { current.CurrentState = AttackDash; }
				else if (current.CurrentState == AttackNeutral) { current.CurrentState = AttackNeutral2; }
				else if (current.CurrentState == AttackNeutral2) { current.CurrentState = AttackNeutral3; }
				else
				{
					switch (current.ConvertToRelativeAttackDirection(current.DesiredAttackDirection))
					{
						case None:
							current.CurrentState = AttackNeutral;
							break;
						case Up:
							current.CurrentState = AttackUp;
							break;
						case Down:
							current.CurrentState = AttackDown;
							break;
						case Forward:
							current.CurrentState = AttackForward;
							break;
						case Back:
							current.CurrentState = AttackBack;
							break;
						default:
							throw new Exception("Attack Called without Direction");
					}
				}
			}
			else if (!current.IsGrounded)//In Air
			{
				switch (current.ConvertToRelativeAttackDirection(current.DesiredAttackDirection))
				{
					case None:
						current.CurrentState = AerialNeutral;
						break;
					case Up:
						current.CurrentState = AerialUp;
						break;
					case Down:
						current.CurrentState = AerialDown;
						break;
					case Forward:
						current.CurrentState = AerialForward;
						break;
					case Back:
						current.CurrentState = AerialBack;
						break;
					default:
						throw new Exception("Aerial Called without Direction");
				}
			}
		}

		public static void EnterSpecial(CharacterState current)
		{
			switch (current.ConvertToRelativeAttackDirection(current.DesiredAttackDirection))
			{
				case None:
					current.CurrentState = SpecialNeutral;
					break;
				case Up:
					current.CurrentState = SpecialUp;
					break;
				case Down:
					current.CurrentState = SpecialDown;
					break;
				case Forward:
					current.CurrentState = SpecialForward;
					break;
				case Back:
					current.CurrentState = SpecialBack;
					break;
				default:
					throw new Exception("Special Called without Direction");
			}
		}
		public static void EnterJump(CharacterState current)
		{
			//because the dictionary effectively makes a dictionary check we can assume at this point that jump is mandatory
			if (!(current.JumpsLeft <= 0))
			{
				current.CurrentState = Jump;
				current.DecrementJumps();

				//for now I will personally set isGrounded false this should likely be done by something else later
				current.IsGrounded = false;
				current.fancyPlatformCollisionFlag = true;
			}
			else
			{
				Debug.WriteLine("you should not be doing such unregulated vaulting my good sir how darest you vault thyself when you are unable (you're out of jumps)");
			}
		}
		public static void EnterLanding(CharacterState current)
		{
			current.fancyPlatformCollisionFlag = false;
            if (current.CurrentState == FreeFall)
			{
				current.CurrentState = LayingDown;
				current.MovementDirection = None;
			}
			else
			{
				current.CurrentState = Landing;
			}


            //the hitGround will reset our jumps
        }
        public static void EnterKnockedBack(CharacterState current)
        {
            current.fancyPlatformCollisionFlag = true;
            current.CurrentState = KnockedBack;
        }
        public static void EnterIdle(CharacterState current)
        {
			current.DesiredAttackDirection = None;
			if(current.CurrentState==KnockedBack)
			{
				current.CurrentState = Ragdolled;
			}
            else if (current.IsGrounded) { current.CurrentState = Idle; }
            else if (!current.IsGrounded) { current.CurrentState = AirIdle; }
            else { throw new Exception("WHAT HAVE YOU DONE"); }
        }
        public static void EnterFalling(CharacterState current)
        {
            //the specificity of this transition means we should check it
            if (current.CurrentState == SpecialUp)
            {
                current.CurrentState = FreeFall;
            }
            else
            {
                //this can trigger in console state machine because during up special you could in theory hit the ground
                Debug.WriteLine("You should not be entering Freefall avoid hitting the ground during upSpecial");
            }
        }
        public static void EnterRecover(CharacterState current)
        {
            current.CurrentState = Recover;
        }
        public static void EnterMovement(CharacterState current)
        {
            //should only be called on basic
            if (!current.IsGrounded)
            {
                if (current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) != Forward)
                {
                    current.MovementDirection = current.DesiredMovementDirection;
                }
                current.CurrentState = AirMove;
            }
            else if (current.IsGrounded)
            {
				Debug.WriteLine(current.CurrentState);
                if (current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) != Forward)
                {
                    current.MovementDirection = current.DesiredMovementDirection;
                    current.FacingDirection = current.DesiredMovementDirection;
                }
                current.MovementDirection = current.DesiredMovementDirection;
                current.FacingDirection = current.DesiredMovementDirection;
				current.CurrentState = Run;
            }
            else
            {
                throw new Exception("Enter Movement Not Implemented");
            }
        }
        public static void AlterMovement(CharacterState current)
        {
            //This is very complicated but it is simpler to understand when you realize that we only change our facing direction or speedSetting
            if (!current.IsGrounded)
            {
                if (!(current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward))
                {
                    current.MovementDirection = current.DesiredMovementDirection;
                    // in air we only change movement direction
                    if(current.CurrentState==Idle||current.CurrentState == Walk || current.CurrentState == Run || current.CurrentState == Sprint)
                    {
                        current.CurrentState = StateEnum.AirMove;
                    }
                }
            }
            else if (current.IsGrounded)
            {
                switch (current.CurrentState)
                {
                    case Walk:
                        if (current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward)
                        {
                            //will later check speed to see if we should increase for now we will assume
                            //current.CurrentState = Run;
                        }
                        else
                        {
                            //current.CurrentState = Run;
                            current.MovementDirection = current.DesiredMovementDirection;
                            current.FacingDirection = current.DesiredMovementDirection;
                        }
                        break;
                    case Run:
                        if (current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward)
                        {
                            //will later check speed to see if we should increase for now we will assume
                            current.CurrentState = Sprint;
                        }
                        else
                        {
                            current.CurrentState = Sprint;
                            current.MovementDirection = current.DesiredMovementDirection;
                            current.FacingDirection = current.DesiredMovementDirection;
                        }
                        break;
                    case Sprint:
                        if (current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward)
                        {
                            //will later check speed to see if we should increase for now we will assume
                            current.CurrentState = Sprint;
                            //no increase we are already sprinting
                        }
                        else
                        {
                            current.CurrentState = SlideTurn;
                            current.MovementDirection = None;
                            current.FacingDirection = current.DesiredMovementDirection;
                        }
                        break;
                }
            }

		}
        #endregion

    }

}
