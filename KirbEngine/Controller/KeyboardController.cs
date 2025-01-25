using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Iron.Iron.Controller
{
	public class IronKeyButtonState
	{
		public Keys key;
		public bool isPressed { get; set; }
		public bool isJustPressed { get; set; }
		public bool isReleased { get; set; }
		public bool isJustReleased { get; set; }

		public IronKeyButtonState(Keys key)
		{
			this.key = key;
		}
	}

	internal class KeyboardController : IController
	{
		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;

		private Dictionary<Keys, IronKeyButtonState> keyStateDictionary;
		public KeyboardController()
		{
			currentKeyboardState = Keyboard.GetState();
			previousKeyboardState = currentKeyboardState;
			keyStateDictionary = new Dictionary<Keys, IronKeyButtonState>();
		}

		public bool RegisterKey(Keys key)
		{
			return keyStateDictionary.TryAdd(key, new IronKeyButtonState(key));
		}


		public void UpdateInput()
		{
			this.previousKeyboardState = currentKeyboardState;
			this.currentKeyboardState = Keyboard.GetState();
			foreach (Keys key in keyStateDictionary.Keys)
			{
				UpdateKeyInDictionary(key, currentKeyboardState.IsKeyDown(key), previousKeyboardState.IsKeyDown(key));
			}
		}
		public IronKeyButtonState GetKeyState(Keys key)
		{
			IronKeyButtonState output;
			keyStateDictionary.TryGetValue(key, out output);
			return output;
		}
		private void UpdateKeyInDictionary(Keys key, bool keyIsDown, bool prevKeyWasDown)
		{
			keyStateDictionary[key].isPressed = keyIsDown;
			keyStateDictionary[key].isReleased = !keyIsDown;
			keyStateDictionary[key].isJustPressed = keyIsDown && !prevKeyWasDown;
			keyStateDictionary[key].isJustReleased = !keyIsDown && prevKeyWasDown;
		}

	}
}
