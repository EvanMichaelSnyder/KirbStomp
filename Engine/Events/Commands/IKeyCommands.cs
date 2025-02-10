using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.Events.Commands
{
    internal interface IKeyCommands : ICommands
    {
        public void Execute(Keys key);
    }
}
