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
        private CharacterStateMachine stateMachine;
        private ButtonDataManager buttonDataManager;
        private ActionList actionList;
        private Vector2 position, velocity;
        private ISpriteComplete sprite;
        private string _spriteSheetName;
        private static int xLocaleSpawn = 0;
        //locale spawn is not used later
        
        public Character(Texture2D spriteSheet, string spriteSheetName)
        {

            velocity = Vector2.Zero;
            position.X = xLocaleSpawn;
            xLocaleSpawn += 50;
            position.Y = 0;
            stateMachine = new CharacterStateMachine();
            buttonDataManager = new ButtonDataManager();
            actionList = new ActionList();
            _spriteSheetName = spriteSheetName;
            sprite = new AllPurposeSprite(spriteSheet);
        }
        public ButtonDataManager GetButtonDataManager
        {
            get => buttonDataManager;
        }
        public void doBehavior()
        {
            stateMachine.performBehavior();
            if (stateMachine.State.getElapsedTime() >= 1000)
            {
                actionList.addAction(GameButtons.End);
            }
        }

        public void Animate(GameTime gameTime)
        {
            var animationData = AnimationSystem.GetAnimationData(_spriteSheetName, stateMachine.State.CurrentState);

            if (animationData == null || animationData.Frames.Count == 0)
                throw new Exception("major error in frame grabbing");

            // Calculate time per frame based on animation duration
            float frameDuration = animationData.Duration / animationData.Frames.Count;

            // Accumulate elapsed time
            stateMachine.State.addToElapsedTime((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Advance frames as needed
            if (stateMachine.State.getElapsedTime() >= frameDuration)
            {
                stateMachine.State.incrementFrameIndex();
                stateMachine.State.resetElapsedTime();
                // Handle frame overflow
                if (stateMachine.State.getFrameIndex() >= animationData.Frames.Count)
                {
                    if (animationData.Loop)
                    {
                        stateMachine.State.resetFrameIndex();
                    }
                    else
                    {
                        actionList.addAction((GameButtons)GameButtons.End);
                    }
                }
            }
        }
        public void debugState()
        {
            Debug.WriteLine("State: " + stateMachine.State.CurrentState + " Frame: " + stateMachine.State.getFrameIndex()+ "\nFacing: "+  stateMachine.State.FacingDirection + " Moving: "+ stateMachine.State.MovementDirection);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, stateMachine.State.FacingDirection, stateMachine.State.CurrentState, stateMachine.State.getFrameIndex(), _spriteSheetName);
        }
        //just pass current facing direction current state enum and current frame
        //stateMachine.State.CurrentState();

        public void UpdateState()
        {
            HandleStates();
            actionList.resetList();
        }
        internal void HandleStates()
        {
            foreach (GameButtons input in actionList.actions.Where(i => IsDirection(i)))
            {
                switch (input)
                {
                    case GameButtons.Left:
                        stateMachine.State.DesiredMovementDirection = DirectionEnum.Left;
                        stateMachine.State.DesiredAttackDirection = DirectionEnum.Left;
                        stateMachine.HandleEvent(EventType.TryMove);
                        //stateMachine.State.FacingDirection = DirectionEnum.Left;
                        break;
                    case GameButtons.Right:
                        stateMachine.State.DesiredMovementDirection = DirectionEnum.Right;
                        stateMachine.State.DesiredAttackDirection = DirectionEnum.Right;
                        stateMachine.HandleEvent(EventType.TryMove);
                        //stateMachine.State.FacingDirection = DirectionEnum.Right;
                        break;
                    case GameButtons.Up:
                        stateMachine.State.DesiredAttackDirection = DirectionEnum.Up;
                        break;
                    case GameButtons.Down:
                        stateMachine.State.DesiredAttackDirection = DirectionEnum.Down;
                        break;
                }
            }

            // Then handle actions
            foreach (var input in actionList.actions.Where(i => !IsDirection(i)))
            {
                switch (input)
                {
                    case GameButtons.Attack:
                        stateMachine.HandleEvent(EventType.TryAttack);
                        stateMachine.State.DesiredAttackDirection = DirectionEnum.None; // Clear after use
                        break;

                    case GameButtons.Special:
                        stateMachine.HandleEvent(EventType.TrySpecial);
                        stateMachine.State.DesiredAttackDirection = DirectionEnum.None;
                        break;

                    case GameButtons.Jump:
                        stateMachine.HandleEvent(EventType.TryJump);
                        break;

                    case GameButtons.GotHit:
                        stateMachine.HandleEvent(EventType.GotHit);
                        break;

                    case GameButtons.End:
                        stateMachine.HandleEvent(EventType.EndOfState);
                        break;

                    case GameButtons.HitGround:
                        stateMachine.HandleEvent(EventType.HitGround);
                        break;

                        /*
                        case GameInput.move:
                            stateMachine.HandleEvent(EventType.TryMove);
                            stateMachine.State.DesiredMovementDirection = DirectionEnum.None;
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
            foreach (var button in buttonDataManager.buttonDataSheet.Keys)
            {
                actionList.processButton(buttonDataManager.buttonDataSheet[button],button);
            }

        }

        public void ApplyMovementBehavior()
        {
            switch (stateMachine.State.CurrentState)
            {
                case (StateEnum.AirMove):
                    velocity.X = 300;
                    if (stateMachine.State.MovementDirection == DirectionEnum.Left) { velocity.X *= -1; }
                    break;
                case (StateEnum.Walk):
                    velocity.X = 40;
                    if (stateMachine.State.MovementDirection == DirectionEnum.Left) { velocity.X *= -1; }
                    break;
                case (StateEnum.Run):
                    velocity.X = 80;
                    if (stateMachine.State.MovementDirection == DirectionEnum.Left) { velocity.X *= -1; }
                    break;
                case (StateEnum.Sprint):
                    velocity.X = 300;
                    if (stateMachine.State.MovementDirection == DirectionEnum.Left) { velocity.X *= -1; }
                    break;
                case (StateEnum.SlideTurn):
                    velocity.X = 10;
                    if (stateMachine.State.FacingDirection == DirectionEnum.Left) { velocity.X *= -1; }
                    break;
                case (StateEnum.Idle):
                    velocity.X = 0;
                    if (stateMachine.State.MovementDirection == DirectionEnum.Left) { velocity.X *= 0; }
                    break;
                case (StateEnum.Landing):
                    velocity.X = 0;
                    if (stateMachine.State.MovementDirection == DirectionEnum.Left) { velocity.X *= 0; }
                    break;
                case (StateEnum.SpecialUp):
                    velocity.Y = -150;
                    break;
                case (StateEnum.Jump):
                    if (stateMachine.State.getFrameIndex() == 0)
                    {
                        velocity.Y = -550;
                        break;
                    }
                    break;

            }

        }

        public void MoveCharacter(GameTime gameTime) //this may be permanent but should in the future maybe include acceleration
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void checkGroundCollision() //temporary
        {
            if (stateMachine.State.CurrentState != StateEnum.Jump)
            {
                if (position.Y >= 400)
                {
                    actionList.addAction(GameButtons.HitGround);
                    velocity.Y = 0;
                    //velocity.X = 0;
                    position.Y = 400;
                    stateMachine.State.IsGrounded = true;
                    stateMachine.State.ResetJumps();
                    Console.WriteLine("EventHitGround: This may not necessarily result in a new Enum State");
                }
                if (stateMachine.State.CurrentState == StateEnum.SpecialBack
                    || stateMachine.State.CurrentState == StateEnum.SpecialDown
                    || stateMachine.State.CurrentState == StateEnum.SpecialForward
                    || stateMachine.State.CurrentState == StateEnum.SpecialNeutral
                    || stateMachine.State.CurrentState == StateEnum.SpecialUp)
                {
                    velocity.X = 0;
                }
            }
        }

        public void gravity(GameTime gameTime) //this is temporary
        {
            velocity.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

}