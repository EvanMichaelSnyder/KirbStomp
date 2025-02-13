using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;

    internal class ActionList
    {

        internal List<GameButtons> actions;

        public ActionList()
        {
            actions = new List<GameButtons>();
        }
        public void resetList()
        {
            actions.Clear();
        }
        public void addAction(GameButtons buttonType)
        {
            actions.Add(buttonType);
        }

        public void processButton(ButtonData button, GameButtons buttonType)
        {
            if (IsDirection(buttonType))
            {
                if (button.getButtonState() == ButtonState2.Pressed ||
                    button.getButtonState() == ButtonState2.Held)
                {
                    addAction(buttonType);
                }
            }
            else if (button.getButtonState() == ButtonState2.Pressed)
            {
                addAction(buttonType);
            }

        }

        static bool IsDirection(GameButtons input)
        {
            return input == GameButtons.Left || input == GameButtons.Right
                || input == GameButtons.Up || input == GameButtons.Down;
        }
    }