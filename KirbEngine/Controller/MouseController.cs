using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;


namespace Iron.Iron.Controller
{
	public enum IronMouseButtons
	{
		LeftMouseButton, RightMouseButton, MiddleMouseButton, XButton1, XButton2
	}
	public class IronMouseButtonState
	{
		public IronMouseButtons button;
		public bool isPressed { get; set; }
		public bool isJustPressed { get; set; }
		public bool isReleased { get; set; }
		public bool isJustReleased { get; set; }

		public IronMouseButtonState(IronMouseButtons button)
		{
			this.button = button;
		}
	}

	public delegate void CommandFunction();


	internal class MouseController : IController
	{

		private MouseState previousMouseState;
		private MouseState currentMouseState;

		private Dictionary<IronMouseButtons, IronMouseButtonState> buttonDictionary;
		public List<IronMouseButtons> allButtons { get; }

		public Point position { get; set; }
		public int scrollWheelVertical;
		public int scrollWheelHorizonal;

		/*
		 * What do we need to know? 
		 * Each buttons current state: Just pressed, just released, Pressed (Repeated), Released
		 * Mouse Position: xPos, yPos
		 * Scroll: Hortizonal, Vert
		 * 
		 * These will be read only, this controller will do updates for the states
		 *
		 *
		 *
		 */

		public MouseController()
		{
			this.currentMouseState = Mouse.GetState();
			this.previousMouseState = currentMouseState;
			this.buttonDictionary = new Dictionary<IronMouseButtons, IronMouseButtonState>();

			allButtons = new List<IronMouseButtons>
			{
				IronMouseButtons.LeftMouseButton,
				IronMouseButtons.RightMouseButton,
				IronMouseButtons.MiddleMouseButton,
				IronMouseButtons.XButton1,
				IronMouseButtons.XButton2
			};

			foreach(IronMouseButtons button in allButtons)
			{
				this.buttonDictionary.Add(button, new IronMouseButtonState(button));
			}
			this.position = new Point(0, 0);
			this.scrollWheelHorizonal = 0;
			this.scrollWheelVertical = 0;
		}
		public void UpdateInput()
		{
			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

			// Updates m1-m5 (x1 == m4, x2 == m5)
			UpdateButtonStateInDictionary(IronMouseButtons.LeftMouseButton, previousMouseState.LeftButton, currentMouseState.LeftButton);
			UpdateButtonStateInDictionary(IronMouseButtons.RightMouseButton, previousMouseState.RightButton, currentMouseState.RightButton);
			UpdateButtonStateInDictionary(IronMouseButtons.MiddleMouseButton, previousMouseState.MiddleButton, currentMouseState.MiddleButton);
			UpdateButtonStateInDictionary(IronMouseButtons.XButton1, previousMouseState.XButton1, currentMouseState.XButton1);
			UpdateButtonStateInDictionary(IronMouseButtons.XButton2, previousMouseState.XButton2, currentMouseState.XButton2);

			this.position = currentMouseState.Position;
			this.scrollWheelVertical = currentMouseState.ScrollWheelValue;
			this.scrollWheelHorizonal = currentMouseState.HorizontalScrollWheelValue;
		}

		public IronMouseButtonState GetButtonState(IronMouseButtons button)
		{
			IronMouseButtonState output;
			buttonDictionary.TryGetValue(button, out output);
			return output;
		}

		
		private void UpdateButtonStateInDictionary(IronMouseButtons IronMouseButtonKey, ButtonState previousButtonState, ButtonState currentButtonState)
		{
			buttonDictionary[IronMouseButtonKey].isPressed = currentButtonState == ButtonState.Pressed;
			buttonDictionary[IronMouseButtonKey].isReleased = currentButtonState == ButtonState.Released;
			buttonDictionary[IronMouseButtonKey].isJustPressed = currentButtonState == ButtonState.Pressed && previousButtonState == ButtonState.Released;
			buttonDictionary[IronMouseButtonKey].isJustReleased = currentButtonState == ButtonState.Released && previousButtonState == ButtonState.Pressed;
		}


	}
}
