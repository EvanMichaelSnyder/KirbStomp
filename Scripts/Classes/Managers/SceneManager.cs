using KirbStomp;
using KirbStomp.Data;
using KirbStomp.Interfaces;
using KirbStomp.Scripts.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace KirbStomp
{
    public enum SceneCalls
    { 
        Default, Pause, Previous, Next, Reset, End
    }

    delegate void UpdateMethod(GameTime gameTime);

    public class SceneManager {
        private IScene _currentScene;
        private static SceneManager inst;

        private BattleScene _battleScene;
        private TestScene _testScene;
        private Scene _defaultScene;


        private SceneCalls _sceneStateCall;
        private List<Scene> _scenesList;
        private List<UpdateMethod> _sceneUpdateList;
        private int _sceneIndex;


        private UpdateMethod _updateMethod;
        private bool _paused;
        public event EventHandler OnSceneChange;
        private readonly string _scenesToLoad = Path.Combine(XMLData.GetDataFolder(), "SceneData", "AllScenesToLoad");
        /*
         * This scene manager will deal with pause, quit, and reset
         * Implement methods accordingly
         * Pause: Draw but don't update anything except input polling for quitting pause
         * 
         * 
         */

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
            _sceneStateCall = SceneCalls.Default;
            _scenesList = new();
            _sceneUpdateList = new();



            //Setting Default Scene
            /*
            _battleScene = new BattleScene();
            _testScene = new TestScene();
            _defaultScene = new Scene("test");
            _currentScene = _battleScene;
            */
            Init();     // Tempoarily here
            
		}

        private void Init()

        {
            LoadScenesFromXML();

            foreach(Scene scene in this._scenesList)
            {
                scene.Initialize();
            }
            /*
            _battleScene.Initialize();
            _testScene.Initialize();
            _defaultScene.Initialize();
             */
            _currentScene = _scenesList[_sceneIndex];
            /*
             * Register the hard coded bindings for pause, quit, next, previous, reset
             */
        }

        public void SwitchScene(string sceneName)
        {
            
            foreach(Scene scene in this._scenesList)
            {
                if(scene.GetName() == sceneName)
                {
                    _currentScene = scene;
                    OnSceneChange?.Invoke(this, EventArgs.Empty);
                    break;
                }
            }
            // switch (sceneName)
            // {
            //     case "BattleScene":
            //         _currentScene = _battleScene;
            //         break;
            //     case "TestScene":
            //         _currentScene = _testScene;
            //         break;
            //     case "GeneralSceneTemplate":
            //         _currentScene = _defaultScene;
            //         break;
            //     default:
            //         break;
            // }
             
        }
        

      
        public void UpdateScene(GameTime gameTime)
        {
            _sceneUpdateList[_sceneIndex](gameTime);
        }

        public void DrawScene(GameTime gameTime, SpriteBatch spriteBatch)
        {
           _currentScene.Draw(spriteBatch);
        }

        internal IScene GetCurrentScene()
        {
            return _currentScene;
        }

        public void ResetCurrentScene()
        {
            _currentScene.ResetScene();
        }




        private void LoadScenesFromXML()
        {
            Scene temp;
            foreach(XElement scene in XMLData.GetXMLRootElement("Scenes", this._scenesToLoad).Elements("Scene"))
            {
                temp = new Scene(scene.Value.Replace(" ", string.Empty));
                this._scenesList.Add(temp);
                _sceneUpdateList.Add(temp.Update);
            }
        }

        // Not the most elegant solution, but it was quick
        public void UpdateSceneByCall(SceneCalls call)
        {
            switch (call)
            {
                case SceneCalls.End:
                    Game1.Get().Exit();
                    break;
                case SceneCalls.Next:
                    this._sceneIndex = _sceneIndex == this._scenesList.Count - 1 ? 0 : _sceneIndex + 1;
                    break;
                case SceneCalls.Previous:
                    this._sceneIndex = _sceneIndex == 0 ? this._scenesList.Count - 1 : _sceneIndex - 1;
                    break;
                case SceneCalls.Reset:
                    _scenesList[_sceneIndex].ResetScene();
                    if (_paused) UpdateSceneByCall(SceneCalls.Pause);

                    break;
                case SceneCalls.Pause:
                    _paused = !_paused;
                    if (_paused) _updateMethod = DoNothingUpdate;
                    else _updateMethod = _scenesList[_sceneIndex].Update;

                    _sceneUpdateList[_sceneIndex] = _updateMethod;
                    break;
                default:
                    break;
            }

            _currentScene = _scenesList[_sceneIndex];
        }
        private void DoNothingUpdate(GameTime gameTime)
        {

        }
    }
}