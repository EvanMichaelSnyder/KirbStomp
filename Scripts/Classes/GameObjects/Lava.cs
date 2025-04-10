using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.GameObjects
{
    public class Lava : CollisionObject
    {
        Sprite _sprite;
        public Lava(String texture)
        {
            Texture2D tex = AssetPool.GetTexture(texture);
            Rectangle srcRect = new Rectangle(0,0, tex.Width, tex.Height);
            _sprite = new Sprite(tex, srcRect, 1f);

            this.Position = new Vector2(0, (float)Game1.Get().GetScreenWindow().globalScaleY-srcRect.Height);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
        }

    }
}
