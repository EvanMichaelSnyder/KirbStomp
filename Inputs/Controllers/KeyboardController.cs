using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Inputs.Controllers
{
	public delegate void KeyCallBackFN(Keys key);
	// NOTE: This class will inherintly be ties to Xna.Framework.Input.Keyboard and it's states
	internal class KeyboardController : IController
	{
		// There'll only be one instance of this class, therefore there will never be any duplicates of these members
		private HashSet<Keys> keysPressed;
		private HashSet<Keys> keysJustPressed;
		private HashSet<Keys> keysJustReleased;

		private  KeyboardState previousState;
		private  KeyboardState currentState;

		private static KeyboardController instance;
		private KeyboardController()
		{
			keysPressed = new();
			keysJustPressed = new();
			keysJustReleased = new();
			currentState = Keyboard.GetState();
		}

		public static KeyboardController GetInstance()
		{
			if (instance == null) instance = new KeyboardController();
			return instance;
		}

		public void Update()
		{
			previousState = currentState;
			currentState = Keyboard.GetState();
			ResetKeysSets();
			UpdateKeysStates();
		}

		private void ResetKeysSets()
		{
			keysPressed.Clear();
			keysJustPressed.Clear();
			keysJustReleased.Clear();
		}

		private void UpdateKeysStates()
		{

			Keys[] currentlyPressedKeys = currentState.GetPressedKeys();
			Keys[] previouslyPressedKeys = previousState.GetPressedKeys();
			IEnumerable<Keys> justReleasedKeys = previouslyPressedKeys.Except(currentlyPressedKeys); // 
			foreach (Keys key in justReleasedKeys)
			{
				keysJustReleased.Add(key);
			}
			foreach (Keys key in currentlyPressedKeys)
			{
				keysPressed.Add(key);
				if (previousState.IsKeyUp(key))
				{
					keysJustPressed.Add(key);
				}
			}
		}
		public HashSet<Keys> GetKeysPressed()
		{
			return this.keysPressed;
		}
		public bool IsKeyPressed(Keys key)
		{
			return keysPressed.Contains(key);
		}
		public HashSet<Keys> GetKeysJustPressed()
		{
			return this.keysJustPressed;
		}
		public bool IsKeyJustPressed(Keys key)
		{
			return keysJustPressed.Contains(key);
		}
		public HashSet<Keys> GetKeysJustReleased()
		{
			return this.keysJustReleased;
		}
		public bool IsKeyJustReleased(Keys key)
		{
			return keysJustReleased.Contains(key);
		
		}
		public bool IsKeyReleased(Keys key)
		{
			return !keysPressed.Contains(key);
		}


	}
}
