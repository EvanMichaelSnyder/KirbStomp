using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Inputs.Controllers
{
	public enum MouseButtons
	{
		LeftMouseButton, RightMouseButton, MiddleMouseButton, XButton1, XButton2
	}
	internal class MouseController : IController
	{
		private static MouseController instance;
		private delegate bool GetButtonIsPressed(MouseState button);
		
		private MouseState previousState;
		private MouseState currentState;
		private Point mousePosition;
		private int scrollWheel;
		private int horizonalScrollWheel;
		private Dictionary<MouseButtons, GetButtonIsPressed> buttonDictionary;
		
		private MouseController()
		{
			currentState = Mouse.GetState();
			buttonDictionary = new()
			{
				{ MouseButtons.LeftMouseButton,     (MouseState state) => state.LeftButton      == ButtonState.Pressed },
				{ MouseButtons.MiddleMouseButton,   (MouseState state) => state.MiddleButton    == ButtonState.Pressed },
				{ MouseButtons.RightMouseButton,    (MouseState state) => state.RightButton     == ButtonState.Pressed },
				{ MouseButtons.XButton1,            (MouseState state) => state.XButton1        == ButtonState.Pressed },
				{ MouseButtons.XButton2,            (MouseState state) => state.XButton2        == ButtonState.Pressed }
			};
		}
		public static MouseController GetInstance()
		{
			if (instance == null) instance = new MouseController();
			return instance;
		}

		public void Update()
		{
			previousState = currentState;
			currentState = Mouse.GetState();

			mousePosition = currentState.Position;
			scrollWheel = currentState.ScrollWheelValue;
			horizonalScrollWheel = currentState.HorizontalScrollWheelValue;

		}

		public bool IsButtonPressed(MouseButtons buttons)
		{
			GetButtonIsPressed fn;
			buttonDictionary.TryGetValue(buttons, out fn);
			return fn(currentState);
		}
		public bool IsButtonJustPressed(MouseButtons buttons)
		{
			GetButtonIsPressed fn;
			buttonDictionary.TryGetValue(buttons, out fn);
			return fn(currentState) && !fn(previousState);
		}
		public bool IsButtonReleased(MouseButtons buttons)
		{
			GetButtonIsPressed fn;
			buttonDictionary.TryGetValue(buttons, out fn);
			return !fn(currentState);
		}
		public bool IsButtonJustReleased(MouseButtons buttons)
		{
			GetButtonIsPressed fn;
			buttonDictionary.TryGetValue(buttons, out fn);
			return !fn(currentState) && fn(previousState);
		}

		public Point GetPosition()
		{
			return this.mousePosition;
		}
		public void SetMousePosition(Point position)
		{
			this.mousePosition = position;
		}
		public int GetScrollWheel()
		{
			return this.scrollWheel;
		}
		public int GetHorizontalScrollWheel()
		{
			return this.horizonalScrollWheel;
		}
	}
}
