using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using Microsoft.Xna.Framework;


    internal class UpdateButtonCommand : ICommand
    {
        private ButtonData _button;
        public UpdateButtonCommand(ButtonData button)
        {
            _button = button;
        }
        public void Execute(KeyState state)
        {
        _button.FrameUpdate(state);
        }

    }