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
using KirbStomp.Scripts.Scenes;

namespace KirbStomp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ScreenWindow _screenWindow;

        private SceneManager _sceneManager;
        private MouseController _mouseController;
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
		}

        protected override void Initialize()
        {
            //graphics
            this._screenWindow = new ScreenWindow(_graphics);
            _screenWindow.UpdateWindowSize();
            _screenWindow.UpdateWindowSize();

            _sceneManager = SceneManager.Get();
            _mouseController = new MouseController();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

		protected override void Update(GameTime gameTime)
		{
            _sceneManager.UpdateScene(gameTime);
            _mouseController.Update();

            // if(Keyboard.GetState().IsKeyDown(Keys.D1))
            // {
            //    // _sceneManager.SwitchScene("TestScene");
            //     _sceneManager.UpdateSceneByCall(SceneCalls.Previous);
            // }
            // else if(Keyboard.GetState().IsKeyDown(Keys.D2))
            // {
            //     //_sceneManager.SwitchScene("BattleScene");
            //     _sceneManager.UpdateSceneByCall(SceneCalls.Next);
            // } 
            // else if(Keyboard.GetState().IsKeyDown(Keys.D4))
            // {
            //     //_sceneManager.SwitchScene("GeneralSceneTemplate");
            //     _sceneManager.UpdateSceneByCall(SceneCalls.Pause);
            // }
            // else if(Keyboard.GetState().IsKeyDown(Keys.D3))
            // {
            //     //_sceneManager.ResetCurrentScene();
            //     _sceneManager.UpdateSceneByCall(SceneCalls.Reset);
            // }

            if(Keyboard.GetState().IsKeyDown(Keys.D1))
            {
               _sceneManager.SwitchScene("StartScreen");
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                //_sceneManager.SwitchScene("BattleScene");
                _sceneManager.SwitchScene("GeneralSceneTemplate");
            } 
            else if(Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Scene currScene = (Scene)_sceneManager.GetCurrentScene();
                //_sceneManager.SwitchScene("GeneralSceneTemplate");
                // _sceneManager.UpdateSceneByCall(SceneCalls.Pause);
                if(currScene.GetName() == "GeneralSceneTemplate")
                {
                    _sceneManager.UpdateSceneByCall(SceneCalls.Pause);
                    _sceneManager.SwitchScene("PauseScreen");
                }
               
                
                // Console.WriteLine("Pause");
                // _sceneManager.UpdateSceneByCall(SceneCalls.Pause);
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                //_sceneManager.ResetCurrentScene();
                _sceneManager.SwitchScene("EndScreen");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D8))
            {
                _sceneManager.SwitchScene("PauseScreen");
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5)) {
                _sceneManager.UpdateSceneByCall(SceneCalls.Reset);
            } else if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {

                _sceneManager.SwitchScene("MovingPlatformSceneTemplate");
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
