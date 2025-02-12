using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp
{
    public enum GameButtons
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Attack,
        Special,
        GotHit,
        End,
        HitGround,
        None,
    }
    public enum ButtonState
    {
        Inactive,
        Pressed,
        Held,
        Released, //as in the frame where the button was released 
    }
    public enum KeyState
    {
        Inactive,
        Active,
    }

    internal class ButtonDataManager
    {
        public Dictionary<GameButtons, ButtonData> buttonDataSheet;  //only public for prototype

        public ButtonDataManager()
        {
            buttonDataSheet = new Dictionary<GameButtons, ButtonData>()
            {
                { GameButtons.Up,new ButtonData()},
                { GameButtons.Down,new ButtonData()},
                { GameButtons.Left,new ButtonData()},
                { GameButtons.Right,new ButtonData()},
                { GameButtons.Jump,new ButtonData()},
                { GameButtons.Attack,new ButtonData()},
                { GameButtons.Special,new ButtonData()},

                //debugging
                { GameButtons.HitGround,new ButtonData()},
                { GameButtons.End,new ButtonData()},

            };
        }

        public bool checkPressed(GameButtons button)
        {
            return (buttonDataSheet[button].getButtonState() == ButtonState.Pressed);
        }
        public bool checkHeld(GameButtons button)
        {
            return (buttonDataSheet[button].getButtonState() == ButtonState.Held);
        }
        public bool checkReleased(GameButtons button)
        {
            return (buttonDataSheet[button].getButtonState() == ButtonState.Released);
        }
        public bool checkInactive(GameButtons button)
        {
            return (buttonDataSheet[button].getButtonState() == ButtonState.Inactive);
        }
        public int checkFramesPressed(GameButtons button)
        {
            return buttonDataSheet[button].getFramesPressed();
        }


        public GameButtons directionalPriority()
        {
            var directionalButtons = new List<GameButtons>
            {
                GameButtons.Up,
                GameButtons.Down,
                GameButtons.Left,
                GameButtons.Right
            };

            // Find the directional button with the smallest number of frames pressed
            GameButtons buttonWithSmallestFrames = directionalButtons
                .OrderBy(button => buttonDataSheet[button].getFramesPressed()) // Order by frames pressed (ascending)
                .FirstOrDefault(); // Take the first one (the one with smallest frames pressed)

            return buttonWithSmallestFrames;
        }
        public GameButtons movementPriority()
        {
            var directionalButtons = new List<GameButtons>
            {
                GameButtons.Left,
                GameButtons.Right
            };

            // Find the directional button with the smallest number of frames pressed
            GameButtons buttonWithSmallestFrames = directionalButtons
                .OrderBy(button => buttonDataSheet[button].getFramesPressed()) // Order by frames pressed (ascending)
                .FirstOrDefault(); // Take the first one (the one with smallest frames pressed)

            return buttonWithSmallestFrames;
        }


    }









    internal class ButtonData
    {
        private ButtonState _state = ButtonState.Inactive;
        private int framesPressed = 0;
        public int getFramesPressed() { return framesPressed; }
        public ButtonState getButtonState() { return _state; }

        public ButtonData() { }
        public void FrameUpdate(KeyState keyState)
        {
            if (keyState == KeyState.Inactive)
            {
                if (_state == ButtonState.Pressed || _state == ButtonState.Held)
                {
                    framesPressed = 0;
                    _state = ButtonState.Released;   //button just released
                }
                else
                {
                    _state = ButtonState.Inactive;   //fully inactive
                }
            }
            else
            {
                framesPressed++;
                if (/*State == ButtonState.Released ||*/ _state == ButtonState.Inactive)
                {
                    _state = ButtonState.Pressed;
                }
                else
                {
                    _state = ButtonState.Held;
                }
            }
        }

    }
}
