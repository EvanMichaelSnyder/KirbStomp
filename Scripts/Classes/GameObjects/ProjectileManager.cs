using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.GameObjects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Projectiles
{
    public class ProjectileManager
    {
        private List<AProjectile> _projectiles;
        private List<AProjectile> _removedPool;
        private List<AProjectile> _addedPool;
        private CollisionSystem _collisionSystem;

        
        public ProjectileManager(CollisionSystem collisionSystem) 
        {
            this._projectiles = new List<AProjectile>();
            this._removedPool = new List<AProjectile>();
            this._addedPool = new List<AProjectile>();
            this._collisionSystem = collisionSystem;
        }

        public void AddProjectile(AProjectile projectile)
        {

            this._addedPool.Add(projectile);
           
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

            foreach (AProjectile projectile in this._addedPool)
            {
                this._projectiles.Add(projectile);
                projectile.SetProjectileManager(this);
                projectile.ProvideProjectileCarriers();
                projectile.RegisterCollider();
                this._collisionSystem.RegisterObject(projectile);
            }
            this._addedPool.Clear();


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AProjectile go in this._projectiles)
            {
                go.Draw(spriteBatch);
            }
        }


        //TODO maybe make this into its own class/thing
        public void SpawnProjectile(String projectileName, Vector2 position, bool facingRight)
        {
            
            Type type = Type.GetType(projectileName);
            //try to find w/o extra
            if(type == null)
            {
                type = Type.GetType("KirbStomp.Scripts.Classes.GameObjects.Projectiles." +projectileName);
            }
            if (type == null)
            {
                throw new Exception("projectilename: " +projectileName+ " is not a valid class");
            }
            else if (!type.IsSubclassOf(typeof(AProjectile))){
                throw new Exception("projectilename: " + projectileName +" is not a projectile");
            }
            AProjectile projectile = (AProjectile)Activator.CreateInstance(type, position, facingRight);
            this.AddProjectile(projectile);
        }

        public void Reset(CollisionSystem collisionSystem)
        {
            this._collisionSystem = collisionSystem;
            this._addedPool.Clear();
            this._removedPool.Clear();
            this._projectiles.Clear();
        }

        public void SpawnProjectile(String projectileName, Vector2 position, bool facingRight, Character spawner)
        {

            Type type = Type.GetType(projectileName);
            //try to find w/o extra
            if (type == null)
            {
                type = Type.GetType("KirbStomp.Scripts.Classes.GameObjects.Projectiles." + projectileName);
            }
            if (type == null)
            {
                throw new Exception("projectilename: " + projectileName + " is not a valid class");
            }
            else if (!type.IsSubclassOf(typeof(AProjectile)))
            {
                throw new Exception("projectilename: " + projectileName + " is not a projectile");
            }
            AProjectile projectile = (AProjectile)Activator.CreateInstance(type, position, facingRight);
            projectile.SetSpawningCharacter(spawner);
            this.AddProjectile(projectile);
        }


    }
}
