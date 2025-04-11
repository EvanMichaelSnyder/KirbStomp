using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.GameObjects;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Managers
{
    //a manager that simulate the idea of rising lava. 
    public class LevelManager
    {
        private int MAX_HEIGHT_PLATFORM_APART = 250;
        private int MIN_HEIGHT_PLATFORM_APART = 150;
        private int PLATFORM_HEIGHT = 30;
        private int MIN_PLATFORM_WIDTH = 100;
        private int MAX_PLATFORM_WIDTH = 700;
        private int DIST_SPAWN_ABOVE = 0;

        private float TIME_TO_LIVE = 20f;

        private int _distanceTillNextSpawnPlatform = 0;
        private float _platformSpeedY;
        private Random _random;
        private CollisionSystem _collisionSystem;

        private int worldWidth, worldHeight;

        //TODO abstract out sprite
        private Sprite _spritePlatform;
        private Rectangle PLATFORM_SRC = new Rectangle(4, 4, 300, 140);
        private float SCALE = 1;

        private List<MovingPlatform> _platforms;
        private List<MovingPlatform> _removePool;
        private Lava _lava;
        private int _worldWidth;
        public LevelManager(CollisionSystem collisionSystem, float platformSpeedY, String platformTexture, int worldWidth, int worldHeight) {

            //this._lava = new Lava("lava");
            this._worldWidth = worldWidth;
            this._collisionSystem = collisionSystem;
            this._platformSpeedY = platformSpeedY;
            this._platforms = new List<MovingPlatform>();
            this._removePool = new List<MovingPlatform>();
            Texture2D tex = AssetPool.GetTexture(platformTexture);
            //TODO better solutiion
            _spritePlatform = new Sprite(tex, PLATFORM_SRC, SCALE);

            this._random = new Random();

            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;

            int yPos = worldHeight;
            //bc of stupid screen coords - is above screen, pos below
            while(yPos > DIST_SPAWN_ABOVE)
            {
                foreach (MovingPlatform platform in _platforms)
                {
                    platform.Position.Y += _distanceTillNextSpawnPlatform;
                }
                this.SpawnPlatform();
                yPos -= _distanceTillNextSpawnPlatform;

            }


        }

        public void SpawnPlatform()
        {
            //reset next spawn distance
            
            this._distanceTillNextSpawnPlatform = _random.Next(MIN_HEIGHT_PLATFORM_APART, MAX_HEIGHT_PLATFORM_APART);

            //spawn platform at DIST_SPAWN_ABOVE
            int platformWidth = _random.Next(MIN_PLATFORM_WIDTH, MAX_PLATFORM_WIDTH);
            int xPos = _random.Next(0, worldWidth -platformWidth);

            Rectangle posHW = new Rectangle(xPos, DIST_SPAWN_ABOVE, platformWidth, PLATFORM_HEIGHT);

            MovingPlatform platform = new MovingPlatform(_spritePlatform, posHW);
            this._collisionSystem.RegisterObject(platform);
            this._platforms.Add(platform);
            
        }


        public void Update(float dt)
        {
            int moveDist =  (int)(dt * _platformSpeedY);

            this._distanceTillNextSpawnPlatform -= moveDist;

            if(this._distanceTillNextSpawnPlatform <= 0)
            {
                this.SpawnPlatform();
            }

            foreach (MovingPlatform platform in this._platforms)
            {
                //go down for each platform
                platform.Position.Y +=moveDist;

                platform.Update(dt);

                if(platform.GetTimeAlive() > TIME_TO_LIVE)
                {
                    this._removePool.Add(platform);
                }
            }


            
            foreach (MovingPlatform platform in this._removePool)
            {
                this._platforms.Remove(platform);
            }
            this._removePool.Clear();


        }

        public void Draw(SpriteBatch spriteBatch)
        {
           

            foreach (MovingPlatform platform in this._platforms)
            {
                platform.Draw(spriteBatch);
            }

            //this._lava.Draw(spriteBatch);
        }

    }

}
