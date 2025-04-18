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
    public class MegaManMetalBlade : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1.6f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "MetalBlade";
        private float X_SPEED = 400f;
        private float DAMAGE = 5f;
        private float TIME_TO_LIVE = 5f;

        public MegaManMetalBlade(Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SHOOT;
            this._damage = DAMAGE;
            this._timeToLive = TIME_TO_LIVE;
            this._destroyOnPlayerHit = false;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);

        }
    }
}
