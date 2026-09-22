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
    public class MegaManChargeShot : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1.75f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "ChargeShot";
        private float X_SPEED = 200f;
        private float DAMAGE = 5f;

        public MegaManChargeShot (Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SHOOT;
            this._damage = DAMAGE;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);
            
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            // destroy when animation is done
            if (this._animationSystem.IsAnimationDone())
            {
                this.Destroy();
                return;
            } 

        }
    }
}
