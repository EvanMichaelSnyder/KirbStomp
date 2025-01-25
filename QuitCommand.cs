using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

// concrete class for quitting the program command
public class QuitCommand : ICommand
{
    public void Execute()
    {
        // exit program with no error
        Environment.Exit(0);
    }

}