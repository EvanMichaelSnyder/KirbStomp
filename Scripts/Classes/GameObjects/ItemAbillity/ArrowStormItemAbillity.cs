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
    public class ArrowStormItemAbillity : AItemAbillity
    {
        private ProjectileManager _projectileManager;
        private int _numArrowsToSpawn = 10;
        private int _spawnDistance = 100;
        private int _layers = 3;
        private Character _spawner;
        public ArrowStormItemAbillity(ProjectileManager projectileManager, Character spawner)
        {
            this._projectileManager = projectileManager;
            this._spawner = spawner;
        }
        public override void ExectuteAbillity()
        {
            for(int i = 0;i < _numArrowsToSpawn ; i++)
            {
                for(int j = 0; j < _layers; j++)
                {
                    Vector2 pos = new Vector2(0 -j*_spawnDistance, i * this._spawnDistance);
                    AProjectile arrow = new LinkArrow(pos, true);
                    arrow.SetSpawningCharacter(_spawner);
                    this._projectileManager.AddProjectile(arrow);
                }
            }
        }
    }
}
