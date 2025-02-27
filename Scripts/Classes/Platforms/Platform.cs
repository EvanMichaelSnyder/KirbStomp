using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Platforms
{
    public enum PlatformTypeEnum
    {
        None,
        SideDirtPlatform
    }
    internal class Platform
    {
        private Rectangle _platformDimensions;
        private AllPurposeSprite _sprite;
        private PlatformTypeEnum _platformType;
        private int _ID;
        private HitboxManager _hitboxManager;


        public Platform(PlatformTypeEnum type, Rectangle rectangle,Texture2D spriteSheet)
        {
            _ID = BattleScene.getNewID();
            _platformDimensions = rectangle;
            _platformType = type;
            _sprite = new AllPurposeSprite(spriteSheet);
            _hitboxManager = new HitboxManager(BattleScene.boxSheet, HitboxTypeEnum.Platform);
        }

        public void Draw(SpriteBatch spriteBatch)

        {
            _sprite.DrawRectangle(spriteBatch, _platformDimensions);
        }
    }
}
