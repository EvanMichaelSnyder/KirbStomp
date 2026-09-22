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
    public class LinkDownThrust : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1f;
        private string SPR_NAME = "LinkProjectile";
        private string ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "DownAerialThrustShoot";
        private string ANIM_STATE_GROUND = "DownAerialThrustGround";
        private float Y_SPEED = 500f;
        private float DAMAGE = 10f;
        private int GROUND_HEIGHT = 20;
        private float TIME_TO_LIVE = 5f;
        private bool _hitGround = false;

        public LinkDownThrust(Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SHOOT;
            this._damage = DAMAGE;
            this._timeToLive = TIME_TO_LIVE;
            SetVelocity(new Vector2(0, Y_SPEED));

            SetupAnimation(startPos, facingRight);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_GROUND));

        }

        public void HitGround(CollisionObject obj, CollisionContext context)
        {
            if (!_hitGround)
            {
                this._hitGround = true;
                this._bodyCarrier.IsDisabled = true;
                this.Velocity.Y = 0;
                this._animationSystem.SetAnimation(ANIM_STATE_GROUND);
                this.Position.Y -= context.Intersection.Height;
                this._yOffSetCollider = -this._sprite.GetYOffset();
                this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(SCALE * _sprite.GetSrcRectangle().Width), (int)(SCALE * GROUND_HEIGHT));
            }
        }
        public override void RegisterCollider()
        {
            base.RegisterCollider();

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => HitGround(obj, ctx));

        }

        public override void Update(float dt)
        {
            base.Update(dt);
            
            if (_hitGround)
            {
                if (this._animationSystem.IsAnimationDone())
                {
                    this.Destroy();
                }
            }

        }
    }
}
