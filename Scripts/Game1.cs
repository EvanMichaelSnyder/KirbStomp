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
        private DocumentationGenerator _docGen;
        private IScene _currentScene;

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
            _currentScene = new BattleScene();
		}

        protected override void Initialize()
        {
            //graphics
            _screenWindow.UpdateWindowSize();
            
            //MD Doc generation
            _docGen = new DocumentationGenerator("MDDocs");
            _docGen.GenerateAllDocumentation();

            _currentScene.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentScene.LoadContent();
        }

		protected override void Update(GameTime gameTime)
		{
            _currentScene.Update(gameTime);
			base.Update(gameTime);
		}
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _currentScene.Draw(gameTime, _spriteBatch);

			base.Draw(gameTime);
		}
        public ScreenWindow GetScreenWindow() { return _screenWindow; }
        internal IScene GetCurrentScene() { return _currentScene; }
	}
}
