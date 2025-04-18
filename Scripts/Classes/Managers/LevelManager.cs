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
        private int MAX_HEIGHT_PLATFORM_APART = MagicNum.LevelManagerMagic.MAX_HEIGHT_PLATFORM_APART;
        private int MIN_HEIGHT_PLATFORM_APART = MagicNum.LevelManagerMagic.MIN_HEIGHT_PLATFORM_APART;
        private int PLATFORM_HEIGHT = MagicNum.LevelManagerMagic.PLATFORM_HEIGHT;
        private int MIN_PLATFORM_WIDTH = MagicNum.LevelManagerMagic.MIN_PLATFORM_WIDTH;
        private int MAX_PLATFORM_WIDTH = MagicNum.LevelManagerMagic.MAX_PLATFORM_WIDTH;
        private int DIST_SPAWN_ABOVE = MagicNum.LevelManagerMagic.DIST_SPAWN_ABOVE;
        private int TRIGGER_WIDTH_DOUBLE_SPAWN = MagicNum.LevelManagerMagic.TRIGGER_WIDTH_DOUBLE_SPAWN;
        private int PLAYER_PLATFORM_WIDTH = MagicNum.LevelManagerMagic.PLAYER_PLATFORM_WIDTH;

        private float TIME_TO_LIVE = MagicNum.LevelManagerMagic.TIME_TO_LIVE;

        private int _distanceTillNextSpawnPlatform = 0;
        private float _platformSpeedY;
        private Random _random;
        private CollisionSystem _collisionSystem;


        //TODO abstract out sprite
        private Sprite _spritePlatform;
        private Rectangle PLATFORM_SRC = MagicNum.LevelManagerMagic.PLATFORM_SRC;
        private float SCALE = 1;

        private List<MovingPlatform> _platforms;
        private List<MovingPlatform> _removePool;
 
        private int _worldWidth, _worldHeight;

        

        
        public LevelManager(CollisionSystem collisionSystem, float platformSpeedY, String platformTexture, int worldWidth, int worldHeight) {

            
            
            this._collisionSystem = collisionSystem;
            this._platformSpeedY = platformSpeedY;
            this._platforms = new List<MovingPlatform>();
            this._removePool = new List<MovingPlatform>();
            Texture2D tex = AssetPool.GetTexture(platformTexture);
            //TODO better solutiion
            _spritePlatform = new Sprite(tex, PLATFORM_SRC, SCALE);

            this._random = new Random();

            this._worldHeight = worldHeight;
            this._worldWidth = worldWidth;

            int yPos = worldHeight;
            
            /**while(yPos > DIST_SPAWN_ABOVE)
            {
                foreach (MovingPlatform platform in _platforms)
                {
                    platform.Position.Y += _distanceTillNextSpawnPlatform;
                }
               // this.SpawnPlatform();
                yPos -= _distanceTillNextSpawnPlatform;

            }
            **/

        }

        public void SpawnPlatform()
        {
            //reset next spawn distance
            
            this._distanceTillNextSpawnPlatform = _random.Next(MIN_HEIGHT_PLATFORM_APART, MAX_HEIGHT_PLATFORM_APART);

            //spawn platform at DIST_SPAWN_ABOVE
            int platformWidth = _random.Next(MIN_PLATFORM_WIDTH, MAX_PLATFORM_WIDTH);
            if (platformWidth < TRIGGER_WIDTH_DOUBLE_SPAWN)
            {
                int xPos1 = _random.Next(0, _worldWidth/2 - platformWidth);
                int xPos2 = _random.Next(_worldWidth / 2 - platformWidth, _worldWidth - platformWidth);
                Rectangle posHW1 = new Rectangle(xPos1, DIST_SPAWN_ABOVE, platformWidth, PLATFORM_HEIGHT);
                Rectangle posHW2 = new Rectangle(xPos2, DIST_SPAWN_ABOVE, platformWidth, PLATFORM_HEIGHT);
                MovingPlatform platform = new MovingPlatform(_spritePlatform, posHW1);
                this._collisionSystem.RegisterObject(platform);
                this._platforms.Add(platform);
                MovingPlatform platform2 = new MovingPlatform(_spritePlatform, posHW2);
                this._collisionSystem.RegisterObject(platform2);
                this._platforms.Add(platform2);
            }
            else
            {

                int xPos = _random.Next(0, _worldWidth - platformWidth);

                Rectangle posHW = new Rectangle(xPos, DIST_SPAWN_ABOVE, platformWidth, PLATFORM_HEIGHT);

                MovingPlatform platform = new MovingPlatform(_spritePlatform, posHW);
                this._collisionSystem.RegisterObject(platform);
                this._platforms.Add(platform);
            }

            
        }

        public void SpawnPlayerPlatform(int x, int y, int playerHeight)
        {
            Rectangle posHW = new Rectangle(x -PLAYER_PLATFORM_WIDTH/2, y + playerHeight, PLAYER_PLATFORM_WIDTH, PLATFORM_HEIGHT);
          
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
                this._collisionSystem.RemoveObject(platform);
            }
            this._removePool.Clear();


        }

        public void Draw(SpriteBatch spriteBatch)
        {
           

            foreach (MovingPlatform platform in this._platforms)
            {
                platform.Draw(spriteBatch);
            }

            
        }

    }

}
