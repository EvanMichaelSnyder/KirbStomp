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
    public class LinkArrow : AProjectile
    {
        private readonly String SPR_NAME = "LinkProjectile";
        private readonly String ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private readonly String ANIM_STATE_SPAWN = "ArrowShoot";
        private readonly String ANIM_STATE_DOWN = "ArrowDown";
        private readonly String ANIM_STATE_DEATH = "ArrowDeath";
        private float SCALE = 1.5f;
        private float X_SPEED = 600f;
        private float GRAVITY = 100f;
        private bool _isDead = false;
        private float _deathTimeLeft = 3f;
        private float _noHitTimeLeft = 4f;

        private float DAMAGE = 10;


        public LinkArrow(Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SPAWN;
            this._damage = DAMAGE;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_DOWN));
            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_DEATH));

        }

        public override void RegisterCollider()
        {
            base.RegisterCollider();

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => HitGround(obj, ctx));
        }

        public void HitGround(CollisionObject obj, CollisionContext context)
        {
            this.BeginDeath();
        }

        public override void Update(float dt)
        {
            _animationSystem.Animate(dt);

            UpdateHitbox();
            
            if (this._isDead)
            {
                this._deathTimeLeft -= dt;
                if(this._deathTimeLeft < 0)
                {
                    this.Destroy();
                }
            }
            else
            {
                this._noHitTimeLeft -= dt;
                if( this._noHitTimeLeft < 0)
                {
                    this.Destroy();
                }
            }
            

            Velocity.Y += GRAVITY * dt;
            Position += Velocity * dt;
        }

        public void BeginDeath()
        {
            this._isDead = true;
            this._animationSystem.SetAnimation(ANIM_STATE_DEATH);
            this.Velocity = Vector2.Zero;
            this.GRAVITY = 0;
            this._attackCarrier.IsDisabled = true;
            this._bodyCarrier.IsDisabled = true;
        }
    }
}
