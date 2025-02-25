using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Projectiles
{
    public abstract class AProjectile : GameObject
    {
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
