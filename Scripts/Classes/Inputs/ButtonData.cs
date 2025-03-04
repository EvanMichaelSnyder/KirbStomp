using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.MainLine
{
	public class ButtonData
	{
		private ButtonState _state = ButtonState.Inactive;
		private int _framesPressed = 0;
		public int GetFramesPressed() { return _framesPressed; }
		public ButtonState GetButtonState() { return _state; }

		public ButtonData() { }
		public void FrameUpdate(KeyState keyState)
		{
			if (keyState == KeyState.Inactive)
			{
				if (_state == ButtonState.Pressed || _state == ButtonState.Held)
				{
					_framesPressed = 0;
					_state = ButtonState.Released;   //button just released
				}
				else
				{
					_state = ButtonState.Inactive;   //fully inactive
				}
			}
			else
			{
				_framesPressed++;
				if (/*State == ButtonState.Released ||*/ _state == ButtonState.Inactive)
				{
					_state = ButtonState.Pressed;
				}
				else
				{
					_state = ButtonState.Held;
				}
			}
		}
	}
}
