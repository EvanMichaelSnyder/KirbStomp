using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
namespace KirbStomp.Interfaces {
    internal interface ICommand
    {
        void Execute(KeyState state);
    }
}
    