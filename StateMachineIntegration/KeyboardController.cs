using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using Microsoft.Xna.Framework.Input;


internal class KeyboardController2 : IController
    {
        //through here keys map to commands
        private Dictionary<Keys, ICommand> _commands;
        public KeyboardController2(ButtonDataManager buttons)
        {
            _commands = new Dictionary<Keys, ICommand>();
            DefaultKeyAssignments(buttons);

        }

        //Attempts to register command returns 0 if successful, -1 if not
        public int RegisterCommand(Keys key, ICommand command)
        {
            //check for overlap
            if (_commands.ContainsKey(key)) return -1;
            _commands[key] = command;    
            return 0;

        }

        //Defaults the key assignments can also be called mid game to reset the key assignments if changed
        public void DefaultKeyAssignments(ButtonDataManager buttons)
        {
            _commands.Clear();
            RegisterCommand(Keys.W, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Up]));
            RegisterCommand(Keys.A, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Left]));
            RegisterCommand(Keys.S, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Down]));
            RegisterCommand(Keys.D, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Right]));
            RegisterCommand(Keys.G, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Attack]));
            RegisterCommand(Keys.H, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Special]));
            RegisterCommand(Keys.J, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.Jump]));
            RegisterCommand(Keys.T, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.End]));
            //RegisterCommand(Keys.Y, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Special]));
            RegisterCommand(Keys.V, new UpdateButtonCommand(buttons.buttonDataSheet[GameButtons.HitGround]));

    }
        public void Update()
        {
            var state = Keyboard.GetState();
            foreach (var key in _commands.Keys)
            {

                if (state.IsKeyDown(key))
                {
                    _commands[key].Execute(KeyState.Active);
                }

                else if (!state.IsKeyDown(key))
                {
                    _commands[key].Execute(KeyState.Inactive);
                }
               
            }
        }


    }
