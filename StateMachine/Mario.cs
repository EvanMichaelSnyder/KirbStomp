using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;

namespace KirbStomp
{
    internal class Mario : ICharacter
    {
        private StateMachine stateMachine;
        private ButtonDataManager buttonDataManager;
        private ActionList actionList;
        private Vector2 position, velocity;
        public ISpriteComplete sprite;

        public Mario(Texture2D spriteSheet)
        {
            velocity = Vector2.Zero;
            position.X = 200;
            position.Y = 200;
            stateMachine = new StateMachine();
            buttonDataManager = new ButtonDataManager();
            actionList = new ActionList();
            sprite = new AllPurposeSprite(spriteSheet, MarioSpriteSheetMapping.convertToMarioState[stateMachine.State.CurrentState]);
        }
        public ButtonDataManager GetButtonDataManager
        {
            get => buttonDataManager;
        }
        public void doBehavior()
        {
            stateMachine.performBehavior();
            actionList.resetList();
            if (stateMachine.State.AnimationFrame >= 1000)
            {
                actionList.addAction(GameButtons.End);
            }
        }

        public void debugState()
        {
            Debug.WriteLine("State: " + stateMachine.State.CurrentState + " Frame: " + stateMachine.State.AnimationFrame);
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }
            //just pass current facing direction current state enum and current frame
            //stateMachine.State.CurrentState();

        internal void HandleStates()
        {
            foreach (var input in actionList.actions.Where(i => IsDirection(i)))
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
        public void updateState()
        {
            HandleStates();
            if (MarioSpriteSheetMapping.isFinalFrame.ContainsKey(sprite.GetAnimation()))
                {
                if (MarioSpriteSheetMapping.isFinalFrame[sprite.GetAnimation()])
                {
                    sprite.ChangeAnimation(MarioSpriteSheetMapping.convertToMarioState[this.stateMachine.State.CurrentState]);
                }
            }   
            }

            static bool IsDirection(GameButtons input)
        {
            return input == GameButtons.Left || input == GameButtons.Right
                || input == GameButtons.Up || input == GameButtons.Down;
        }

        internal void ProcessButtons()
        {
            foreach (var button in buttonDataManager.buttonDataSheet.Keys)
            {
                actionList.processButton(buttonDataManager.buttonDataSheet[button],button);
            }

        }
    }

}