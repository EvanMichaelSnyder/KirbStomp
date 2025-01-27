using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    internal class KeyboardController : IController
    {
        //through here keys map to commands
        private Dictionary<Keys, ICommand> _commands;
        public KeyboardController(Game1 game, CharacterLuigi player)
        {
            _commands = new Dictionary<Keys, ICommand>();
            DefaultKeyAssignments(game, player);

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
        public void DefaultKeyAssignments(Game1 game, CharacterLuigi player)
        {
            _commands.Clear();
            //Assigns 0 to quit
            RegisterCommand(Keys.D0, new QuitCommand(game));
            //Assigns 1 to static sprite
            RegisterCommand(Keys.D1, new BecomeUnanimatedStillSpriteCommand(player));
            //Assigns 2 to animated sprite
            RegisterCommand(Keys.D2, new BecomeAnimatedStillSpriteCommand(player));
            //Assigns 3 to static sprite moving up
            RegisterCommand(Keys.D3, new BecomeUnanimatedMovingSpriteCommand(player));
            //Assigns 4 to animated sprite moving to the side
            RegisterCommand(Keys.D4, new BecomeAnimatedMovingSpriteCommand(player));
        }
        public void Update()
        {
            var state = Keyboard.GetState();
            foreach (var key in _commands.Keys)
            {
                if (state.IsKeyDown(key))
                    _commands[key].Execute();
            }
        }


    }
}
