using KirbStomp;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KirbStomp {
    public class SceneManager {
        private IScene _currentScene;
        private static SceneManager inst;

        private BattleScene _battleScene;
        private TestScene _testScene;

        public static SceneManager Get()
        {
            if(inst == null)
            {
                inst = new SceneManager();
            }
            return inst;
        }

		private SceneManager()
		{
            //Setting Default Scene
            _battleScene = new BattleScene();
            _testScene = new TestScene();
            _battleScene.Initialize();
            _testScene.Initialize();
			_currentScene = _battleScene;
            
		}
        
        public void SwitchScene(string sceneName)
        {
            switch (sceneName)
            {
                case "BattleScene":
                    _currentScene = _battleScene;
                    break;
                case "TestScene":
                    _currentScene = _testScene;
                    break;
                default:
                    break;
            }
        }

        public void UpdateScene(GameTime gameTime)
        {
            _currentScene.Update(gameTime);
        }

        public void DrawScene(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScene.Draw(gameTime, spriteBatch);
        }

        internal IScene GetCurrentScene()
        {
            return _currentScene;
        }

        public void ResetCurrentScene()
        {
            _currentScene.ResetScene();
        }
    }
}