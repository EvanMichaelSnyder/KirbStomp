using KirbStomp.Scripts.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Scripts.Classes.Managers
{
    internal class DefaultLevelManager : ILevelManager
    {
        public void Update(float dT)
        {
            // Do Nothing Special
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Do Nothing Special
        }


        public void SpawnPlayerPlatform(int x, int y, int playerHeight)
        {

        }
    }
}
