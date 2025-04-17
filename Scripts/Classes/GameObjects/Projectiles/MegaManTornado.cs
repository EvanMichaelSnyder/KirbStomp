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
        private float _scale = 1.85f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "Tornado";
        // private float X_SPEED = 400f;
        private float DAMAGE = 5f;
        private float TIME_TO_LIVE = 5f;
        private float _gravity = 800f;

        private float X_BOUND = 100f;      // left/right bound
        private float X_SPEED = 5f;
        private float _elapsedTime = 0f;
        private Vector2 _base;             


        private Sprite _sprite;
        private AnimationSystem _animationSystem;


        public MegaManTornado(Vector2 startPos, bool facingRight)
        {
            this.Position = startPos;
            this._base = startPos;
            Texture2D texture = AssetPool.GetTexture(SPR_NAME);
            this._sprite = new Sprite(texture, new Rectangle()/*doesnt matter, anim will change*/, _scale);
            this._animationSystem = new AnimationSystem(_sprite);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_SHOOT));
            this._animationSystem.SetAnimation(ANIM_STATE_SHOOT);

            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(_scale * _sprite.GetSrcRectangle().Width), (int)(_scale * _sprite.GetSrcRectangle().Height));

            this.Velocity = new Vector2(X_SPEED, 0);
            if (!facingRight)
            {
                this.Velocity.X *= -1f;
                this._sprite.FlipTextureX();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
            if (this.DrawHitbox) {
                this._bodyCarrier.HitboxManager.Draw(spriteBatch);
            }  

        }

        public override void RegisterCollider()
        {
            this._attackCarrier.SetDamage(DAMAGE);
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => HitGround(obj, ctx));
        }

        public void HitGround(CollisionObject obj, CollisionContext context)
        {
            this.Velocity.Y = 0;
            this.Position.Y -= context.Intersection.Height;
            this._yOffSetCollider = -this._sprite.GetYOffset();
            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(_scale * _sprite.GetSrcRectangle().Width), (int)(_scale * _sprite.GetSrcRectangle().Height));
        }
        public override void Update(float dt)
        {
            this._animationSystem.Animate(dt);

            this._dimension.Width = (int)(_scale * _sprite.GetSrcRectangle().Width);
            this._dimension.Height = (int)(_scale * _sprite.GetSrcRectangle().Height);

            this._yOffSetCollider = this._sprite.GetYOffset();
            this._xOffSetCollider = this._sprite.GetXOffset();
            
            TIME_TO_LIVE -= dt;
            if (TIME_TO_LIVE < 0)
            {
                this.Destroy();
                return;
            }

            _elapsedTime += dt;

            // moving the tornado zig zag restricted by the bound and pull down by gravity
            Position.X = _base.X + X_BOUND * MathF.Sin(X_SPEED * _elapsedTime);
            Velocity.Y += _gravity * dt;

            Position.Y += Velocity.Y * dt;

        }
    }
}
