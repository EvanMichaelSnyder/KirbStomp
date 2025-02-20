using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.MainLine;

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
        public Dictionary<GameButtons, ButtonData> ButtonDataSheet;  //only public for prototype

        public ButtonDataManager()
        {
            ButtonDataSheet = new Dictionary<GameButtons, ButtonData>()
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

        public bool CheckPressed(GameButtons button)
        {
            return (ButtonDataSheet[button].GetButtonState() == ButtonState.Pressed);
        }
        public bool CheckHeld(GameButtons button)
        {
            return (ButtonDataSheet[button].GetButtonState() == ButtonState.Held);
        }
        public bool CheckReleased(GameButtons button)
        {
            return (ButtonDataSheet[button].GetButtonState() == ButtonState.Released);
        }
        public bool CheckInactive(GameButtons button)
        {
            return (ButtonDataSheet[button].GetButtonState() == ButtonState.Inactive);
        }
        public int CheckFramesPressed(GameButtons button)
        {
            return ButtonDataSheet[button].GetFramesPressed();
        }


        public GameButtons DirectionalPriority()
        {
            var directionalButtons = new List<GameButtons>
            {
                GameButtons.None,
                GameButtons.Up,
                GameButtons.Down,
                GameButtons.Left,
                GameButtons.Right
            };

            // Find the directional button with the smallest number of frames pressed
            GameButtons buttonWithSmallestFrames = directionalButtons
                .OrderBy(button => ButtonDataSheet[button].GetFramesPressed()) // Order by frames pressed (ascending)
                .FirstOrDefault(); // Take the first one (the one with smallest frames pressed)

            return buttonWithSmallestFrames;
        }
        public GameButtons MovementPriority()
        {
            var directionalButtons = new List<GameButtons>
            {
                GameButtons.None,
                GameButtons.Left,
                GameButtons.Right
            };

            // Find the directional button with the smallest number of frames pressed
            GameButtons buttonWithSmallestFrames = directionalButtons
                .OrderBy(button => ButtonDataSheet[button].GetFramesPressed()) // Order by frames pressed (ascending)
                .FirstOrDefault(); // Take the first one (the one with smallest frames pressed)

            return buttonWithSmallestFrames;
        }


    }



}
