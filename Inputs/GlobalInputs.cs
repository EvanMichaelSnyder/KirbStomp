using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Inputs.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KirbStomp.Inputs
{

    internal class GlobalInputs
    {
		private KeyboardController keyboard;
		private MouseController mouse;
        public GlobalInputs()
        {
			keyboard = KeyboardController.GetInstance();
			mouse = MouseController.GetInstance();
		}

        public void UpdateAllControllers()
        {
			keyboard.Update();
			mouse.Update();
        }

		// IF WE WANT GLOBAL KEYCODES, THIS IS WHERE IT WOULD BE IMPLEMENTED
		// Note: we can check type and have 1 method, or we can not and have 2 overloaded methods
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
	}
}
