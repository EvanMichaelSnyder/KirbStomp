using System;
using System.Collections.Generic;
using KirbStomp;
using static KirbStomp.StateEnum;
using static KirbStomp.EventType;
using static KirbStomp.DirectionEnum;
using System.Diagnostics;

namespace KirbStomp.StateMachine
{
    public class CharacterState
    {
        private StateEnum _currentState = Idle;
        private int _animationFrame = 0;
        private DirectionEnum _facingDirection = Right;
        private DirectionEnum _movementDirection = Right;
        private bool _isGrounded = true;
        private bool _changeDirectionPermission = true;
        private int _jumpsLeft = 2;
        /*
        private AccelEnum _accel = AccelEnum.Same;

        public AccelEnum Acceleration
        {
            get => _accel;
            internal set => _accel = value;
        }
        */
        // Public properties with controlled access
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

        public int AnimationFrame
        {
            get => _animationFrame;
            internal set => _animationFrame++;
        }

        public DirectionEnum FacingDirection
        {
            get => _facingDirection;
            set
            {
                if (_changeDirectionPermission)
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

        public bool ChangeDirectionPermission
        {
            get => _changeDirectionPermission;
            set => _changeDirectionPermission = value;
        }

        public int JumpsLeft
        {
            get => _jumpsLeft;
            private set => _jumpsLeft = Math.Max(0, value);  // Prevent negative jumps
        }
        internal void DecrementJumps() { JumpsLeft--; }

        // Intents (publicly settable but validated)
        private DirectionEnum _desiredAttackDirection = Right;
        public DirectionEnum DesiredAttackDirection
        {
            get => _desiredAttackDirection;
            set => _desiredAttackDirection = value;
        }

        private DirectionEnum _desiredMovementDirection = Right;
        public DirectionEnum DesiredMovementDirection
        {
            get => _desiredMovementDirection;
            set => _desiredMovementDirection = value;
        }

        public void ResetJumps() => JumpsLeft = 2;

        public DirectionEnum ConvertToRelativeAttackDirection(DirectionEnum direction)
        {
            //takes an input direction like left or right and the current facing direction and tells you if it is forward or backward
            if (direction == Right)
            {
                if (_facingDirection == direction) { return Forward; }
                else { return Back; }
            }
            else if (direction == Left)
            {
                if (_facingDirection == direction) { return Forward; }
                else { return Back; }
            }
            else
            {
                return direction;
            }


        }
        public DirectionEnum ConvertToRelativeMovementDirection(DirectionEnum direction)
        {
            //takes an input direction like left or right and the current movement direction and tells you if it is forward or backward
            if (direction == Right)
            {
                if (_movementDirection == direction) { return Forward; }
                else { return Back; }
            }
            else if (direction == Left)
            {
                if (_movementDirection == direction) { return Forward; }
                else { return Back; }
            }
            else
            {
                return direction;
            }


        }
    }


    public class StateMachine
    {
        private Dictionary<(StateEnum, EventType), Action<CharacterState>> transitions =
            new Dictionary<(StateEnum, EventType), Action<CharacterState>>();

        public CharacterState State = new CharacterState();

        public StateMachine()
        {
            InitializeTransitions();
            State.CurrentState = Idle; // Initial state
        }

        public void performBehavior()
        {
            State.AnimationFrame++;
            Console.WriteLine("Performing Behavior of State: " + State.CurrentState + " on Frame: " + State.AnimationFrame + "\n");
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
            AddTransition(Walk, TryMove, AlterMovement);//goes up or down a move level/change direction
            AddTransition(Walk, GotHit, EnterKnockedBack);

            AddTransition(Run, TryAttack, EnterAttack);
            AddTransition(Run, TrySpecial, EnterSpecial);
            AddTransition(Run, TryJump, EnterJump);
            AddTransition(Run, TryMove, AlterMovement);//goes up a move level/change direction
            AddTransition(Run, GotHit, EnterKnockedBack);

            AddTransition(Sprint, TryAttack, EnterAttack);
            AddTransition(Sprint, TrySpecial, EnterSpecial);
            AddTransition(Sprint, TryJump, EnterJump);
            AddTransition(Sprint, TryMove, AlterMovement);//only can be used for new direction but if buggy could try to go up a move level
            AddTransition(Sprint, GotHit, EnterKnockedBack);

            AddTransition(SlideTurn, GotHit, EnterKnockedBack);
            AddTransition(SlideTurn, EndOfState, EnterIdle);//This might need to change

            //No crouching

            AddTransition(Jump, GotHit, EnterKnockedBack);
            AddTransition(Jump, HitGround, EnterLanding);
            AddTransition(Jump, EndOfState, EnterIdle); //this may seem confusing but if at the end of a frame we go from jump to idle while holding a direction on the next frame we immediately go into a movement

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
            AddTransition(SpecialUp, EndOfState, EnterFalling);    //VERY IMPORTANT

            AddTransition(SpecialDown, GotHit, EnterKnockedBack);
            AddTransition(SpecialDown, EndOfState, EnterIdle);
            #endregion

            #region Hits
            /*  This is def not done we need more details
            //CANNOT MOVE OR DO ANYTHING
            AddTransition(Ragdolled, HitGround, EnterLanding);
            AddTransition(Ragdolled, EndOfState, EnterKnockedBack);

            AddTransition(KnockedBack, TryJump, EnterJump);
            //AddTransition(KnockedBack, TryMove, EnterMovement);
            AddTransition(KnockedBack, GotHit, EnterKnockedBack);
            AddTransition(KnockedBack, HitGround, EnterLanding);
            AddTransition(KnockedBack, EndOfState, EnterIdle);
            */

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
            transitions[(from, eventType)] = handler;
        }

        private void ApplyHitGround()
        {
            State.IsGrounded = true;
            State.ResetJumps();
            Console.WriteLine("EventHitGround: This may not necessarily result in a new Enum State");
        }
        private void ApplyEndOfState()
        {
            Console.WriteLine("EventEndOfState: This may not necessarily result in a new Enum State but it really should");
        }

        public void HandleEvent(EventType eventType)
        {
            // Handle non-permissive events first
            if (eventType == EndOfState) { ApplyEndOfState(); }//possibly unused but you never know
            if (eventType == HitGround) { ApplyHitGround(); }//basic collision snapping


            var key = (State.CurrentState, eventType);
            if (transitions.TryGetValue(key, out var handler))
            {
                handler(State);
            }
            else
            {
                Console.WriteLine("\n Could not find suitable mapping for " + key + "\n");
            }
        }

        #region Transition Handlers
        private void EnterAttack(CharacterState current)
        {
            if (current.IsGrounded)//on ground
            {
                //combo stuffs
                if (current.CurrentState == Sprint || current.CurrentState == Run) { current.CurrentState = AttackDash; }
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

        private void EnterSpecial(CharacterState current)
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
        private void EnterJump(CharacterState current)
        {
            //because the dictionary effectively makes a dictionary check we can assume at this point that jump is mandatory
            if (!(current.JumpsLeft <= 0))
            {
                current.CurrentState = Jump;
                current.DecrementJumps();

                //for now I will personally set isGrounded false this should likely be done by something else later
                current.IsGrounded = false;
            }
            else
            {
                Debug.WriteLine("you should not be doing such unregulated vaulting my good sir how darest you vault thyself when you are unable (you're out of jumps)");
            }
        }
        private void EnterLanding(CharacterState current)
        {
            if (current.CurrentState == FreeFall)
            {
                current.CurrentState = LayingDown;
            }
            else
            {
                current.CurrentState = Landing;
            }

            //the hitGround will reset our jumps
        }
        private void EnterKnockedBack(CharacterState current)
        {
            throw new Exception("\n Knockback not implemented");
        }
        private void EnterIdle(CharacterState current)
        {
            if (current.IsGrounded) { current.CurrentState = Idle; }
            else if (!current.IsGrounded) { current.CurrentState = AirIdle; }
            else { throw new Exception("WHAT HAVE YOU DONE"); }
        }
        private void EnterFalling(CharacterState current)
        {
            //the specificity of this transition means we should check it
            if (current.CurrentState == SpecialUp! && current.IsGrounded)
            {
                current.CurrentState = FreeFall;
            }
            else
            {
                //this can trigger in console state machine because during up special you could in theory hit the ground
                Debug.WriteLine("You should not be entering Freefall avoid hitting the ground during upSpecial");
            }
        }
        private void EnterRecover(CharacterState current)
        {
            current.CurrentState = Recover;
        }
        private void EnterMovement(CharacterState current)
        {
            //should only be called on basic
            if (!current.IsGrounded)
            {
                if (!(current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward))
                {
                    current.MovementDirection = current.DesiredMovementDirection;
                }
                current.CurrentState = AirMove;
            }
            else if (current.IsGrounded)
            {
                if (!(current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward))
                {
                    current.MovementDirection = current.DesiredMovementDirection;
                }
                current.CurrentState = Walk;
            }
            else
            {
                throw new Exception("Enter Movement Not Implemented");
            }
        }
        private void AlterMovement(CharacterState current)
        {
            //This is very complicated but it is simpler to understand when you realize that we only change our facing direction or speedSetting
            if (!current.IsGrounded)
            {
                if (!(current.ConvertToRelativeMovementDirection(current.DesiredMovementDirection) == Forward))
                {
                    current.MovementDirection = current.DesiredMovementDirection;
                    // in air we only change movement direction
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
                            current.CurrentState = Run;
                        }
                        else
                        {
                            current.CurrentState = Run;
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
                            current.MovementDirection = current.DesiredMovementDirection;
                            current.FacingDirection = current.DesiredMovementDirection;
                        }
                        break;
                }
            }

        }

        #endregion
    }
}