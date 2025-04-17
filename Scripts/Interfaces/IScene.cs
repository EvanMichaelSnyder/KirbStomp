using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Scripts.Scenes;


namespace KirbStomp.Interfaces
{
    public interface IScene
    {
        public void Initialize();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
        public void LoadContent();
        public ProjectileManager GetProjectileManager();
        public void ResetScene();
        public class OnGameEndEventArgs : EventArgs {
            public Character character;
        }
        public event EventHandler<OnGameEndEventArgs> OnGameEnd;
    }
}
