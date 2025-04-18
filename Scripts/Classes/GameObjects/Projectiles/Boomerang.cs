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
    public class Boomerang : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1f;
        private string SPR_NAME = "LinkProjectile";
        private string ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private string ANIM_STATE_NAME = "Boomerang";
        private float X_SPEED = 500f;
        private float MAX_TIME_DIRECTION = 1f;
        private float DAMAGE = 5f; 

        private float _timeDirection = 0;
        private bool _hasSwitchedDirection = false;

        public Boomerang(Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_NAME;
            this._damage = DAMAGE;
            this._destroyOnPlayerHit = false;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);

        }


        public override void Update(float dt)
        {
            this._animationSystem.Animate(dt);

            UpdateHitbox();

            this._timeDirection += dt;

            if (this._hasSwitchedDirection)
            {
                if (_timeDirection >= MAX_TIME_DIRECTION)
                {
                    this.Destroy();
                }
            }
            else if(this._timeDirection >= MAX_TIME_DIRECTION)
            {
                this._timeDirection = 0;
                this.Velocity *= -1f;
                this._hasSwitchedDirection = true;
            }

            Position += Velocity * dt;

        }
    }
}
