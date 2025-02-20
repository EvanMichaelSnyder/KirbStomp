using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using Microsoft.Xna.Framework.Input;


internal class KeyboardController : IController
    {
        //through here keys map to commands
        private Dictionary<Keys, ICommand> _commands;
        public KeyboardController(ButtonDataManager buttons)
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
            RegisterCommand(Keys.W, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Up]));
            RegisterCommand(Keys.A, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Left]));
            RegisterCommand(Keys.S, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Down]));
            RegisterCommand(Keys.D, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Right]));
            RegisterCommand(Keys.G, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Attack]));
            RegisterCommand(Keys.H, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Special]));
            RegisterCommand(Keys.J, new UpdateButtonCommand(buttons.buttonDataSheet[KirbStomp.GameButtons.Jump]));
        }
        public void Update()
        {
            var state = Keyboard.GetState();
            foreach (var key in _commands.Keys)
            {

                if (state.IsKeyDown(key))
                {
                    _commands[key].Execute(KirbStomp.KeyState.Active);
                }

                else if (!state.IsKeyDown(key))
                {
                    _commands[key].Execute(KirbStomp.KeyState.Inactive);
                }
               
            }
        }


    }
