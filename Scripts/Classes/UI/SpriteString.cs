
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;

public class SpriteString
{
    private SpriteFont _font;
    private string _content;
    private float _scale;
    private Color _color;
    private float _rotation;
    private float _zIndex;
    private bool _isFlipped;
    private Vector2 _rotateOrigin;
    private Vector2 _offset;
    private float _globalScaleX;
    private float _globalScaleY;
    private Vector2 _position;
    public string Name { get; set; }
    public SpriteString(SpriteFont font, string content, float scale)
    {
        this._font = font;
        this._content = content;
        this._scale = scale;
        this._isFlipped = false;
        this._rotation = 0;
        this._zIndex = 0;
        this._color = Color.White;
        this._rotateOrigin = Vector2.Zero;
        this._offset = Vector2.Zero;
        this.Name = null;

        this._globalScaleX = (float)Game1.Get().GetScreenWindow().globalScaleX;
        this._globalScaleY = (float)Game1.Get().GetScreenWindow().globalScaleY;
    }
    public SpriteString(string name, SpriteFont font, string content, float scale, Vector2 position)
    {
        this._font = font;
        this._content = content;
        this._scale = scale;
        this._isFlipped = false;
        this._rotation = 0;
        this._zIndex = 0;
        this._color = Color.White;
        this._rotateOrigin = Vector2.Zero;
        this._offset = Vector2.Zero;
        this._position = position;
        this.Name = name;

        this._globalScaleX = (float)Game1.Get().GetScreenWindow().globalScaleX;
        this._globalScaleY = (float)Game1.Get().GetScreenWindow().globalScaleY;
    }
    public SpriteString(SpriteFont font, string content, float scale, Vector2 position)
    {
        this._font = font;
        this._content = content;
        this._scale = scale;
        this._isFlipped = false;
        this._rotation = 0;
        this._zIndex = 0;
        this._color = Color.White;
        this._rotateOrigin = Vector2.Zero;
        this._offset = Vector2.Zero;
        this._position = position;

        this._globalScaleX = (float)Game1.Get().GetScreenWindow().globalScaleX;
        this._globalScaleY = (float)Game1.Get().GetScreenWindow().globalScaleY;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Vector2 coords = new Vector2(position.X, position.Y);
        coords += _offset;
        coords.X *= _globalScaleX;
        coords.Y *= _globalScaleY;

        Vector2 adjustedScale = new Vector2(this._scale * _globalScaleX, this._scale * _globalScaleY);

        SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        spriteBatch.DrawString(this._font, this._content, coords, this._color, this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 coords = new Vector2(this._position.X, this._position.Y);
        coords += _offset;
        coords.X *= _globalScaleX;
        coords.Y *= _globalScaleY;

        Vector2 adjustedScale = new Vector2(this._scale * _globalScaleX, this._scale * _globalScaleY);

        SpriteEffects spriteEffects = this._isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        spriteBatch.DrawString(this._font, this._content, coords, this._color, this._rotation, this._rotateOrigin, adjustedScale, spriteEffects, this._zIndex);
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

    public void FlipStringX(bool flip)
    {
        this._isFlipped = flip;
    }

    public void FlipStringX()
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
    public void SetContent(string content)
    {
        this._content = content;
    }
    public string GetContent()
    {
        return this._content;
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