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
        public ArrowStormItemAbillity(ProjectileManager projectileManager)
        {
            this._projectileManager = projectileManager;
        }
        public override void ExectuteAbillity()
        {
            for(int i = 0;i < _numArrowsToSpawn ; i++)
            {
                for(int j = 0; j < _layers; j++)
                {
                    Vector2 pos = new Vector2(0 -j*_spawnDistance, i * this._spawnDistance);
                    this._projectileManager.AddProjectile(new LinkArrow(pos, true));
                }
            }
        }
    }
}
