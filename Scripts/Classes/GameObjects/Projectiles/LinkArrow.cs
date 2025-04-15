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
        private readonly String TEXTURE_NAME = "LinkProjectile";
        private readonly String ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private readonly String ANIM_STATE_SPAWN = "ArrowShoot";
        private readonly String ANIM_STATE_DOWN = "ArrowDown";
        private readonly String ANIM_STATE_DEATH = "ArrowDeath";
        private readonly int WIDTH = 32;
        private readonly int HEIGHT = 32;
        private float _scale = 1.5f;
        private Sprite _sprite;
        private AnimationSystem _animationSystem;
        private float _maxSpeedX = 600;
        private float _gravity = 60;
        private bool _isDead = false;
        private float _deathTimeLeft = 3f;
        private float _noHitTimeLeft = 4f;

        private float DAMAGE = 10;

        

        
        

        public LinkArrow(Vector2 position, bool facingRight)
        {
            
            this.Position = position;
            this._sprite = new Sprite(AssetPool.GetTexture(TEXTURE_NAME), new Rectangle()/*doesnt matter, will be animated*/, _scale);
            this._animationSystem = new AnimationSystem(_sprite);

            Animation spawnArrow = AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_SPAWN);
            Animation downArrow = AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_DOWN);
            Animation downDeath = AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_DEATH);

            this._animationSystem.AddAnimation(spawnArrow);
            this._animationSystem.AddAnimation(downArrow);
            this._animationSystem.AddAnimation(downDeath);

            this._animationSystem.SetAnimation(ANIM_STATE_SPAWN);

            this.Velocity = new Vector2(_maxSpeedX, 0);
            if (!facingRight)
            {
                this.Velocity.X *= -1;
                this._sprite.FlipTextureX();
            }

            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(WIDTH * _scale), (int)(HEIGHT* _scale));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
        }

        public override void RegisterCollider()
        {
            this._attackCarrier.SetDamage(DAMAGE);
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => HitGround(obj, ctx));
            RegisterCollisionResponse(HitboxTypeEnum.Attack,
                                    HitboxTypeEnum.Body,
                                    (obj, ctx) => HitPlayer(obj, ctx));
        }

        public void HitGround(CollisionObject obj, CollisionContext context)
        {
            this.BeginDeath();
            
        }

        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
            
            this.Destroy();
        }

        public override void Update(float dt)
        {
            _animationSystem.Animate(dt);
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
            

            Velocity.Y += _gravity * dt;
            Position += Velocity * dt;
        }

        public void BeginDeath()
        {
            this._isDead = true;
            this._animationSystem.SetAnimation(ANIM_STATE_DEATH);
            this.Velocity = Vector2.Zero;
            this._gravity = 0;
            this._attackCarrier.IsDisabled = true;
            this._bodyCarrier.IsDisabled = true;
        }
    }
}
