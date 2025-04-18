
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Classes.Collision.CollisionHandlers;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.GameObjects.Projectiles
{
    public class MarioFireBall : AProjectile
    {
        private float SCALE = 1f;
        private readonly String SPR_NAME = "MarioProjectile";
        private readonly String ANIM_STATE_RELEASED = "FireballReleased";
        private readonly String ANIM_STATE_DEATH = "FireballDeath";
        private readonly String ANIM_NAME = "MarioProjectileTransparentSpriteSheet";
        private float DAMAGE = 5f;
        private float X_SPEED = 400f;
        private float _elapsedTime = 0;
        private float _gravity = 800;
        private bool _isDying = false;
        private float _deathTimeLeft = 1f;
        private float TIME_TO_LIVE = 5;




        public MarioFireBall(Vector2 startPos, bool facingRight)
        {
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_RELEASED;
            this._damage = DAMAGE;
            this._timeToLive = TIME_TO_LIVE;
            this._destroyOnPlayerHit = false;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);
            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_DEATH));

        }


        public override void RegisterCollider()
        {
            base.RegisterCollider();

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => Bounce(obj, ctx));
            RegisterCollisionResponse(HitboxTypeEnum.Attack,
                                    HitboxTypeEnum.Body,
                                    (obj, ctx) => HitPlayer(obj, ctx));
        }



        public void Bounce(CollisionObject obj, CollisionContext context)
        {
            this.Velocity.Y *= -1;
            this.Position.Y -= context.Intersection.Height;
            // Debug.WriteLine("BALL BOUNCE");
        }

        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
            this.BeginDeath();
        }

        public void BeginDeath()
        {
            _isDying = true;
            _animationSystem.SetAnimation(ANIM_STATE_DEATH);
            Velocity = new Vector2();
            _gravity = 0;
            this._attackCarrier.IsDisabled = true;
            this._bodyCarrier.IsDisabled = true;
        }


        public override void Update(float dt)
        {
            //Debug.WriteLine(this._dimension);

            _elapsedTime += dt;

            if (_isDying)
            {
                _deathTimeLeft -= dt;
                if (_deathTimeLeft < 0)
                {
                    Destroy();
                }
            }
            else if (_elapsedTime > TIME_TO_LIVE)
            {
                this.BeginDeath();
            }

            Velocity.Y += _gravity * dt;

            base.Update(dt);

        }

    }
}
