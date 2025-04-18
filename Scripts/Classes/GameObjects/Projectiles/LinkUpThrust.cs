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
    public class LinkUpThrust : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1f;
        private string SPR_NAME = "LinkProjectile";
        private string ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "UpAerialThrustShoot";
        private string ANIM_STATE_AIR = "UpAerialThrustAir";
        private float Y_SPEED = -600f;
        private float DAMAGE = 10f;
        private float TIME_TO_LIVE = 5f;

        private bool _doneShoot = false;


        public LinkUpThrust(Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SHOOT;
            this._damage = DAMAGE;
            this._timeToLive = TIME_TO_LIVE;
            SetVelocity(new Vector2(0, Y_SPEED));

            SetupAnimation(startPos, facingRight);
            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_AIR));
            
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (!_doneShoot)
            {
                if (this._animationSystem.IsAnimationDone())
                {
                    this._doneShoot = true;
                    this._animationSystem.SetAnimation(ANIM_STATE_AIR);
                    this._dimension.Width = (int)(this._sprite.GetSrcRectangle().Width * SCALE);
                    this._xOffSetCollider = this._sprite.GetXOffset();
                }
            }
        }
    }
}
