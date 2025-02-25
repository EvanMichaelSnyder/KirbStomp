using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Projectiles
{
    public abstract class GameObject
    {
        protected Vector2 _position = new Vector2();
        
        public abstract void Update(float dt);

        public Vector2 GetPosition()
        {
            return new Vector2(this._position.X, this._position.Y);
        }

        
    }
}
