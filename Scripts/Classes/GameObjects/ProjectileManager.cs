using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Projectiles
{
    public class ProjectileManager
    {
        private List<AProjectile> _projectiles;
        private List<AProjectile> _removedPool;
        private CollisionSystem _collisionSystem;
        public ProjectileManager(CollisionSystem collisionSystem) 
        {
            this._projectiles = new List<AProjectile>();
            this._removedPool = new List<AProjectile>();
            this._collisionSystem = collisionSystem;
        }

        public void AddProjectile(AProjectile projectile)
        {

            this._projectiles.Add(projectile);
            projectile.SetProjectileManager(this);
            projectile.RegisterCollider();
            this._collisionSystem.RegisterObject(projectile);
        }

        public void RemoveProjectile(AProjectile projectile)
        {
            if (!this._projectiles.Contains(projectile))
            {
                throw new Exception("projectile cannot be removed, dne: ");
            }
            this._removedPool.Add(projectile);
        }

        public void Update(float dt)
        {


            foreach (AProjectile go in this._projectiles)
            {
                
                go.Update(dt);
                go.UpdateCollider();
            }

            foreach (AProjectile go in this._removedPool)
            {
                this._projectiles.Remove(go);
                this._collisionSystem.RemoveObject(go);
            }
            this._removedPool.Clear();




        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AProjectile go in this._projectiles)
            {
                go.Draw(spriteBatch);
            }
        }
    }
}
