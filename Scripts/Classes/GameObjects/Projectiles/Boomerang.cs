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
        private float _scale = 1f;
        private string SPR_NAME = "LinkProjectile";
        private string ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private string ANIM_STATE_NAME = "Boomerang";
        private float X_SPEED = 500f;
        private float MAX_TIME_DIRECTION = 1f;
        private int WIDTH = 16;
        private int HEIGHT = 16;
        private float DAMAGE = 5; 


        private Sprite _sprite;
        private AnimationSystem _animationSystem;
        private float _timeDirection = 0;
        private bool _hasSwitchedDirection = false;

        public Boomerang(Vector2 startPos, bool facingRight)
        {
            this.Position = startPos;
            Texture2D texture = AssetPool.GetTexture(SPR_NAME);
            this._sprite = new Sprite(texture, new Rectangle()/*doesnt matter, anim will change*/, _scale);
            this._animationSystem = new AnimationSystem(_sprite);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_NAME));
            this._animationSystem.SetAnimation(ANIM_STATE_NAME);

            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, WIDTH, HEIGHT);

            this.Velocity = new Vector2(X_SPEED,0);
            if (!facingRight)
            {
                this.Velocity.X *= -1f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
            if (this.DrawHitbox) {
                this._bodyCarrier.HitboxManager.Draw(spriteBatch);
            }  

        }

        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
           //TODO probaly keep bomerang going, so not instanly destroy on hit.
        }

        public override void RegisterCollider()
        {
            this._attackCarrier.SetDamage(DAMAGE);
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);

            RegisterCollisionResponse(HitboxTypeEnum.Attack,
                                    HitboxTypeEnum.Body,
                                    (obj, ctx) => HitPlayer(obj, ctx));
        }

        public override void Update(float dt)
        {
            this._animationSystem.Animate(dt);

            this._dimension.Width = (int)(_scale * _sprite.GetSrcRectangle().Width);
            this._dimension.Height = (int)(_scale * _sprite.GetSrcRectangle().Height);

            this._yOffSetCollider = this._sprite.GetYOffset();
            this._xOffSetCollider = this._sprite.GetXOffset();

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
