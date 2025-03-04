using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Collections.Generic;
using KirbStomp;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KirbStomp.Data;
using KirbStomp.Scripts.Projectiles;

namespace KirbStomp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenWindow _screenWindow;
        
        //singleton
        private static Game1 inst;

        public static Game1 Get()
        {
            if(inst == null)
            {
                inst = new Game1();
            }
            return inst;
        }

		private Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			
			IsMouseVisible = true;
            this._screenWindow = new ScreenWindow(_graphics);
            _screenWindow.UpdateWindowSize();
		}

        protected override void Initialize()
        {
            //graphics
            _screenWindow.UpdateWindowSize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

		protected override void Update(GameTime gameTime)
		{
            SceneManager.Get().UpdateScene(gameTime);

            if(Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                SceneManager.Get().SwitchScene("TestScene");
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                SceneManager.Get().SwitchScene("BattleScene");
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                SceneManager.Get().ResetCurrentScene();
            }
			base.Update(gameTime);
		}
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SceneManager.Get().DrawScene(gameTime, _spriteBatch);
			base.Draw(gameTime);
		}
        public ScreenWindow GetScreenWindow() { return _screenWindow; }
	}
}
