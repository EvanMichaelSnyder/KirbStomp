using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Platforms
{
    public enum PlatformTypeEnum
    {
        None,
        SideDirtPlatform
    }
    internal class Platform : CollisionObject
    {
        private Rectangle _platformDimensions;
        private AllPurposeSprite _sprite;
        private PlatformTypeEnum _platformType;
        private int _ID;
        internal PlatformCarrier PlatformCarrier;


        public Rectangle GetPosition()
        {
            return PlatformCarrier.HitboxManager.GetApproximation();
        }

        public Platform(PlatformTypeEnum type, Rectangle rectangle,Texture2D spriteSheet)
        {
            {
                _ID = BattleScene.getNewID();
                _platformDimensions = rectangle;
                _platformType = type;
                _sprite = new AllPurposeSprite(spriteSheet);

                PlatformCarrier = new PlatformCarrier() { Parent = this };
                Carriers.Add(PlatformCarrier);

                Rectangle rectangleAdjusted = new Rectangle(rectangle.X + 32, rectangle.Y, rectangle.Width - 64, rectangle.Height);
                PlatformCarrier.HitboxManager.basicUpdateHitbox(rectangleAdjusted);
            }
        }


        public void Draw(SpriteBatch spriteBatch)

        {
            _sprite.DrawRectangle(spriteBatch, _platformDimensions);
        }

        public void DrawHitbox(SpriteBatch spriteBatch)
        {
            PlatformCarrier.HitboxManager.Draw(spriteBatch);
        }
    }

}
