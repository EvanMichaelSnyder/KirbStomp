using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.MainLine;
using Microsoft.Xna.Framework;
using KirbStomp.Interfaces;

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