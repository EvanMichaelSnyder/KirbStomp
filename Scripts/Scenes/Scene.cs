using KirbStomp.Interfaces;
using KirbStomp.Scripts.Classes.GameObjects.Projectiles;
using KirbStomp.Scripts.Classes.Managers;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Classes.Projectiles;
using KirbStomp.Scripts.Classes.Sound;
using KirbStomp.Scripts.Interfaces;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Scripts.Scenes
{
    enum XMLLoadableDataTypes
    {
        Animations, Hitboxes
    }
    
    internal class Scene : IScene
    {
        private string _name;
        private List<string> _characterSpriteSheetName;
        private List<string> _animationXMLFileNames;
        private List<string> _hitboxXMLFileNames;
        private List<string> _characterXMLFiles;
        

        private List<ICharacter> _characters;
        private List<IController> _controllers;
        private List<Platform> _platforms;
        private List<StageBoundary> _boundaries;
        private List<IUI> _screenSpaceUI;   // Sprite'll be UI / Text
        private List<IUI> _worldSpaceSprites;    // Sprite'll be UI / Text
        private List<AreaUI2D> _areaUI2Ds;
        private CollisionSystem _collisionSystem;
        private ProjectileManager _projectileManager;
        private ItemManager _itemManager;
        private ILevelManager _levelManager;
        private MusicManager _musicManager;
        private Sprite _background;

        private Camera2D _camera;
        private List<Vector2> _characterPositions;

        public event EventHandler<IScene.OnGameEndEventArgs> OnGameEnd;

        private bool _useSpecialSceneManager;   // Used for customizable behavior. Currently allow
        public Scene(string name, bool useSpecialSceneManager)
        {
            this._name = name;
            this._characters = new();
            this._controllers = new();
            this._platforms = new();
            this._screenSpaceUI = new();
            this._worldSpaceSprites = new();
            this._boundaries = new();
            this._characterPositions = new();
            this._useSpecialSceneManager = useSpecialSceneManager;
            this._levelManager = null;
        }
        public ProjectileManager GetProjectileManager()
        {
            return _projectileManager;
        }
        
        public void Initialize()
        {
            _areaUI2Ds = new List<AreaUI2D>();
            var (width, height) = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
            this._camera = new(Game1.Get().GraphicsDevice, new Point(width, height));
            _collisionSystem = new CollisionSystem();
            _projectileManager = new ProjectileManager(_collisionSystem);
            _itemManager = new ItemManager(_collisionSystem);
            _musicManager = MusicManager.Get();
            //todo load string
            LoadContent();
            //_levelManager = new LevelManager(_collisionSystem, 60, "Platforms", Game1.Get().GetScreenWindow().GetXSize(), Game1.Get().GetScreenWindow().GetYSize());

        }


        
        public void LoadContent()   // Might be a scene manager thing
        {
            // Load Scene Data
                // Get characters files
                // Get platform files
                // Get UI files
                // Get projectile files
                // Get item files


            // Load Character, it should initialize a character and its controller
            // Add that character and controller to the list
            // Add to collideable objects list
            // var (character, Controller) = LoadCharacter(characterXMLFile);
            SceneLoader.SetLoadFile(_name);    // This will be taken out into scene manager, which'll take care of scene initializations
            SceneLoader.LoadScene(this, _characters, _controllers, _platforms, _screenSpaceUI, _boundaries);

            _levelManager = SceneLoader.GetSceneLevelManager(_collisionSystem);

            foreach (ICharacter character in _characters)
            {
                _collisionSystem.RegisterObject((Character)character);
                //register platforms for moving platform layer
                Character c = (Character)character;
                List<Rectangle> hitboxes = c._bodyCarrier.HitboxManager.getRectangles();
                int h = 0;
                foreach (Rectangle hitbox in hitboxes)
                {
                    h += hitbox.Height;
                }
                this._levelManager.SpawnPlayerPlatform((int)c.Position.X, (int)c.Position.Y, h);
            }

            //_levelManager = _useSpecialSceneManager ? new LevelManager(_collisionSystem, 100, "Platforms", Game1.Get().GetScreenWindow().GetXSize(), Game1.Get().GetScreenWindow().GetYSize()) : new DefaultLevelManager();

            _musicManager.LoadMusic();
            _musicManager.PlayMusic();



            foreach (ICharacter character in _characters)
            {
                _collisionSystem.RegisterObject((Character)character);
                
                character.OnDeath += Character_OnDeath;    
            }
            foreach (Platform platform in _platforms)
            {
                _collisionSystem.RegisterObject(platform);
            }
            foreach (StageBoundary boundary in _boundaries)
            {
                _collisionSystem.RegisterObject(boundary);
            }
            foreach (IUI ui in _screenSpaceUI)
            {
                if (ui is ButtonUI button)
                {
                    _areaUI2Ds.Add(button.Area);
                }
            }
            
            // _background = new Sprite(Game1.Get().Content.Load<Texture2D>("SpaceBackground"), new Rectangle(0, 0, 3000, 2000), 0.27f);// To be taken out later

            // Load all platforms
                // Add the platform to the list
                // Add to collideable object list

            // Load UI
                // Add UI to lists


            // Load necessary stuff for projectils
            // Load necessary stuff for items
        }

        public void Character_OnDeath(object sender, EventArgs e) {
            if (sender is Character character)
            {
                // Remove character from the list
                _characters.Remove(character);
            }
            if(_characters.Count == 1)
            {
                OnGameEnd?.Invoke(this, new IScene.OnGameEndEventArgs { character = (Character)_characters[0]});
            }
        }
        public void Update(GameTime gameTime)
        {
            UpdateAccessableDatas();
            this._projectileManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            this._itemManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            this._levelManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            this._camera.Update(gameTime, _characterPositions);
            this._musicManager.PlayMusic();



            foreach (ICharacter chara in _characters) { chara.Animate(gameTime); }

            //if animate ends the current frame the event endOfState was added
            foreach (ICharacter chara in _characters) { chara.UpdateState(); }


            //new keyboard inputs are taken
            foreach (IController controller in _controllers) { controller.Update(); }

            //action list includes new events
            foreach (ICharacter chara in _characters) { chara.ProcessButtons(); }

            //State is actually changed
            foreach (ICharacter chara in _characters) { chara.UpdateState(); }
            foreach (ICharacter chara in _characters) { chara.ApplyMovementBehavior(); }
            foreach (ICharacter chara in _characters) { chara.Gravity(gameTime); }
            foreach (ICharacter chara in _characters) { chara.MoveCharacter(gameTime); }


            //State is actually changed
            //right now this is actually called under process buttons
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Platform, gameTime);
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Boundary, gameTime);
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Platform, gameTime);
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Body, gameTime);
            foreach (ICharacter chara in _characters) { chara.UpdateState(); }

            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Attack, gameTime);
            



            //mario.UpdateState(); //State is actually changed

            foreach (ICharacter chara in _characters) { chara.UpdateState(); }
            //mario.doSpecialBehaviors

            // is every action commented out above
            foreach (ICharacter chara in _characters) { chara.DoBehavior(); }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            // Draw all objects in world space
            spriteBatch.Begin(transformMatrix: _camera.GetTranslationMatrix());
             //_background.Draw(spriteBatch, new Vector2());

            // Draw projectiles, Items, Characters, and Platforms
            this._levelManager.Draw(spriteBatch);
            this._projectileManager.Draw(spriteBatch);
            this._itemManager.Draw(spriteBatch);
            



            foreach (Platform platform in _platforms)
            {
                platform.Draw(spriteBatch);
                //platform.DrawHitbox(spriteBatch);
            }
            foreach (StageBoundary boundary in _boundaries)
            {
                boundary.Draw(spriteBatch);
                //platform.DrawHitbox(spriteBatch);
            }
            foreach (ICharacter character in _characters)
            {
                character.Draw(spriteBatch);
                //character.DrawHitbox(spriteBatch);
            }

            foreach (IUI sprite in _worldSpaceSprites)
            {
            }
            spriteBatch.End();

            // Draw all objects in screen space
            spriteBatch.Begin();
            // Draw UI
            foreach (IUI sprite in _screenSpaceUI)
            {
                sprite.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        
        public void ResetScene()    // This may be a scene manager thing
        {
            this._characters.Clear();
            this._platforms.Clear();
            this._screenSpaceUI.Clear();
            this._worldSpaceSprites.Clear();
            this._boundaries.Clear();
            this._characterPositions.Clear();
            Initialize();
        }
        public string GetName()
        {
            return _name;
        }
        public void UpdateAccessableDatas()
        {
            _characterPositions.Clear();
            Point boxPosition;
            foreach(ICharacter chara in _characters)
            {
                _characterPositions.Add(chara.GetPointPosition().ToVector2());
            }
        }

        public List<AreaUI2D> GetAreas()
        {
            return _areaUI2Ds;
        }
        public void AddScreenIUI(IUI ui)
        {
            this._screenSpaceUI.Add(ui);
        } 
        public void ClearScreenIUI()
        {
            this._screenSpaceUI.Clear();
        }
        public void RemoveScreenIUI(string uiName)
        {
            foreach (IUI ui in this._screenSpaceUI)
            {
                if (ui.Name == uiName)
                {
                    this._screenSpaceUI.Remove(ui);
                    break;
                }
            }
        }        
    }
}
