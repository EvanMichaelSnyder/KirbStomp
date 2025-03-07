
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
        private Sprite _sprite;
        private AnimationSystem _animationSystem;

        private float _elapsedTime = 0;
        private float _gravity = 800;
        private float _scale = 1f;
        private bool _isDying = false;
        private float _deathTimeLeft = 1f;
        private float _lifeTime = 5;

        

        
        public MarioFireBall(Vector2 startPosition, bool facingRight)
        {
            Position.X = startPosition.X;
            Position.Y = startPosition.Y;
            //setting up fireball
            _sprite = new Sprite(AssetPool.GetTexture("MarioProjectile"), new Rectangle()/*doesnt matter, will be animated*/, 1f);

            _animationSystem = new AnimationSystem(_sprite);


            Animation releasedAnimation = AssetPool.GetAnimation("MarioProjectileTransparentSpriteSheet", "FireballReleased");
            Animation deathAnimation = AssetPool.GetAnimation("MarioProjectileTransparentSpriteSheet", "FireballDeath");
            _animationSystem.AddAnimation(releasedAnimation);
            _animationSystem.AddAnimation(deathAnimation);
            _animationSystem.SetAnimation("FireballReleased");

            Velocity = new Vector2(400, 0);
            if (!facingRight)
            {
                Velocity.X *= -1;
            }
           
           
            
            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, 16, 16);
            


        }


        public override void RegisterCollider()
        {
            // this.ProvideCharacterCarriers();
            this._dimension.Height *= (int)this._scale;
            this._dimension.Width *= (int)this._scale;
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => Bounce(obj, ctx));
            RegisterCollisionResponse(HitboxTypeEnum.Attack,
                                    HitboxTypeEnum.Body,
                                    (obj, ctx) => HitPlayer(obj,ctx));
        }



        public void Bounce(CollisionObject obj, CollisionContext context)
        {
            this.Velocity.Y *= -1;
            this.Position.Y -= 5 + context.Intersection.Height;
           // Debug.WriteLine("BALL BOUNCE");
        }

        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
            this.BeginDeath();
        }

        public void BeginDeath()
        {
            _isDying = true;
            _animationSystem.SetAnimation("FireballDeath");
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
            else if (_elapsedTime > _lifeTime)
            {
                this.BeginDeath();
            }

            _animationSystem.Animate(dt);

            Velocity.Y += _gravity * dt;
            Position += Velocity * dt;
            
        }

        

        

        public override void Draw(SpriteBatch spriteBatch)
        {
            _bodyCarrier.HitboxManager.Draw(spriteBatch);
            _sprite.Draw(spriteBatch, Position);
            //throw new Exception("TEST");
        }




    }
}
