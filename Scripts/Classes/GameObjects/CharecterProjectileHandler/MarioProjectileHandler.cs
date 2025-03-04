using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.GameObjects.Projectiles;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.GameObjects.CharecterProjectileHandler
{
    public class MarioProjectileHandler : ICharacterProjectileHandler
    {
        public void DoAttack1(Vector2 position, bool facingRight)
        {
            AProjectile fireball = new MarioFireBall(position, facingRight);
            //SceneManager.Get().GetCurrentScene().GetProjectileManager().AddProjectile(fireball);
        }

        
    }
}
