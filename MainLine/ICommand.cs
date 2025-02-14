using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;

internal interface ICommand
    {
        void Execute(KeyState state);
    }