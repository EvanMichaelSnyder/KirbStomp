using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.Interfaces;
using KirbStomp.StateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp
{
    internal class Character : ICharacter
    {
        private int _ID;

        private string _name;
        private HitboxManager _hitboxManager;
        private CharacterStateMachine _stateMachine;
        private ButtonDataManager _buttonDataManager;
        private ActionList _actionList;
        private Vector2 _position, _velocity;
        private ISpriteComplete _sprite;
        private static int xLocaleSpawn = 0;
        private static int yLocaleSpawn = 0;
        //locale spawn is not used later

        private bool hitboxDrawEnabled;

        public Character(string name, Texture2D spriteSheet, string spriteSheetName)
        {
            _ID = BattleScene.getNewID();
            _name = name;
            _velocity = Vector2.Zero;
            _position.X = xLocaleSpawn;
            //Magic numbers 50
            xLocaleSpawn += 50;
            _position.Y = yLocaleSpawn;
            yLocaleSpawn += 50;
            _stateMachine = new CharacterStateMachine();
            _buttonDataManager = new ButtonDataManager();
            _actionList = new ActionList();
            _sprite = new AllPurposeSprite(spriteSheet);


            _hitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Character);
            hitboxDrawEnabled = true;
        }

        public ButtonDataManager GetButtonDataManager
        {
            get => _buttonDataManager;
        }
        public void DoBehavior()
        {
            // _stateMachine.PerformBehavior();
            if (_stateMachine.State.getElapsedTime() >= 1000)
            {
                _actionList.AddAction(GameButtons.End);
            }
        }

		public void Animate(GameTime gameTime)
		{
			var animationData = AnimationRepository.GetAnimationData(_name, _stateMachine.State.CurrentState);

			if (animationData == null || animationData.Frames.Count == 0)
				throw new Exception("major error in frame grabbing");

			// Calculate time per frame based on animation duration
			float frameDuration = animationData.Duration / animationData.Frames.Count;

			// Accumulate elapsed time
			_stateMachine.State.addToElapsedTime((float)gameTime.ElapsedGameTime.TotalSeconds);

			// Advance frames as needed
			if (_stateMachine.State.getElapsedTime() >= frameDuration)
			{
				_stateMachine.State.IncrementFrameIndex();
				_stateMachine.State.resetElapsedTime();
				// Handle frame overflow
				if (_stateMachine.State.GetFrameIndex() >= animationData.Frames.Count)
				{
					if (animationData.Loop)
					{
						_stateMachine.State.ResetFrameIndex();
					}
					else
					{
						_actionList.AddAction((GameButtons)GameButtons.End);
					}
				}
			}
		}

		public void DebugState()
		{
			Debug.WriteLine("State: " + _stateMachine.State.CurrentState + " Frame: " + _stateMachine.State.GetFrameIndex()+ "\nFacing: "+  _stateMachine.State.FacingDirection + " Moving: "+ _stateMachine.State.MovementDirection);
		}

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, _position, _stateMachine.State.FacingDirection, _stateMachine.State.CurrentState, _stateMachine.State.GetFrameIndex(), _name);
        }
        //just pass current facing direction current state enum and current frame
        //_stateMachine.State.CurrentState();

        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            if (hitboxDrawEnabled)
            {
                if (_name == "Mario")
                {
                    _hitboxManager.DrawExtended(spriteBatch);
                }
            }
        }

        public void UpdateState()
        {
            HandleStates();
            _actionList.ResetList();
            if (_name == "Mario")
            {
                // _hitboxManager.UpdateHitboxList(_position, _stateMachine.State.FacingDirection, _name, _stateMachine.State.CurrentState, _stateMachine.State.GetFrameIndex());
            }
        }
        internal void HandleStates()
        {
            foreach (GameButtons input in _actionList.actions.Where(i => IsDirection(i)))
            {
                switch (input)
                {
                    case GameButtons.Left:
                        _stateMachine.State.DesiredMovementDirection = DirectionEnum.Left;
                        _stateMachine.State.DesiredAttackDirection = DirectionEnum.Left;
                        _stateMachine.HandleEvent(EventType.TryMove);
                        //_stateMachine.State.FacingDirection = DirectionEnum.Left;
                        break;
                    case GameButtons.Right:
                        _stateMachine.State.DesiredMovementDirection = DirectionEnum.Right;
                        _stateMachine.State.DesiredAttackDirection = DirectionEnum.Right;
                        _stateMachine.HandleEvent(EventType.TryMove);
                        //_stateMachine.State.FacingDirection = DirectionEnum.Right;
                        break;
                    case GameButtons.Up:
                        _stateMachine.State.DesiredAttackDirection = DirectionEnum.Up;
                        break;
                    case GameButtons.Down:
                        _stateMachine.State.DesiredAttackDirection = DirectionEnum.Down;
                        break;
                }
            }

			// Then handle actions
			foreach (var input in _actionList.actions.Where(i => !IsDirection(i)))
			{
				switch (input)
				{
					case GameButtons.Attack:
						_stateMachine.HandleEvent(EventType.TryAttack);
						_stateMachine.State.DesiredAttackDirection = DirectionEnum.None; // Clear after use
						break;

					case GameButtons.Special:
						_stateMachine.HandleEvent(EventType.TrySpecial);
						_stateMachine.State.DesiredAttackDirection = DirectionEnum.None;
						break;

					case GameButtons.Jump:
						_stateMachine.HandleEvent(EventType.TryJump);
						break;

					case GameButtons.GotHit:
						_stateMachine.HandleEvent(EventType.GotHit);
						break;

					case GameButtons.End:
						_stateMachine.HandleEvent(EventType.EndOfState);
						break;

					case GameButtons.HitGround:
						_stateMachine.HandleEvent(EventType.HitGround);
						break;

						/*
						case GameInput.move:
							_stateMachine.HandleEvent(EventType.TryMove);
							_stateMachine.State.DesiredMovementDirection = DirectionEnum.None;
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
				_actionList.ProcessButton(_buttonDataManager.ButtonDataSheet[button],button);
			}

		}

		public void ApplyMovementBehavior()
		{
			switch (_stateMachine.State.CurrentState)
			{
				case (StateEnum.AirMove):
					_velocity.X = 300;
					if (_stateMachine.State.MovementDirection == DirectionEnum.Left) { _velocity.X *= -1; }
					break;
				case (StateEnum.Walk):
					_velocity.X = 40;
					if (_stateMachine.State.MovementDirection == DirectionEnum.Left) { _velocity.X *= -1; }
					break;
				case (StateEnum.Run):
					_velocity.X = 80;
					if (_stateMachine.State.MovementDirection == DirectionEnum.Left) { _velocity.X *= -1; }
					break;
				case (StateEnum.Sprint):
					_velocity.X = 300;
					if (_stateMachine.State.MovementDirection == DirectionEnum.Left) { _velocity.X *= -1; }
					break;
				case (StateEnum.SlideTurn):
					_velocity.X = 10;
					if (_stateMachine.State.FacingDirection == DirectionEnum.Left) { _velocity.X *= -1; }
					break;
				case (StateEnum.Idle):
					_velocity.X = 0;
					if (_stateMachine.State.MovementDirection == DirectionEnum.Left) { _velocity.X *= 0; }
					break;
				case (StateEnum.Landing):
					_velocity.X = 0;
					if (_stateMachine.State.MovementDirection == DirectionEnum.Left) { _velocity.X *= 0; }
					break;
				case (StateEnum.SpecialUp):
					_velocity.Y = -150;
					break;
				case (StateEnum.Jump):
					if (_stateMachine.State.GetFrameIndex() == 0)
					{
						_velocity.Y = -550;
						break;
					}
					break;

			}

		}

		public void MoveCharacter(GameTime gameTime) //this may be permanent but should in the future maybe include acceleration
		{
			_position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

        public void CheckGroundCollision() //temporary
        {
            if(hitboxDrawEnabled && _position.Y < 300)
            {
                Rectangle platform = new Rectangle(200, 200, 50, 20);
                Rectangle approx = _hitboxManager.GetApproximation();
                if (!approx.IsEmpty)
                {
                    Rectangle inter = Rectangle.Intersect(platform, approx);
                    if (!inter.IsEmpty) {

                        _position.Y -= (int)inter.Height-1;
                        _position.Y = (int)_position.Y;
                        Debug.WriteLine(_position.Y);

                        _actionList.AddAction(GameButtons.HitGround);
                        _velocity.Y = 0;
                        _stateMachine.State.IsGrounded = true;
                        _stateMachine.State.ResetJumps();
                    }
                                    else
                {
                    _stateMachine.State.IsGrounded = false;
                }
                }
            }
            if (_stateMachine.State.CurrentState != StateEnum.Jump)
            {
                if (_position.Y >= 400)
                {
                    _actionList.AddAction(GameButtons.HitGround);
                    _velocity.Y = 0;
                    //_velocity.X = 0;
                    _position.Y = 400;
                    _stateMachine.State.IsGrounded = true;
                    _stateMachine.State.ResetJumps();
                    // Console.WriteLine("EventHitGround: This may not necessarily result in a new Enum State");
                }
                else
                {
                    _stateMachine.State.IsGrounded = false;
                }
                if (_stateMachine.State.CurrentState == StateEnum.SpecialBack
                    || _stateMachine.State.CurrentState == StateEnum.SpecialDown
                    || _stateMachine.State.CurrentState == StateEnum.SpecialForward
                    || _stateMachine.State.CurrentState == StateEnum.SpecialNeutral
                    || _stateMachine.State.CurrentState == StateEnum.SpecialUp)
                {
                    _velocity.X = 0;
                }
            }
        }

        public void Gravity(GameTime gameTime) //this is temporary
        {
            if (!_stateMachine.State.IsGrounded)
            {
                _velocity.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            //_velocity = _velocity * .8f;
        }
    }

}