using KirbStomp.Interfaces;
using KirbStomp.Scripts.Classes.GameObjects.Projectiles;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Classes.Projectiles;
using KirbStomp.Scripts.Classes.Sound;
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

        private CollisionSystem _collisionSystem;
        private ProjectileManager _projectileManager;
        private ItemManager _itemManager;
        private MusicManager _musicManager;
        private Sprite _background;

        private Camera2D _camera;

        //Only applicable for the win screen
        private SpriteString _winScreenText;

        public Scene(string name)
        {
            this._name = name;
            this._characters = new();
            this._controllers = new();
            this._platforms = new();
            this._screenSpaceUI = new();
            this._worldSpaceSprites = new();
            this._boundaries = new();


        }
        public ProjectileManager GetProjectileManager()
        {
            return _projectileManager;
        }
        
        public void Initialize()
        {
            var (width, height) = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
            this._camera = new(Game1.Get().GraphicsDevice, new Point(width, height));
            _collisionSystem = new CollisionSystem();
            _projectileManager = new ProjectileManager(_collisionSystem);
            _itemManager = new ItemManager(_collisionSystem);
            _musicManager = MusicManager.Get();
            LoadContent();
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
            SceneLoader.LoadScene(_characters, _controllers, _platforms, _screenSpaceUI, _boundaries);
            _musicManager.LoadMusic();
            _musicManager.PlayMusic();

            foreach (ICharacter character in _characters)
            {
                _collisionSystem.RegisterObject((Character)character);
            }
            foreach (Platform platform in _platforms)
            {
                _collisionSystem.RegisterObject(platform);
            }
            foreach (StageBoundary boundary in _boundaries)
            {
                _collisionSystem.RegisterObject(boundary);
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


        public void Update(GameTime gameTime)
        {

            this._projectileManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            this._itemManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            //this._itemManager.SpawnRandomItem();
            this._camera.Update(gameTime);
            _musicManager.PlayMusic();

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
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Platform);
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Boundary);
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Platform);
            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Item, HitboxTypeEnum.Body);
            foreach (ICharacter chara in _characters) { chara.UpdateState(); }

            _collisionSystem.CheckCollisionPair(HitboxTypeEnum.Body, HitboxTypeEnum.Attack);
            



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
            // _background.Draw(spriteBatch, new());

            // Draw projectiles, Items, Characters, and Platforms
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

            Initialize();
        }
        public string GetName()
        {
            return _name;
        }         
    }
}
