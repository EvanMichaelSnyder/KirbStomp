using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.MainLine;

namespace KirbStomp
{

    internal class ActionList
    {

        internal List<GameButtons> actions;

        public ActionList()
        {
            actions = new List<GameButtons>();
        }
        public void ResetList()
        {
            actions.Clear();
        }
        public void AddAction(GameButtons buttonType)
        {
            actions.Add(buttonType);
        }

        public void ProcessButton(ButtonData button, GameButtons buttonType)
        {
            if (IsDirection(buttonType))
            {
                if (button.GetButtonState() == ButtonState.Pressed ||
                    button.GetButtonState() == ButtonState.Held)
                {
                    AddAction(buttonType);
                }
            }
            else if (button.GetButtonState() == ButtonState.Pressed)
            {
                AddAction(buttonType);
            }

        }

        static bool IsDirection(GameButtons input)
        {
            return input == GameButtons.Left || input == GameButtons.Right
                || input == GameButtons.Up || input == GameButtons.Down;
        }
    }
}
