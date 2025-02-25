using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Projectiles
{
    public class Sprite
    {
        private Texture2D _spriteSheet;
        private Rectangle _srcRectangle;
        private float _scale;
        public Sprite(Texture2D sprSheet, Rectangle src, float scale) 
        {
            this._spriteSheet = sprSheet;
            this._srcRectangle = src;
            this._scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle destRect = new Rectangle((int)position.X,(int) position.Y, (int)(_srcRectangle.Width* _scale), (int)(_srcRectangle.Height* _scale));
            spriteBatch.Draw(this._spriteSheet, destRect, _srcRectangle, Color.White);
        }

        public void SetSrcRectangle(Rectangle srcRectangle)
        {
            this._srcRectangle = srcRectangle;
        }

    }
}
