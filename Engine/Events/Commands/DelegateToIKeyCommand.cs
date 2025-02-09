using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// DONT THINK THIS IS USEDgit 
namespace KirbStomp.Engine.Events.Commands
{
    public delegate bool KeyPressedFN(Keys key);
    internal class DelegateToIKeyCommand : IKeyCommands
    {
        private KeyPressedFN fn;
        public DelegateToIKeyCommand(KeyPressedFN fn)
        {
            this.fn = fn;
            if (this.fn == null) this.fn = NothingFN;
        }

        public void Execute(Keys key)
        {
            fn(key);
        }

        private bool NothingFN(Keys key)
        {
            return false;
        }
    }
}
