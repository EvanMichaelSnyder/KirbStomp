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
    public class MegaManTornado : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1.85f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "Tornado";
        private float DAMAGE = 10f;
        private float TIME_TO_LIVE = 5f;
        private float _gravity = 800f;

        // left/right bound
        private float X_BOUND = 100f;     
        private float X_SPEED = 5f;
        private float _elapsedTime = 0f;
        private Vector2 _base;             

        public MegaManTornado(Vector2 startPos, bool facingRight)
        {
            this._base = startPos;
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

        public override void RegisterCollider()
        {
            base.RegisterCollider();

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => HitGround(obj, ctx));
        }

        public void HitGround(CollisionObject obj, CollisionContext context)
        {
            this.Velocity.Y = 0;
            this.Position.Y -= context.Intersection.Height;
            this._yOffSetCollider = -this._sprite.GetYOffset();
            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(SCALE * _sprite.GetSrcRectangle().Width), (int)(SCALE * _sprite.GetSrcRectangle().Height));
        }
        public override void Update(float dt)
        {
            //update velocity before update position
            Velocity.Y += _gravity * dt;

            base.Update(dt);

            // moving the tornado zig zag restricted by the bound and pull down by gravity
            _elapsedTime += dt;
            Position.X = _base.X + X_BOUND * MathF.Sin(X_SPEED * _elapsedTime);
            
        }
    }
}
