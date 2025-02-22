using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Interfaces
{
	internal interface ISpriteComplete
	{
		void Draw(SpriteBatch spriteBatch, Vector2 location, DirectionEnum direction, StateEnum state, int frame, string name);
	}
}
