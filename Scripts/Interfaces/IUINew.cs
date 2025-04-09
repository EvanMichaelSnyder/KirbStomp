using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Scripts.Projectiles;
using System.Collections.Generic;

namespace KirbStomp.Interfaces
{
    internal interface IUINew
    {
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public bool IsVisible { get; set; }
        public List<Sprite> Sprites { get; set; }
        public List<SpriteString> TextSprites { get; set; }
        public void Initialize();
        public void Draw(SpriteBatch spriteBatch);
    }
}
