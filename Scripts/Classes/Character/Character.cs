using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.Interfaces;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Classes.Collision.CollisionHandlers;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.StateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp
{
    internal class Character : CollisionObject, ICharacter
    {
        private int _ID;

        private string _name;
        private ButtonDataManager _buttonDataManager;
        private ISpriteComplete _sprite;
        private static int xLocaleSpawn = 0;
        private static int yLocaleSpawn = 0;
        private CharacterMovement _movementManager;
        //locale spawn is not used later

        internal BodyCarrier _bodyCarrier;
        private AttackCarrier _attackCarrier;

        internal CharacterStateMachine StateMachine { get; set; }
        internal ActionList ActionList { get; set; }


        private bool hitboxDrawEnabled;

        public Character(string name, Texture2D spriteSheet, string spriteSheetName)
        {
            _ID = BattleScene.getNewID();
            _name = name;
            Velocity = Vector2.Zero;
            Position.X = xLocaleSpawn;
            //Magic numbers 50
            xLocaleSpawn += 50;
            Position.Y = yLocaleSpawn;
            yLocaleSpawn += 50;
            StateMachine = new CharacterStateMachine();
            _buttonDataManager = new ButtonDataManager();
            ActionList = new ActionList();
            _sprite = new AllPurposeSprite(spriteSheet);


            provideCharacterCarriers();
            AssignCollisionData();


            hitboxDrawEnabled = true;
        }
        public Character(string name, Texture2D spriteSheet, string spriteSheetName, Vector2 spawnLocation)
        {
            _ID = BattleScene.getNewID();
            _name = name;
            Velocity = Vector2.Zero;
            Position = spawnLocation;
            StateMachine = new CharacterStateMachine();
            _buttonDataManager = new ButtonDataManager();
            ActionList = new ActionList();
            _sprite = new AllPurposeSprite(spriteSheet);

            provideCharacterCarriers();
            AssignCollisionData();

            hitboxDrawEnabled = true;
        }


        public Rectangle GetPosition()
        {
            return _bodyCarrier.HitboxManager.GetApproximation();
        }

        private void provideCharacterCarriers()
        {
            _bodyCarrier = new BodyCarrier() { Parent = this };
           _attackCarrier = new AttackCarrier() { Parent = this };
            Carriers.Add( _bodyCarrier );
            Carriers.Add(_attackCarrier );
        }
        private void AssignCollisionData()
        {
            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => CharacterCollisionHandlers.HandlePlatformCollision(this, ctx));

        }

        public ButtonDataManager GetButtonDataManager
        {
            get => _buttonDataManager;
        }
        public void DoBehavior()
        {
            // StateMachine.PerformBehavior();
            if (StateMachine.State.getElapsedTime() >= 1000)
            {
                ActionList.AddAction(GameButtons.End);
            }
        }

		public void Animate(GameTime gameTime)
		{
			var animationData = AnimationRepository.GetAnimationData(_name, StateMachine.State.CurrentState);

			if (animationData == null || animationData.Frames.Count == 0)
				throw new Exception("major error in frame grabbing");

			// Calculate time per frame based on animation duration
			float frameDuration = animationData.Duration / animationData.Frames.Count;

			// Accumulate elapsed time
			StateMachine.State.addToElapsedTime((float)gameTime.ElapsedGameTime.TotalSeconds);

			// Advance frames as needed
			if (StateMachine.State.getElapsedTime() >= frameDuration)
			{
				StateMachine.State.IncrementFrameIndex();
				StateMachine.State.resetElapsedTime();
				// Handle frame overflow
				if (StateMachine.State.GetFrameIndex() >= animationData.Frames.Count)
				{
					if (animationData.Loop)
					{
						StateMachine.State.ResetFrameIndex();
					}
					else
					{
						ActionList.AddAction((GameButtons)GameButtons.End);
					}
				}
			}
		}

		public void DebugState()
		{
			Debug.WriteLine("State: " + StateMachine.State.CurrentState + " Frame: " + StateMachine.State.GetFrameIndex()+ "\nFacing: "+  StateMachine.State.FacingDirection + " Moving: "+ StateMachine.State.MovementDirection);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, Position, StateMachine.State.FacingDirection, StateMachine.State.CurrentState, StateMachine.State.GetFrameIndex(), _name);
        }
        //just pass current facing direction current state enum and current frame
        //StateMachine.State.CurrentState();

        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            _bodyCarrier.HitboxManager.Draw(spriteBatch);
        }

        public void UpdateState()
        {
            HandleStates();
            ActionList.ResetList();
            _bodyCarrier.HitboxManager.UpdateHitboxList(Position, StateMachine.State.FacingDirection, _name, StateMachine.State.CurrentState, StateMachine.State.GetFrameIndex());
        }
        internal void HandleStates()
        {
            foreach (GameButtons input in ActionList.actions.Where(i => IsDirection(i)))
            {
                switch (input)
                {
                    case GameButtons.Left:
                        StateMachine.State.DesiredMovementDirection = DirectionEnum.Left;
                        StateMachine.State.DesiredAttackDirection = DirectionEnum.Left;
                        StateMachine.HandleEvent(EventType.TryMove);
                        //StateMachine.State.FacingDirection = DirectionEnum.Left;
                        break;
                    case GameButtons.Right:
                        StateMachine.State.DesiredMovementDirection = DirectionEnum.Right;
                        StateMachine.State.DesiredAttackDirection = DirectionEnum.Right;
                        StateMachine.HandleEvent(EventType.TryMove);
                        //StateMachine.State.FacingDirection = DirectionEnum.Right;
                        break;
                    case GameButtons.Up:
                        StateMachine.State.DesiredAttackDirection = DirectionEnum.Up;
                        break;
                    case GameButtons.Down:
                        StateMachine.State.DesiredAttackDirection = DirectionEnum.Down;
                        break;
                }
            }

			// Then handle actions
			foreach (var input in ActionList.actions.Where(i => !IsDirection(i)))
			{
				switch (input)
				{
					case GameButtons.Attack:
						StateMachine.HandleEvent(EventType.TryAttack);
						StateMachine.State.DesiredAttackDirection = DirectionEnum.None; // Clear after use
						break;

					case GameButtons.Special:
						StateMachine.HandleEvent(EventType.TrySpecial);
						StateMachine.State.DesiredAttackDirection = DirectionEnum.None;
						break;

					case GameButtons.Jump:
						StateMachine.HandleEvent(EventType.TryJump);
						break;

					case GameButtons.GotHit:
						StateMachine.HandleEvent(EventType.GotHit);
						break;

					case GameButtons.End:
						StateMachine.HandleEvent(EventType.EndOfState);
						break;

					case GameButtons.HitGround:
						StateMachine.HandleEvent(EventType.HitGround);
						break;

						/*
						case GameInput.move:
							StateMachine.HandleEvent(EventType.TryMove);
							StateMachine.State.DesiredMovementDirection = DirectionEnum.None;
							break;
						*/
				}
			}
		}

		static bool IsDirection(GameButtons input)
		{
			return input == GameButtons.Left || input == GameButtons.Right
				|| input == GameButtons.Up || input == GameButtons.Down;
		}

		public  void ProcessButtons()
		{
			foreach (var button in _buttonDataManager.ButtonDataSheet.Keys)
			{
				ActionList.ProcessButton(_buttonDataManager.ButtonDataSheet[button],button);
			}

		}

		public void ApplyMovementBehavior()
		{
			switch (StateMachine.State.CurrentState)
			{
				case (StateEnum.AirMove):
					Velocity.X = 300;
					if (StateMachine.State.MovementDirection == DirectionEnum.Left) { Velocity.X *= -1; }
					break;
				case (StateEnum.Walk):
					Velocity.X = 40;
					if (StateMachine.State.MovementDirection == DirectionEnum.Left) { Velocity.X *= -1; }
					break;
				case (StateEnum.Run):
					Velocity.X = 80;
					if (StateMachine.State.MovementDirection == DirectionEnum.Left) { Velocity.X *= -1; }
					break;
				case (StateEnum.Sprint):
					Velocity.X = 300;
					if (StateMachine.State.MovementDirection == DirectionEnum.Left) { Velocity.X *= -1; }
					break;
				case (StateEnum.SlideTurn):
					Velocity.X = 10;
					if (StateMachine.State.FacingDirection == DirectionEnum.Left) { Velocity.X *= -1; }
					break;
				case (StateEnum.Idle):
					Velocity.X = 0;
					if (StateMachine.State.MovementDirection == DirectionEnum.Left) { Velocity.X *= 0; }
					break;
				case (StateEnum.Landing):
					Velocity.X = 0;
					if (StateMachine.State.MovementDirection == DirectionEnum.Left) { Velocity.X *= 0; }
					break;
				case (StateEnum.SpecialUp):
					Velocity.Y = -150;
					break;
				case (StateEnum.Jump):
					if (StateMachine.State.GetFrameIndex() == 0)
					{
						Velocity.Y = -550;
						break;
					}
					break;

			}

		}

		public void MoveCharacter(GameTime gameTime) //this may be permanent but should in the future maybe include acceleration
		{
			Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

        /*
        public void CheckGroundCollision() //temporary
        {
            if(hitboxDrawEnabled && Position.Y < 300)
            {
                Rectangle platform = new Rectangle(200, 200, 50, 20);
                Rectangle approx = _hitboxManager.GetApproximation();
                if (!approx.IsEmpty)
                {
                    Rectangle inter = Rectangle.Intersect(platform, approx);
                    if (!inter.IsEmpty) {

                        Position.Y -= (int)inter.Height-1;
                        Position.Y = (int)Position.Y;
                        Debug.WriteLine(Position.Y);

                        ActionList.AddAction(GameButtons.HitGround);
                        Velocity.Y = 0;
                        StateMachine.State.IsGrounded = true;
                        StateMachine.State.ResetJumps();
                    }
                                    else
                {
                    StateMachine.State.IsGrounded = false;
                }
                }
            }
            if (StateMachine.State.CurrentState != StateEnum.Jump)
            {
                if (Position.Y >= 400)
                {
                    ActionList.AddAction(GameButtons.HitGround);
                    Velocity.Y = 0;
                    //Velocity.X = 0;
                    Position.Y = 400;
                    StateMachine.State.IsGrounded = true;
                    StateMachine.State.ResetJumps();
                    // Console.WriteLine("EventHitGround: This may not necessarily result in a new Enum State");
                }
                else
                {
                    StateMachine.State.IsGrounded = false;
                }
                if (StateMachine.State.CurrentState == StateEnum.SpecialBack
                    || StateMachine.State.CurrentState == StateEnum.SpecialDown
                    || StateMachine.State.CurrentState == StateEnum.SpecialForward
                    || StateMachine.State.CurrentState == StateEnum.SpecialNeutral
                    || StateMachine.State.CurrentState == StateEnum.SpecialUp)
                {
                    Velocity.X = 0;
                }
            }
        }
        */

        public void Gravity(GameTime gameTime) //this is temporary
        {
            if (StateMachine.State.IsGrounded != true)
            {
                {
                    if(StateMachine.State.CurrentState==StateEnum.Sprint||
                        StateMachine.State.CurrentState == StateEnum.Run||
                        StateMachine.State.CurrentState == StateEnum.Walk)
                    {
                        StateMachine.State.CurrentState = StateEnum.AirMove;
                    }
                    Velocity.Y += 1300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                //Velocity = Velocity * .8f;
            }
            StateMachine.State.IsGrounded = false;
        }
    }

}