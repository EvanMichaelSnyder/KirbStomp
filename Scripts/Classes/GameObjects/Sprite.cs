
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
        private Color _color;
        private float _rotation;
        private float _zIndex;
        private bool _isFlipped;
        private Vector2 _rotateOrigin;
        private Vector2 _offset;
        private Vector2 _position;
        public Sprite(Texture2D sprSheet, Rectangle src, float scale)
        {
            this._spriteSheet = sprSheet;
            this._srcRectangle = src;
            this._scale = scale;
            this._isFlipped = false;
            this._rotation = 0;
            this._zIndex = 0;
            this._color = Color.White;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 coords = _position;
            coords += _offset;
            coords.X *= (float)Game1.Get().GetScreenWindow().globalScaleX;
            coords.Y *= (float)Game1.Get().GetScreenWindow().globalScaleY;

            Vector2 adjustedScale = new Vector2(this._scale * (float)Game1.Get().GetScreenWindow().globalScaleX, this._scale * (float)Game1.Get().GetScreenWindow().globalScaleY);

            SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(this._spriteSheet, coords, this._srcRectangle, this._color, this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 coords = new Vector2(position.X, position.Y);
            coords += _offset;
            coords.X *= (float)Game1.Get().GetScreenWindow().globalScaleX;
            coords.Y *= (float)Game1.Get().GetScreenWindow().globalScaleY;

            Vector2 adjustedScale = new Vector2(this._scale * (float)Game1.Get().GetScreenWindow().globalScaleX, this._scale * (float)Game1.Get().GetScreenWindow().globalScaleY);

            SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(this._spriteSheet, coords, this._srcRectangle, this._color, this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int alphaPercent)
        {
            Vector2 coords = new Vector2(position.X, position.Y);
            coords += _offset;
            coords.X *= (float)Game1.Get().GetScreenWindow().globalScaleX;
            coords.Y *= (float)Game1.Get().GetScreenWindow().globalScaleY;

            Vector2 adjustedScale = new Vector2(this._scale * (float)Game1.Get().GetScreenWindow().globalScaleX, this._scale * (float)Game1.Get().GetScreenWindow().globalScaleY);

            SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(this._spriteSheet, coords, this._srcRectangle, new Color(this._color.R, this._color.G, this._color.B, (int)(alphaPercent * 2.55f)), this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
        }

        public void SetSrcRectangle(Rectangle srcRectangle)
        {
            this._srcRectangle = srcRectangle;
        }

        public Rectangle GetSrcRectangle()
        {
            return this._srcRectangle;
        }

        public void SetScale(float scale)
        {
            this._scale = scale;
        }

        public float GetScale()
        {
            return this._scale;
        }

        public void SetRotation(float rotation)
        {
            this._rotation = rotation;
        }

        public void SetColor(Color color)
        {
            this._color = color;
        }

        public void FlipTextureX(bool flip)
        {
            this._isFlipped = flip;
        }

        public void FlipTextureX()
        {
            this._isFlipped = !this._isFlipped;
        }

        public void SetZIndex(float zIndex)
        {
            this._zIndex = zIndex;
        }

        public void SetRotateOrigin(Vector2 origin)
        {
            this._rotateOrigin = origin; ;
        }

        public void SetOffset(Vector2 offset)
        {
            this._offset = offset;
        }

        public int GetXOffset()
        {
            return (int)this._offset.X;
        }

        public int GetYOffset()
        {
            return (int)this._offset.Y;
        }

    }
}
