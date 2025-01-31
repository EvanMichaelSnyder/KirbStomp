using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KirbStomp.Inputs.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KirbStomp.Inputs
{
	public enum InputStatus
	{ 
		Pressed, JustPressed, Released, JustReleased
	}

	internal class GlobalInputs
    {
		private KeyboardController keyboard;
		private MouseController mouse;

		private Dictionary<Keys, HashSet<ICommands>> keysPressedCBFNDictionary;
		private Dictionary<Keys, HashSet<ICommands>> keysJustPressedCBFNDictionary;
		private Dictionary<Keys, HashSet<ICommands>> keysJustReleasedCBFNDictionary;

		// Purely for development, it's easier than using switchcases to me
		private Dictionary<InputStatus, Dictionary<Keys, HashSet<ICommands>>> statusDictionaryDictionary;



		public GlobalInputs()
        {
			keysPressedCBFNDictionary = new();
			keysJustPressedCBFNDictionary = new();
			keysJustReleasedCBFNDictionary = new();
			statusDictionaryDictionary = new() // Maybe this wasn't the best Idea...
			{
				{InputStatus.Pressed, keysPressedCBFNDictionary},
				{InputStatus.JustPressed,  keysJustPressedCBFNDictionary},
				{InputStatus.JustReleased,  keysJustReleasedCBFNDictionary }
			};
			keyboard = KeyboardController.GetInstance();
			mouse = MouseController.GetInstance();
		}

        public void UpdateAllControllers()
		{
			keyboard.Update();
			CallBackNecessaryKeys();
			mouse.Update();
			//CallbackNecessaryMouseStates(); TODO
		}


		// IF WE WANT GLOBAL KEYCODES, THIS IS WHERE IT WOULD BE IMPLEMENTED
		// Note: we could check type and have 1 method, or we can not and have 2 overloaded methods
		public bool IsInputPressed(Keys key)
		{ return keyboard.IsKeyPressed(key);}
		public bool IsInputPressed(MouseButtons button)
		{return mouse.IsButtonPressed(button);}

		// Just Pressed
		public bool IsInputJustPressed(Keys key)
		{return keyboard.IsKeyJustPressed(key);}
		public bool IsInputJustPressed(MouseButtons button)
		{return mouse.IsButtonJustPressed(button);}
		
		// Released
		public bool IsInputReleased(Keys key)
		{return keyboard.IsKeyReleased(key);}
		public bool IsInputReleased(MouseButtons button)
		{return mouse.IsButtonReleased(button);}

		// Just Released
		public bool IsInputJustReleased(Keys key)
		{return keyboard.IsKeyJustReleased(key);}
		public bool IsInputJustReleased(MouseButtons button)
		{return mouse.IsButtonJustReleased(button);}


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


		public bool AddCommandToKeyStatus(Keys key, ICommands command, InputStatus status)
		{
			if (status == InputStatus.Released) return false;
			Dictionary<Keys, HashSet<ICommands>>  dict = statusDictionaryDictionary[status];
			HashSet<ICommands> commands;

			if (!dict.TryGetValue(key, out commands))
				dict.Add(key, commands = new());

			commands.Add(command);
			return true;
		}

		private void CallBackNecessaryKeys()
		{
			CallBackKeysAndFunctionsFromDictionary(keysPressedCBFNDictionary, keyboard.GetKeysPressed());
			CallBackKeysAndFunctionsFromDictionary(keysJustPressedCBFNDictionary, keyboard.GetKeysJustPressed());
			CallBackKeysAndFunctionsFromDictionary(keysJustReleasedCBFNDictionary, keyboard.GetKeysJustReleased());
		}

		private void CallBackKeysAndFunctionsFromDictionary(Dictionary<Keys, HashSet<ICommands>> dictionaryOfICommandsToCallBack,
			HashSet<Keys> keysToCallBack)
		{
			HashSet<ICommands> objCommands;
			foreach (Keys key in keysToCallBack)
			{
				if (dictionaryOfICommandsToCallBack.TryGetValue(key, out objCommands))
				{
					foreach (ICommands commandObj in objCommands)
						commandObj.Execute();
				}
			}
		}

	}
}
