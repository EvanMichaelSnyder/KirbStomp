using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSV2.Systems.ISystems{
    internal interface IDrawableSystem : ISystem
    {
		public void Draw(SpriteBatch spriteBatch);
    }
}
