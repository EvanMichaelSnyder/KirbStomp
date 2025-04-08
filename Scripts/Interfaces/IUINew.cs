using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Scripts.Projectiles;
namespace KirbStomp.Interfaces
{
    internal interface IUINew
    {
        private string Name { public get; set; }
        private Vector2 Position { public get; set; }
        private bool IsVisible { public get; set; }
        private List<Sprite> Sprites { public get; set; }
        private List<StringSprite> TextSprites { public get; set; }
        public void Initialize();
        public void Draw(SpriteBatch spriteBatch);
    }
}
