using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Platforms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Managers
{
    public class LevelManager
    {
        private int MAX_HEIGHT_PLATFORM_APART = 300;
        private int MIN_HEIGHT_PLATFORM_APART = 100;
        private int PLATFORM_HEIGHT = 25;
        private int MIN_PLATFORM_WIDTH = 100;
        private int MAX_PLATFORM_WIDTH = 400;
        private int NUMBER_OF_STARTING_PLATFORM = 5;

        private float _distanceMovedSinceLastPlatformSpawned = 0;
        private Vector2 _platformVelocity;
        private Texture2D _platformTexture;
        private Random _random;

        private int worldWidth, worldHeight;

        private List<Platform> _platforms;
        public LevelManager(Vector2 platformVelocity, Texture2D platformTexture, int worldWidth, int worldHeight) {

            this._platformVelocity = platformVelocity;
            this._platforms = new List<Platform>();
            this._platformTexture = platformTexture;
            this._random = new Random();

            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;


        }

        public void SpawnPlatform()
        {
            int platformWidth = _random.Next(MIN_PLATFORM_WIDTH, MAX_PLATFORM_WIDTH);
            int platform;
            Rectangle posHW = new Rectangle();
        }


        public void Update(float dt)
        {

        }

    }

}
