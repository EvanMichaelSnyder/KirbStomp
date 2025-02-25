using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Projectiles
{
    public class MarioFireBall : AProjectile
    {
        private Sprite _sprite;
        private AnimationSystem _animationSystem;

        private float _elapsedTime;
        //TODO better than hrd code value
        private Vector2 _velocity;
        private float _gravity;
        private float _scale;
        private bool _isDying;
        private float _deathTimeLeft;
        private float _lifeTime;
        public MarioFireBall(Vector2 startPosition) { 
            this._position.X = startPosition.X;
            this._position.Y = startPosition.Y;
            //setting up fireball
            this._sprite = new Sprite(AssetPool.GetTexture("MarioProjectile"), new Rectangle()/*doesnt matter, will be animated*/, 2f);
            
            this._animationSystem = new AnimationSystem(this._sprite);

            
            Animation releasedAnimation = AssetPool.GetAnimation("MarioProjectileTransparentSpriteSheet", "FireballReleased");
            Animation deathAnimation = AssetPool.GetAnimation("MarioProjectileTransparentSpriteSheet", "FireballDeath");
            this._animationSystem.AddAnimation(releasedAnimation);
            this._animationSystem.AddAnimation(deathAnimation);
            this._animationSystem.SetAnimation("FireballReleased");

            this._velocity = new Vector2(400, 0);
            this._gravity = 1000;
            this._scale = 2f;
            this._elapsedTime = 0;
            this._isDying = false;
            this._deathTimeLeft = .8f;
            this._lifeTime = 5;

            
            

        }

        public override void Update(float dt)
        {
            this._elapsedTime += dt;

            if (this._isDying)
            {
                this._deathTimeLeft -= dt;
                if(this._deathTimeLeft < 0)
                {
                    this.Destroy();
                }
            }
            else if(this._elapsedTime > this._lifeTime)
            {
                this._isDying = true;
                this._animationSystem.SetAnimation("FireballDeath");
                this._velocity = new Vector2();
                this._gravity = 0;
            }

            this._animationSystem.Animate(dt);

            this._velocity.Y += _gravity * dt;
            this._position += this._velocity*dt;
            this.CheckGroundBounce();
            this.CheckWallBounce();
        }

        public void CheckWallBounce()
        {
            if (this._position.X < 0)
            {
                this._velocity.X *= -1f;
            }
            else if (this._position.X > 1400)
            {
                this._velocity.X *= -1f;
            }
        }

        public void CheckGroundBounce()
        {
            if(this._position.Y > 890)
            {
                this._position.Y = 889;
                this._velocity.Y *= -1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this._position);
            //throw new Exception("TEST");
        }

        public void Destroy()
        {
            Game1.Get().GetProjectileManager().RemoveProjectile(this);
        }
    }
}
