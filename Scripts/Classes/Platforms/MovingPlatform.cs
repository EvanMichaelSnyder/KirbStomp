using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Platforms
{
    public class MovingPlatform : CollisionObject
    {
        private Sprite _sprite;
        private Rectangle _posHW;
        private float _timeAlive = 0;
        private PlatformCarrier _platformCarrier;
        public MovingPlatform(Sprite spr, Rectangle posHW)
        { 
            this._sprite = spr;
            this._posHW = posHW;
            this.Position.X = _posHW.X;
            this.Position.Y = _posHW.Y;
            _platformCarrier = new PlatformCarrier() { Parent = this };
            Carriers.Add(_platformCarrier);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, _posHW);
           // _platformCarrier.HitboxManager.Draw(spriteBatch);
        }

        public void Update(float dt)
        {
            this._timeAlive += dt;
            this._posHW.X = (int)Position.X;
            this._posHW.Y = (int)Position.Y;

            this._platformCarrier.HitboxManager.basicUpdateHitbox(this._posHW);



        }

        public float GetTimeAlive()
        {
            return this._timeAlive;
        }
    }
}
