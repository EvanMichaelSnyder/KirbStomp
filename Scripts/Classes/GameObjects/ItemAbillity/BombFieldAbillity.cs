using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.GameObjects.Projectiles;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.GameObjects.ItemAbillity
{
    internal class BombFieldAbillity : AItemAbillity
    {
        private readonly int NUM_SPAWN = 4;
        private readonly float DIST_BETWEEN = 200;
        private ProjectileManager _projectileManager;
        private Character _spawner;
        public BombFieldAbillity(ProjectileManager projectileManager, Character spawner) 
        {
            this._projectileManager = projectileManager;
            this._spawner = spawner;
                    
        }
        public override void ExectuteAbillity()
        {
            Random rand = new Random();
            bool faceRight = false;
            if (rand.Next(0, 2) == 0)
            {
                faceRight = true;
            }
            for (int i = 0; i < NUM_SPAWN; i++)
            {
                
                Vector2 pos = new Vector2(i* DIST_BETWEEN, 0);
                AProjectile bomb = new Bomb(pos, faceRight);
                bomb.SetSpawningCharacter(_spawner);
                this._projectileManager.AddProjectile(bomb);
            }
        }
    }
}
