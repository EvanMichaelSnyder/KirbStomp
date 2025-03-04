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
        protected ProjectileManager _projectileManager;

        protected BodyCarrier _bodyCarrier;
        protected AttackCarrier _attackCarrier;

        protected Rectangle _dimension;


        public abstract void Draw(SpriteBatch spriteBatch);

        public void ProvideProjectileCarriers()
        {
            _bodyCarrier = new BodyCarrier() { Parent = this };
            _attackCarrier = new AttackCarrier() { Parent = this };
            Carriers.Add(_bodyCarrier);
            Carriers.Add(_attackCarrier);
        }

        //only used by projectilemanager to avoid extra param in projectiles
        public void SetProjectileManager(ProjectileManager projectileManager)
        {
            if (projectileManager == null) return;
            if(this._projectileManager == null)
            {
                this._projectileManager = projectileManager;
            }
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
        public void Destroy()
        {
            this._projectileManager.RemoveProjectile(this);
        }

        public abstract void Update(float dt);

        public void UpdateCollider()
        {
            this._dimension.X = (int)this.Position.X;
            this._dimension.Y = (int)this.Position.Y;
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
        }

        public abstract void RegisterCollider();


    }
}
