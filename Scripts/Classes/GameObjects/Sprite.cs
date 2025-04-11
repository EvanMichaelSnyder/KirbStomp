
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
        private int _alphaPercent;
        public string Name { get; set; }
        public Sprite(Texture2D sprSheet, Rectangle src, float scale)
        {
            this._spriteSheet = sprSheet;
            this._srcRectangle = src;
            this._scale = scale;
            this._isFlipped = false;
            this._rotation = 0;
            this._zIndex = 0;
            this._color = Color.White;
            this.Name = null;
            this._alphaPercent = 100;
        }

        public Sprite(string name, Texture2D sprSheet, Rectangle src, float scale)
        {
            this._spriteSheet = sprSheet;
            this._srcRectangle = src;
            this._scale = scale;
            this._isFlipped = false;
            this._rotation = 0;
            this._zIndex = 0;
            this._color = Color.White;
            this.Name = name;
            this._alphaPercent = 100;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 coords = _position;
            coords += _offset;
            coords.X *= (float)Game1.Get().GetScreenWindow().globalScaleX;
            coords.Y *= (float)Game1.Get().GetScreenWindow().globalScaleY;

            Vector2 adjustedScale = new Vector2(this._scale * (float)Game1.Get().GetScreenWindow().globalScaleX, this._scale * (float)Game1.Get().GetScreenWindow().globalScaleY);

            SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(this._spriteSheet, coords, this._srcRectangle, new Color(this._color.R, this._color.G, this._color.B, (int)(_alphaPercent * 2.55f)), this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 coords = new Vector2(position.X, position.Y);
            coords += _offset;
            coords.X *= (float)Game1.Get().GetScreenWindow().globalScaleX;
            coords.Y *= (float)Game1.Get().GetScreenWindow().globalScaleY;

            Vector2 adjustedScale = new Vector2(this._scale * (float)Game1.Get().GetScreenWindow().globalScaleX, this._scale * (float)Game1.Get().GetScreenWindow().globalScaleY);

            SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(this._spriteSheet, coords, this._srcRectangle, new Color(this._color.R, this._color.G, this._color.B, (int)(_alphaPercent * 2.55f)), this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
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

        public void Draw(SpriteBatch spriteBatch, Rectangle posHW)
        {
            Rectangle coordsHW = new Rectangle((int)(posHW.X + _offset.X),(int)(posHW.Y + _offset.Y), posHW.Width, posHW.Height );
            coordsHW.X = (int)(Game1.Get().GetScreenWindow().globalScaleX * coordsHW.X);
            coordsHW.Y = (int)(Game1.Get().GetScreenWindow().globalScaleY * coordsHW.Y);

            coordsHW.Width = (int)(Game1.Get().GetScreenWindow().globalScaleX * coordsHW.Width);
            coordsHW.Height = (int)(Game1.Get().GetScreenWindow().globalScaleY * coordsHW.Height);


            SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(this._spriteSheet,coordsHW, this._srcRectangle, _color, _rotation, this._rotateOrigin, spriteEffects, _zIndex);
            
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
        public float GetRotation()
        {
            return this._rotation;
        }

        public void SetColor(Color color)
        {
            this._color = color;
        }
        public Color GetColor()
        {
            return this._color;
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
        public float GetZIndex()
        {
            return this._zIndex;
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
        public void SetAlphaPercent(int alphaPercent)
        {
            this._alphaPercent = alphaPercent;
        }
        public Texture2D GetTexture()
        {
            return this._spriteSheet;
        }
        public Vector2 GetPosition()
        {
            return this._position;
        }
        public void SetPosition(Vector2 position)
        {
            this._position = position;
        }
    }
}
