using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KirbStomp.Engine.Events.Commands;
using KirbStomp.Engine.Inputs.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace KirbStomp.Engine.Inputs
{
    public enum InputStatus
    {
        Pressed, JustPressed, Released, JustReleased
    }


    internal class GlobalInputs
    {
        private KeyboardController keyboard;
        private MouseController mouse;

        private IKeyCommands keyPressedCBFN;
        private IKeyCommands keyJustPressedCBFN;
        private IKeyCommands keyJustReleasedCBFN;

        private HashSet<ICommands> mouseMovedCBFNDictionary;
        // Purely for development, it's easier than using switchcases to me
        private Dictionary<InputStatus, Dictionary<Keys, HashSet<IKeyCommands>>> statusDictionaryDictionary;


        private static GlobalInputs instance;

        private GlobalInputs()
        {
            keyPressedCBFN = new DelegateToIKeyCommand(default);
            keyJustPressedCBFN = new DelegateToIKeyCommand(default);
            keyJustReleasedCBFN = new DelegateToIKeyCommand(default);
            mouseMovedCBFNDictionary = new();
            keyboard = KeyboardController.GetInstance();
            mouse = MouseController.GetInstance();
        }
        public static GlobalInputs GetInstance()
        {
            if (instance == null) instance = new GlobalInputs();
            return instance;
        }

        public void UpdateAllControllers()
        {
            keyboard.Update();
            CallBackNecessaryKeys();
            mouse.Update();
        }


        // IF WE WANT GLOBAL KEYCODES, THIS IS WHERE IT WOULD BE IMPLEMENTED
        // Note: we could check type and have 1 method, or we can not and have 2 overloaded methods
        public bool IsInputPressed(Keys key)
        { return keyboard.IsKeyPressed(key); }
        public bool IsInputPressed(MouseButtons button)
        { return mouse.IsButtonPressed(button); }

        // Just Pressed
        public bool IsInputJustPressed(Keys key)
        { return keyboard.IsKeyJustPressed(key); }
        public bool IsInputJustPressed(MouseButtons button)
        { return mouse.IsButtonJustPressed(button); }

        // Released
        public bool IsInputReleased(Keys key)
        { return keyboard.IsKeyReleased(key); }
        public bool IsInputReleased(MouseButtons button)
        { return mouse.IsButtonReleased(button); }

        // Just Released
        public bool IsInputJustReleased(Keys key)
        { return keyboard.IsKeyJustReleased(key); }
        public bool IsInputJustReleased(MouseButtons button)
        { return mouse.IsButtonJustReleased(button); }


        // Some Mouse Only Stuff
        // Positions
        public Vector2 GetMousePosition()
        { return mouse.GetPosition().ToVector2(); }
        public void SetMousePosition(Vector2 position)
        { mouse.SetMousePosition(position.ToPoint()); }
        // Scroll Wheels
        public int GetScrollWheel()
        { return mouse.GetScrollWheel(); }
        public int GetHorizontalScrollWheel()
        { return mouse.GetHorizontalScrollWheel(); }

        // Possible TODO
        // Joystick controllers

        // Calling Commands Back


        public bool AddCommandToKeyStatus(Keys key, IKeyCommands command, InputStatus status) // Can be cleaned up
        {
            if (status == InputStatus.Released) return false;
            Dictionary<Keys, HashSet<IKeyCommands>> dict = statusDictionaryDictionary[status];
            HashSet<IKeyCommands> commands;

            if (!dict.TryGetValue(key, out commands))
                dict.Add(key, commands = new());

            commands.Add(command);
            return true;
        }
        public void AddMousePositionCallback(ICommands command)
        {
            mouseMovedCBFNDictionary.Add(command);
        }
        private void CallBackNecessaryKeys()
        {
            foreach (Keys key in keyboard.GetKeysPressed())
                keyPressedCBFN.Execute(key);
            foreach (Keys key in keyboard.GetKeysJustPressed())
                keyJustPressedCBFN.Execute(key);
            foreach (Keys key in keyboard.GetKeysJustReleased())
                keyJustReleasedCBFN.Execute(key);
        }





    }
}

