using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Scripts.Interfaces
{
    internal interface ILevelManager
    {
        public void Update(float dT);
        public void Draw(SpriteBatch spriteBatch);


        public void SpawnPlayerPlatform(int x, int y, int playerHeight);

    }
}
