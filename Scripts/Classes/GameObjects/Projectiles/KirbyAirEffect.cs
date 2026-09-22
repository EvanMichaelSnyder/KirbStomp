using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.GameObjects.Projectiles
{
    public class KirbyAirEffect : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1.3f;
        private string SPR_NAME = "KirbyProjectile";
        private string ANIM_NAME = "KirbyProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "AirEffect";
        private float X_SPEED = 600f;
        private float DAMAGE = 5f;
        private float TIME_TO_LIVE = 5f;


        public KirbyAirEffect (Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SHOOT;
            this._damage = DAMAGE;
            this._timeToLive = TIME_TO_LIVE;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);
        }

    }
}
