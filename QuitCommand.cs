using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    internal class QuitCommand : ICommand
    {
        private Game _game;
        public QuitCommand(Game1 game)
        {
            _game = game;
        }
        public void Execute()
        {
            _game.Exit();
        }

    }
}
