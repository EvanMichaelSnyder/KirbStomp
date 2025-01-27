using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    internal class MouseController :IController
    {
        //through here keys map to commands
        private Dictionary<int, ICommand> _commands;
        public MouseController(Game1 game, CharacterLuigi player)
        {
            _commands = new Dictionary<int, ICommand>();
            DefaultKeyAssignments(game, player);

        }

        //Attempts to register command returns 0 if successful, -1 if not
        public int RegisterCommand(int key, ICommand command)
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
            //Assigns 1 to static sprite
            RegisterCommand(1, new BecomeUnanimatedStillSpriteCommand(player));
            //Assigns 2 to animated sprite
            RegisterCommand(2, new BecomeAnimatedStillSpriteCommand(player));
            //Assigns 3 to static sprite moving up
            RegisterCommand(3, new BecomeUnanimatedMovingSpriteCommand(player));
            //Assigns 4 to animated sprite moving to the side
            RegisterCommand(4, new BecomeAnimatedMovingSpriteCommand(player));
        }

        public void Update()
        {
            var state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed)
            {
                Vector2 cursorCoords = state.Position.ToVector2();
                if (cursorCoords.X <= 400)//left half
                {
                    if (cursorCoords.Y <= 240)//top left
                    {
                        _commands[1].Execute();
                    }
                    else//bottom left
                    {
                        _commands[3].Execute();
                    }
                }
                else if (cursorCoords.Y <= 240)//top right
                {
                    _commands[2].Execute();
                }
                else//bottom right
                {
                    _commands[4].Execute();
                }
            }
        }


    }
}