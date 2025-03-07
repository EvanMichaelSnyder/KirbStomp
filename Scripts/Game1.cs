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

        private SceneManager _sceneManager;

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

            _sceneManager = SceneManager.Get();
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
            _sceneManager.UpdateScene(gameTime);


            // TODO MOVE THIS INTO SCENE MANAGER
            if(Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                _sceneManager.SwitchScene("TestScene");
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                _sceneManager.SwitchScene("BattleScene");
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                _sceneManager.ResetCurrentScene();
            }



			base.Update(gameTime);
		}
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _sceneManager.DrawScene(gameTime, _spriteBatch);
			base.Draw(gameTime);
		}
        public ScreenWindow GetScreenWindow() { return _screenWindow; }
	}
}
