using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace KirbStomp.Interfaces
{
    internal interface IUI
    {
        public void Initialize();
        public void Draw(SpriteBatch spriteBatch);
    }
}
