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
        private ArrayList _characterList;
        private ArrayList _controllerList;
       
		//fps stuff
		int _numFrames = 0; //just for debugging
		private float _fps;
		private int _framesRendered;
		private DateTime _lastTime;
        //animation stuff
        private ProjectileManager _projectileManager;

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
			_numFrames = 0;

            this._projectileManager = new ProjectileManager();
            this._screenWindow = new ScreenWindow(_graphics);
		}

        protected override void Initialize()
        {
            //graphics
            _screenWindow.UpdateWindowSize();

            //much of this should be moved to load content 
            string marioSpriteSheetName = "MarioTransparentSpriteSheet";
            string linkSpriteSheetName = "LinkTransparentSpriteSheet";
            Texture2D marioSheet = Content.Load<Texture2D>(marioSpriteSheetName);
            Texture2D linkSheet = Content.Load<Texture2D>(linkSpriteSheetName);
            //Texture2D marioHitSheet = Content.Load<Texture2D>("MarioHitBoxSpriteSheet");
            Texture2D boxSheet = Content.Load<Texture2D>("HitboxWire");

            //projectile stuff***
            Texture2D marioFireBallSheet = Content.Load<Texture2D>("MarioProjectileTransparentSpriteSheet");
            AssetPool.LoadTexture(marioSheet, "MarioProjectile");
            AssetPool.LoadAnimationsFromXML(GetRelativeFilePathProjectile("MarioProjectile.XML"));


            ICharacter mario = new Character("Mario", marioSheet, marioSpriteSheetName);
            IController controllerMario = new KeyboardController(mario.GetButtonDataManager, new Dictionary<Keys, ICommand>()
            {
                {Keys.W, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Up])},
                {Keys.A, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Left])},
                {Keys.S, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Down])},
                {Keys.D, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Right])},
                {Keys.Y, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Attack])},
                {Keys.T, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Special])},
                {Keys.Space, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.Jump])},
                {Keys.V, new UpdateButtonCommand(mario.GetButtonDataManager.ButtonDataSheet[GameButtons.GotHit])}
            });
            ICharacter mario2 = new Character("Mario", marioSheet, marioSpriteSheetName);
            IController controllerMario2 = new KeyboardController(mario2.GetButtonDataManager, new Dictionary<Keys, ICommand>()
            {
                {Keys.P, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Up])},
                {Keys.L, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Left])},
                {Keys.OemSemicolon, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Down])},
                {Keys.OemQuotes, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Right])},
                {Keys.Down, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Attack])},
                {Keys.Left, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Special])},
                {Keys.RightShift, new UpdateButtonCommand(mario2.GetButtonDataManager.ButtonDataSheet[GameButtons.Jump])}
            });
            //ICharacter link = new Character(linkSheet, LinkSpriteSheetName);
            //IController controllerLink = new KeyboardController(link.GetButtonDataManager);

            mario.AssignLegitimateHitboxSheet(boxSheet);

			_characterList = new ArrayList();
			_characterList.Add(mario);
			_characterList.Add(mario2);
			_characterList.Add(new Character("Link", linkSheet, linkSpriteSheetName));

			_controllerList = new ArrayList();
			_controllerList.Add(controllerMario);
			_controllerList.Add(controllerMario2);

			CharacterXMLParser.LoadCharacter("Mario");

            base.Initialize();
            
        }
		
		private string GetRelativeFilePath(string file)
		{
			return Path.Combine(XMLData.GetDataFolder(), "CharacterData", file);
		}

        private string GetRelativeFilePathProjectile(string file)
        {
            return Path.Combine(XMLData.GetDataFolder(), "Projectiles", file);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            string xmlPathLink = GetRelativeFilePath("Link.XML");
            string xmlPathMario = GetRelativeFilePath("Mario.XML");
            AnimationRepository.LoadAnimationsFromXml(xmlPathMario);
            AnimationRepository.LoadAnimationsFromXml(xmlPathLink);

            string xmlPathMarioHitbox = GetRelativeFilePath("MarioHitbox.XML");
            HitboxRepository.LoadHitboxesFromXml(xmlPathMarioHitbox);


        }

		protected override void Update(GameTime gameTime)
		{
            /*
			order of events

            update key registers
            turn key registers into stateChangingEvents
            doSCE
            alter movement based off of state behavior
            do movement
            check ground collision
            doSCE
            check hit collision
            doSCE
            do Behavior special (spawn fireball)
            Draw
            current frame increment and if endOfState add it to the events
            do ECS
            */

            // DebugFPS();
            //TODO REMOVE TEST
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                this._projectileManager.AddProjectile(new MarioFireBall(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
            }

            this._projectileManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            //THESE FIRST TWO USED TO BE AT THE BOTTOM HOPE THIS DOESNT CAUSE ANY ISSUES
            foreach (ICharacter chara in _characterList) { chara.Animate(gameTime); }

            //if animate ends the current frame the event endOfState was added
            foreach (ICharacter chara in _characterList) { chara.UpdateState(); }


			//new keyboard inputs are taken
			foreach (IController controller in _controllerList) { controller.Update();} 

			//action list includes new events
			foreach (ICharacter chara in _characterList) { chara.ProcessButtons(); } 

			//State is actually changed
			foreach (ICharacter chara in _characterList) { chara.UpdateState(); } 

			foreach (ICharacter chara in _characterList) { chara.ApplyMovementBehavior(); }
			foreach (ICharacter chara in _characterList) { chara.Gravity(gameTime); }

			foreach (ICharacter chara in _characterList) { chara.MoveCharacter(gameTime); }
			
			//right now this is actually called under process buttons
			foreach (ICharacter chara in _characterList) { chara.CheckGroundCollision(); } 
			
			//State is actually changed
			foreach (ICharacter chara in _characterList) { chara.UpdateState(); } 
		
			//mario.checkHitCollision
			//mario.UpdateState(); //State is actually changed
			//mario.doSpecialBehaviors

			// is every action commented out above
			foreach (ICharacter chara in _characterList) { chara.DoBehavior(); } 

            //nothing to do with mario, DEBUGGING 
            _numFrames++;

			base.Update(gameTime);
		}

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            this._projectileManager.Draw(_spriteBatch);
            foreach (ICharacter chara in _characterList)
            {
                chara.Draw(_spriteBatch);
                chara.DrawHitbox(_spriteBatch);
            }
            _spriteBatch.End();

			base.Draw(gameTime);
		}

		private void DebugFPS() {
			//fps
			_numFrames++;
			_framesRendered++;
			if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
			{
				// one second has elapsed 
				_fps = _framesRendered;
				_framesRendered = 0;
				_lastTime = DateTime.Now;
			}
			Console.WriteLine(_numFrames + " FPS: " + _fps);
		}

        public ProjectileManager GetProjectileManager() { return _projectileManager; } 
        public ScreenWindow GetScreenWindow() { return _screenWindow; }
        // public GraphicsDeviceManager GetGraphicsDeviceManager() { return _graphics; }
        // public void UpdateWindowSize(int width, int height) {}
	}
}
