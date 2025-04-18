using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.Data.AttackData;
using KirbStomp.Interfaces;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Classes.Collision.CollisionHandlers;
using KirbStomp.Scripts.Classes.GameObjects.ItemAbillity;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Classes.Sound;
using KirbStomp.Scripts.Projectiles;
using KirbStomp.StateMachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp
{
    public class Character : CollisionObject, ICharacter
    {
        private int _ID;

        private string _name;
        private ButtonDataManager _buttonDataManager;
        private ISpriteComplete _sprite;
        private static int xLocaleSpawn = 0;
        private static int yLocaleSpawn = 0;
        //private CharacterMovement _movementManager;
        //locale spawn is not used later

        internal BodyCarrier _bodyCarrier;
        internal AttackCarrier _attackCarrier;

        internal CharacterStateMachine StateMachine { get; set; }
        internal ActionList ActionList { get; set; }

        private AItemAbillity _itemAbillity;

        internal float _health = 0;

        private int _lives = 3;
        private float minVelocity;
        private float maxVelocity;
        private const float capVelocity = 450;
        private Vector2 acceleration = Vector2.Zero;
        internal bool snapVelocity = false;
        private const float jumpVelocity = -675;
        private const float specialUpVelocity = -400;
        bool projSpawnedOnThisFrame = false;

        private bool hitboxDrawEnabled;
        private CharacterUIData _characterUIData;

        public event EventHandler<OnHealthChangeEventArgs> OnHealthChange;
        public class OnHealthChangeEventArgs : EventArgs {
            public float health;
        }
        public event EventHandler<OnLivesChangeEventArgs> OnLivesChange;
        public class OnLivesChangeEventArgs : EventArgs
        {
            public int lives;
        }
        public event EventHandler OnDeath;

        private SoundManager _soundManager;
        public void Respawn()
        {
            if (_lives==1)
            {
                // _health = 100;
                ResetHealth();
                DecreaseLives();

                ActionList.ResetList();
                StateMachine.State.CurrentState = StateEnum.Idle;
                Velocity = Vector2.Zero;
                acceleration = Vector2.Zero;
                Vector2 respawnLocation = new Vector2(-100000, -100000);
                Position = respawnLocation;

                OnDeath?.Invoke(this, EventArgs.Empty);
                // SceneManager.Get().SwitchScene("EndScreen");
            }
            else
            {
                // _health = 100;
                ResetHealth();
                DecreaseLives();

                ActionList.ResetList();
                StateMachine.State.CurrentState = StateEnum.Idle;
                Velocity = Vector2.Zero;
                acceleration = Vector2.Zero;
                Vector2 respawnLocation = new Vector2(380, 20);
                Position = respawnLocation;
            }
        }
        public Character(string name, Texture2D spriteSheet, Vector2 spawnLocation)
        {
            _ID = BattleScene.getNewID();
            _name = name;
            Velocity = Vector2.Zero;
            Position = spawnLocation;
            StateMachine = new CharacterStateMachine();
            _buttonDataManager = new ButtonDataManager();
            ActionList = new ActionList();
            _sprite = new AllPurposeSprite(spriteSheet);
            _soundManager = SoundManager.Get();
            _soundManager.LoadContent();

            provideCharacterCarriers();
            AssignCollisionData();

            hitboxDrawEnabled = true;
            OnLivesChange?.Invoke(this, new OnLivesChangeEventArgs { lives = _lives });
        }
        public Character(string name, Texture2D spriteSheet, Vector2 spawnLocation, CharacterUIData characterUIData)
        {
            _ID = BattleScene.getNewID();
            _name = name;
            Velocity = Vector2.Zero;
            Position = spawnLocation;
            StateMachine = new CharacterStateMachine();
            _buttonDataManager = new ButtonDataManager();
            ActionList = new ActionList();
            _sprite = new AllPurposeSprite(spriteSheet);
            _characterUIData = characterUIData;
            _soundManager = SoundManager.Get();
            _soundManager.LoadContent();

            if (name == "Link")
            {
                this.StateMachine.State.FacingDirection = DirectionEnum.Left;
            }

            provideCharacterCarriers();
            AssignCollisionData();

            hitboxDrawEnabled = true;
            OnLivesChange?.Invoke(this, new OnLivesChangeEventArgs { lives = _lives });
        }

        public Rectangle GetPosition()
        {
            return _bodyCarrier.HitboxManager.GetApproximation();
        }
        public CharacterUIData GetCharacterUIData()
        {
            return _characterUIData;
        }

        private void provideCharacterCarriers()
        {
            _bodyCarrier = new BodyCarrier() { Parent = this };
            _attackCarrier = new AttackCarrier() { Parent = this };
            Carriers.Add(_bodyCarrier);
            Carriers.Add(_attackCarrier);
        }
        private void AssignCollisionData()
        {
            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => CharacterCollisionHandlers.HandlePlatformCollision(this, ctx));
            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Attack,
                                    (obj, ctx) => CharacterCollisionHandlers.HandleAttackCollision(this, ctx));
            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Boundary,
                                    (obj, ctx) => CharacterCollisionHandlers.HandleBoundaryCollision(this, ctx));

        }

        public ButtonDataManager GetButtonDataManager
        {
            get => _buttonDataManager;
        }
        public void DoBehavior()
        {
            if (projSpawnedOnThisFrame == false)
            {
                //StateMachine.PerformBehavior();
                var data = ProjectileFrameSpawn.GetProjectileInfo(this._name, this.StateMachine.State.CurrentState, this.StateMachine.State.GetFrameIndex());
                if (data.projectileName != "No Projectile")
                {
                    SceneManager sceneManage = SceneManager.Get();
                    IScene currentScene = sceneManage.GetCurrentScene();
                    ProjectileManager projManager = currentScene.GetProjectileManager();
                    Vector2 positionProj = new Vector2();
                    positionProj.X = this.GetPosition().X;
                    positionProj.Y = this.GetPosition().Y;
                    if (this.StateMachine.State.FacingDirection == DirectionEnum.Right)
                    {
                        positionProj.Y += data.offsets.Y;
                        positionProj.X += data.offsets.X;
                        projManager.SpawnProjectile(data.projectileName, positionProj, true, this);
                    }
                    else
                    {
                        positionProj.X += this.GetPosition().Width;
                        positionProj.Y += data.offsets.Y;
                        positionProj.X -= data.offsets.X;
                        projManager.SpawnProjectile(data.projectileName, positionProj, false, this);
                    }
                    projSpawnedOnThisFrame = true;
                }
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
                projSpawnedOnThisFrame = false;
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
            Debug.WriteLine("State: " + StateMachine.State.CurrentState + " Frame: " + StateMachine.State.GetFrameIndex() + "\nFacing: " + StateMachine.State.FacingDirection + " Moving: " + StateMachine.State.MovementDirection);
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
            _attackCarrier.HitboxManager.Draw(spriteBatch);
        }

        public void UpdateState()
        {
            HandleStates();
            ActionList.ResetList();
            _bodyCarrier.HitboxManager.UpdateHitboxList(Position, StateMachine.State.FacingDirection, _name, StateMachine.State.CurrentState, StateMachine.State.GetFrameIndex());
            _attackCarrier.HitboxManager.UpdateAttackHitboxList(Position, StateMachine.State.FacingDirection, _name, StateMachine.State.CurrentState, StateMachine.State.GetFrameIndex());
            _attackCarrier.assignAttackDataFromXML(_name, this.StateMachine.State.CurrentState);
        }
        internal void HandleStates()
        {
            foreach (GameButtons input in ActionList.actions.Where(i => IsDirection(i)))
            {
                switch (input)
                {
                    case GameButtons.Left:
                        if (_buttonDataManager.ButtonDataSheet[GameButtons.Left].GetButtonState() == ButtonState.Released)
                        {
                            StateMachine.State.continueMoving = false;
                        }
                        else
                        {
                            StateMachine.State.continueMoving = true;
                            StateMachine.State.DesiredMovementDirection = DirectionEnum.Left;
                            StateMachine.State.DesiredAttackDirection = DirectionEnum.Left;
                            StateMachine.HandleEvent(EventType.TryMove);
                            //StateMachine.State.FacingDirection = DirectionEnum.Left;
                        }
                        break;
                    case GameButtons.Right:
                        if (_buttonDataManager.ButtonDataSheet[GameButtons.Right].GetButtonState() == ButtonState.Released)
                        {
                            StateMachine.State.continueMoving = false;
                        }
                        else
                        {
                            StateMachine.State.continueMoving = true;
                            StateMachine.State.DesiredMovementDirection = DirectionEnum.Right;
                            StateMachine.State.DesiredAttackDirection = DirectionEnum.Right;
                            StateMachine.HandleEvent(EventType.TryMove);
                        }
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
                        _soundManager.PlaySound("Attack");
                        StateMachine.State.DesiredAttackDirection = DirectionEnum.None; // Clear after use
                        break;

                    case GameButtons.Special:
                        /*
                        Velocity.X = 1;
                        acceleration.X = 0;
                        */
                        StateMachine.HandleEvent(EventType.TrySpecial);
                        _soundManager.PlaySound("Special");
                        StateMachine.State.DesiredAttackDirection = DirectionEnum.None;
                        break;

                    case GameButtons.Jump:
                        StateMachine.HandleEvent(EventType.TryJump);
                        _soundManager.PlaySound("Jump");
                        break;

                    case GameButtons.GotHit:
                        StateMachine.HandleEvent(EventType.GotHit);
                        _soundManager.PlaySound("Hit");
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

        public void ProcessButtons()
        {
            foreach (var button in _buttonDataManager.ButtonDataSheet.Keys)
            {
                ActionList.ProcessButton(_buttonDataManager.ButtonDataSheet[button], button);
            }

        }

        public void ApplyMovementBehavior()
        {
            Dictionary<StateEnum, Action> _movementBehaviors = new Dictionary<StateEnum, Action>
            {
                { StateEnum.AirMove, AirMove },
                { StateEnum.Walk, Walk },
                { StateEnum.Run, Run },
                { StateEnum.Sprint, Sprint },
                { StateEnum.Idle, Idle },
                { StateEnum.Landing, Landing },
                { StateEnum.SpecialUp, SpecialUp },
                { StateEnum.Jump, Jump },
                { StateEnum.LayingDown, LayingDown},
                { StateEnum.SpecialForward, Special },
                { StateEnum.AttackDash, Slide }
            };
            if (_movementBehaviors.TryGetValue(StateMachine.State.CurrentState, out Action behavior))
            {
                behavior.Invoke();
            }
        }
        private void AirMove()
        {
            maxVelocity = 300;
            acceleration.X = 10;
            snapVelocity = true;
        }
        private void Idle()
        {
            minVelocity = 0;
            maxVelocity = 1;
            acceleration.X = 0;
            snapVelocity = true;
        }
        private void Walk()
        {
            minVelocity = 1;
            maxVelocity = 100;
            acceleration.X = 500;
            snapVelocity = false;
        }
        private void Run()
        {
            minVelocity = 100;
            maxVelocity = 300;
            acceleration.X = 800;
            snapVelocity = false;
        }
        private void Sprint()
        {
            minVelocity = 300;
            maxVelocity = 500;
            acceleration.X = 600;
            snapVelocity = false;
        }
        private void Landing()
        {
            maxVelocity = 1;
            acceleration.X = 0;
            snapVelocity = true;
            _soundManager.PlaySound("Landing");
        }
        private void SpecialUp()
        {
            Velocity.Y = specialUpVelocity;
            maxVelocity = 100;
            acceleration.X = 0;
            snapVelocity = true;
            _soundManager.PlaySound("SpecialUp");
        }

        private void Special()
        {
            maxVelocity = 50;
            acceleration.X = 0;
            snapVelocity = true;
        }
        private void Slide()
        {
            maxVelocity = 200;
            acceleration.X = 0;
            snapVelocity = true;
        }

        private void LayingDown()
        {
            maxVelocity = 1;
            acceleration.X = 0;
        }
        private void Jump()
        {
            if (StateMachine.State.GetFrameIndex() == 0) 
            { 
                Velocity.Y = jumpVelocity;
            }
        }
        private void SetDirection()
        {
            if (StateMachine.State.MovementDirection == DirectionEnum.Left)
            {
                Velocity.X = -Math.Abs(Velocity.X);
                StateMachine.State.FacingDirection = DirectionEnum.Left;
            }
            else if (StateMachine.State.MovementDirection == DirectionEnum.Right)
            {
                Velocity.X = Math.Abs(Velocity.X);
                StateMachine.State.FacingDirection = DirectionEnum.Right;
            }
        }

        private void SetAcceleration(GameTime gameTime)
        {
            // Checks whether the character is moving to see if acceleration is needed,
            // Otherwise it decelerates them.
            if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Right)
            {
                if (StateMachine.State.continueMoving)
                {
                    Velocity.X += acceleration.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    Velocity.X -= acceleration.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Left)
            {
                if (StateMachine.State.continueMoving)
                {
                    Velocity.X -= acceleration.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    Velocity.X += acceleration.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        private void CheckTransition()
        {
            // Checks the current state of the character to see if a transition
            // to another state is needed (Walk -> Run -> Sprint -> Capped Sprint velocity)
            //Debug.WriteLine("State: " + StateMachine.State.CurrentState + ", Velocity: " + Velocity.X);
            if (StateMachine.State.CurrentState == StateEnum.Idle)
            {
                IdleHandler();
            }
            else if (StateMachine.State.CurrentState == StateEnum.Walk)
            {
                WalkHandler();
                _soundManager.PlaySound("Walk");
            }
            else if (StateMachine.State.CurrentState == StateEnum.Run)
            {
                RunHandler();
                _soundManager.PlaySound("Walk");
            }
            else if (StateMachine.State.CurrentState == StateEnum.Sprint)
            {
                SprintHandler();
                _soundManager.PlaySound("Walk");
            }
        }

        private void IdleHandler()
        {
            if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Right)
            {
                if (Velocity.X > maxVelocity)
                {
                    Walk();
                    StateMachine.State.CurrentState = StateEnum.Walk;
                    Velocity.X = minVelocity;
                }
            }
            else if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Left)
            {
                if (Velocity.X < -maxVelocity)
                { 
                    Walk();
                    StateMachine.State.CurrentState = StateEnum.Walk;
                    Velocity.X = -minVelocity;
                }
            }
        }

        private void WalkHandler()
        {
            if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Right)
            {
                if (Velocity.X > maxVelocity)
                {
                    Run();
                    StateMachine.State.CurrentState = StateEnum.Run;
                    Velocity.X = minVelocity;
                }
                else if (Velocity.X < minVelocity)
                {
                    Idle();
                    StateMachine.State.CurrentState = StateEnum.Idle;
                }
            }
            else if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Left)
            {
                if (Velocity.X < -maxVelocity)
                {
                    Run();
                    StateMachine.State.CurrentState = StateEnum.Run;
                    Velocity.X = -minVelocity;
                }
                else if (Velocity.X > -minVelocity)
                {
                    Idle();
                    StateMachine.State.CurrentState = StateEnum.Idle;
                }
            }
        }

        private void RunHandler()
        {
            if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Right)
            {
                if (Velocity.X > maxVelocity)
                {
                    Sprint();
                    StateMachine.State.CurrentState = StateEnum.Sprint;
                    Velocity.X = minVelocity;
                }
                else if (Velocity.X < minVelocity)
                {
                    Walk();
                    StateMachine.State.CurrentState = StateEnum.Walk;
                    Velocity.X = maxVelocity;
                }
            }
            else if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Left)
            {
                if (Velocity.X < -maxVelocity)
                {
                    Sprint();
                    StateMachine.State.CurrentState = StateEnum.Sprint;
                    Velocity.X = -minVelocity;
                }
                else if (Velocity.X > -minVelocity)
                {
                    Walk();
                    StateMachine.State.CurrentState = StateEnum.Walk;
                    Velocity.X = -maxVelocity;
                }
            }
        }



        private void SprintHandler()
        {
            if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Right)
            {
                if (Velocity.X > capVelocity)
                {
                    Velocity.X = capVelocity;
                }
                else if (Velocity.X < minVelocity)
                {
                    Run();
                    StateMachine.State.CurrentState = StateEnum.Run;
                    Velocity.X = maxVelocity;
                }
            }
            else if (StateMachine.State.DesiredMovementDirection == DirectionEnum.Left)
            {
                if (Velocity.X < -capVelocity)
                {
                    Velocity.X = -capVelocity;
                }
                else if (Velocity.X > -minVelocity)
                {
                    Run();
                    StateMachine.State.CurrentState = StateEnum.Run;
                    Velocity.X = -maxVelocity;
                }
            }
        }

        public void MoveCharacter(GameTime gameTime)
        {
            // Some movement should snap to a specific velocity value
            // (Idle, Landing, etc.) thus Velocity should auto to that maxVelocity
            // value
            if (snapVelocity)
            {
                Velocity.X = maxVelocity;
            }
            CheckTransition();
            SetAcceleration(gameTime);
            if (StateMachine.State.CurrentState != StateEnum.KnockedBack
                && StateMachine.State.CurrentState != StateEnum.Ragdolled)
            {
                SetDirection();
            }
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
                    //Velocity.X = 
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
                    Velocity.X 
        ;
                }
            }
        }
        */

        public void Gravity(GameTime gameTime) //this is temporary
        {
            //weird place to put this but i dont care
            ClockTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (StateMachine.State.IsGrounded != true)
            {
                {
                    if (StateMachine.State.CurrentState == StateEnum.Sprint ||
                        StateMachine.State.CurrentState == StateEnum.Run ||
                        StateMachine.State.CurrentState == StateEnum.Walk)
                    {
                        StateMachine.State.CurrentState = StateEnum.AirIdle;
                        StateMachine.State.fancyPlatformCollisionFlag = true;
                    }
                    Velocity.Y += 1300 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                //Velocity.X += .9f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            StateMachine.State.IsGrounded = false;
        }

        public void RecieveItemAbillity(AItemAbillity abillity)
        {
            this._itemAbillity = abillity;
        }

        public void AddHealth(float amt)
        {
            this._health -= amt;
            if(this._health < 0)
            {
                this._health = 0;
            }
            OnHealthChange?.Invoke(this, new OnHealthChangeEventArgs { health = this._health });
        }
        public void TakeDamage(float amt)
        {
            this._health += amt;
            OnHealthChange?.Invoke(this, new OnHealthChangeEventArgs { health = this._health });
        }
        public void ResetHealth() {
            this._health = 0;
            OnHealthChange?.Invoke(this, new OnHealthChangeEventArgs { health = this._health });
        }
        public float GetHealth() {
            return this._health;
        }
        public void DecreaseLives() {
            this._lives--;
            OnLivesChange?.Invoke(this, new OnLivesChangeEventArgs { lives = this._lives });
        }
        public BodyCarrier GetBodyCarrier()
        {
            return this._bodyCarrier;   
        }
        public Point GetPointPosition()
        {
            return new((int)Position.X, (int)Position.Y);
        }
        public string GetName()
        {
            return this._name;
        }
    }

}