using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
// Implement a concrete class for a Keyboard controller

public class KeyboardController : IController
{
    private Dictionary<Keys, ICommand> keyCommands; 

    public KeyboardController (Dictionary<Keys, ICommand> keyCommands) 
    { 
        this.keyCommands = keyCommands; 
    }

    public void HandleKeyboardInput(Keys key)
    {
        if(keyCommands.ContainsKey(key)) {
            // Console.WriteLine($"Keyboard: key {key}");

            // execute command according to the key, reset the currentSprite
            keyCommands[key].Execute();
        }
    }

    public void Update()
    {
        // get the keyboard state and all the current keys being pressed
        KeyboardState keyboardState = Keyboard.GetState();
        Keys[] pressedKeys = keyboardState.GetPressedKeys();

        // handle input for all key pressed
        foreach (Keys key in pressedKeys)
        {
            HandleKeyboardInput(key);
        }
    }

}