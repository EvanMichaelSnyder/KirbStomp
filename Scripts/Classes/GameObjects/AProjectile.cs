using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Classes.Collision.CollisionHandlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Projectiles
{
    public abstract class AProjectile : CollisionObject
    {
        //manager
        protected ProjectileManager _projectileManager;
        //collisios
        protected BodyCarrier _bodyCarrier;
        protected AttackCarrier _attackCarrier;
        protected Rectangle _dimension;
        protected Character _spawningCharacter;
        protected bool drawHitbox = false;
        protected int _xOffSetCollider = 0;
        protected int _yOffSetCollider = 0;

        //animation
        protected float _scale = 1f;
        protected string _spriteName;
        protected string _animName;
        protected string _animState;
        protected float _damage = 5f;
        protected float _timeToLive = 5f;
        protected bool _destroyOnPlayerHit = true;

        protected Sprite _sprite;
        protected AnimationSystem _animationSystem;

        //only used by projectilemanager to avoid extra param in projectiles
        public void SetProjectileManager(ProjectileManager projectileManager)
        {
            if (projectileManager == null) return;
            if(this._projectileManager == null)
            {
                this._projectileManager = projectileManager;
            }
        }

        //for setting up common animation for each projectile, remove duplicate codes
        protected void SetupAnimation(Vector2 startPos, bool facingRight)
        {
            this.Position = startPos;

            Texture2D texture = AssetPool.GetTexture(_spriteName);
            this._sprite = new Sprite(texture, new Rectangle(), _scale);
            this._animationSystem = new AnimationSystem(_sprite);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(_animName, _animState));
            this._animationSystem.SetAnimation(_animState);

            this._dimension = new Rectangle(
                                (int)this.Position.X, 
                                (int)this.Position.Y, 
                                (int)(_scale * _sprite.GetSrcRectangle().Width), 
                                (int)(_scale * _sprite.GetSrcRectangle().Height));

            //flip direction
            if (!facingRight)
            {
                Velocity.X *= -1f;
                _sprite.FlipTextureX();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
            if (this.drawHitbox) {
                this._bodyCarrier.HitboxManager.Draw(spriteBatch);
            } 

        }

        //for setting up the volliders for collision
        public void ProvideProjectileCarriers()
        {
            _bodyCarrier = new BodyCarrier() { Parent = this };
            _attackCarrier = new AttackCarrier() { Parent = this };
            Carriers.Add(_bodyCarrier);
            Carriers.Add(_attackCarrier);
        }
        public virtual void RegisterCollider()
        {
            this._attackCarrier.SetDamage(_damage);
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            if(_destroyOnPlayerHit)
            {
                RegisterCollisionResponse(HitboxTypeEnum.Attack,
                                          HitboxTypeEnum.Body,
                                          (obj, ctx) => HitPlayer(obj, ctx));
            }
        }
        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
            this.Destroy();
        }

        public void Destroy()
        {
            this._projectileManager.RemoveProjectile(this);
        }

        public Rectangle getDimensions()
        {
            return _dimension;
        }

        //this is a common projectile update, special projectiles will override this
        public virtual void Update(float dt)
        {
            this._animationSystem.Animate(dt);
            UpdateHitbox();
            
            _timeToLive -= dt;
            if (_timeToLive < 0)
            {
                this.Destroy();
                return;
            }

            Position += Velocity * dt;

        }

        public void UpdateHitbox() {
            this._dimension.Width = (int)(_scale * _sprite.GetSrcRectangle().Width);
            this._dimension.Height = (int)(_scale * _sprite.GetSrcRectangle().Height);

            this._yOffSetCollider = this._sprite.GetYOffset();
            this._xOffSetCollider = this._sprite.GetXOffset();
        }

        public void UpdateCollider()
        {
            this._dimension.X = (int)this.Position.X + this._xOffSetCollider;
            this._dimension.Y = (int)this.Position.Y + this._yOffSetCollider;

            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
        }

        //getters and setters
        public AttackCarrier GetAttackCarrier()
        {
            return this._attackCarrier;
        }

        public void SetSpawningCharacter(Character character)
        {
            this._spawningCharacter = character;
        }

        public Character GetSpawningCharacter()
        {
            return this._spawningCharacter;
        }
        public void SetVelocity(Vector2 velocity)
        {
            this.Velocity.X = velocity.X;
            this.Velocity.Y = velocity.Y;
        }
        public Vector2 GetVelocity()
        {
            return new Vector2(Velocity.X, Velocity.Y);
        }

    }
}
