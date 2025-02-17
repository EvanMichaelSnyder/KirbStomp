using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using KirbStomp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KirbStomp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private Mario mario;
        private ArrayList _characterList;
        private ArrayList _controllerList;
        //private IController controller;



        int numFrames = 0;//just for debugging

        internal static double globalXBoundMax = 800;
        internal static double globalYBoundMax = 480;
        internal static double globalScaleX = 1.0;
        internal static double globalScaleY = 1.0;
        internal static double globalAspectRatio = 5 / 3.0;

        //fps stuff
        private float _fps;
        private int _framesRendered;
        private DateTime _lastTime;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            numFrames = 0;

        }

        public static (int width, int height) GetAdjustedWindowSize()
        {
            // Get screen dimensions
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Calculate 5:3 window size
            double maxWidth = screenWidth * 0.80; // 80% of screen width
            double maxHeight = screenHeight * 0.80; // 80% of screen height

            double windowWidth = maxWidth;
            double windowHeight = maxWidth / globalAspectRatio;

            // Return the calculated width and height as integers
            return ((int)windowWidth, (int)windowHeight);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic


            //graphics
            var (width, height) = GetAdjustedWindowSize();
            //width = 1600;
            //height = 900;
            globalScaleX = width / globalXBoundMax;
            globalScaleY = height / globalYBoundMax;
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            //Custom Graphics Settings
            _graphics.ApplyChanges();
            //

            //much of this should be moved to load content 
            string MarioSpriteSheetName = "MarioTransparentSpriteSheet";
            string LinkSpriteSheetName = "LinkTransparentSpriteSheet";
            Texture2D marioSheet = Content.Load<Texture2D>(MarioSpriteSheetName);
            Texture2D linkSheet = Content.Load<Texture2D>(LinkSpriteSheetName);

            ICharacter mario = new Character(marioSheet, MarioSpriteSheetName);
            IController controllerMario = new KeyboardController(mario.GetButtonDataManager);
            ICharacter mario2 = new Character(marioSheet, MarioSpriteSheetName);
            IController controllerMario2 = new KeyboardController(mario2.GetButtonDataManager);
            ICharacter mario3 = new Character(marioSheet, MarioSpriteSheetName);
            IController controllerMario3 = new KeyboardController(mario3.GetButtonDataManager);
            //ICharacter link = new Character(linkSheet, LinkSpriteSheetName);
            //IController controllerLink = new KeyboardController(link.GetButtonDataManager);

            _characterList = new ArrayList();
            _characterList.Add(mario);
            _characterList.Add(mario2);
            _characterList.Add(new Character(linkSheet, LinkSpriteSheetName));

            _controllerList = new ArrayList();
            _controllerList.Add(controllerMario);
            _controllerList.Add(controllerMario2);

            string xmlPathLink = GetRelativeFilePath("Link.XML");
            string xmlPathMario = GetRelativeFilePath("Mario.XML");
            AnimationSystem.LoadAnimationsFromXml(xmlPathMario);
            AnimationSystem.LoadAnimationsFromXml(xmlPathLink);
            base.Initialize();
        }

        private string GetRelativeFilePath(string file, [CallerFilePath] string currentPath = "")
        {
            string dir = Path.GetDirectoryName(currentPath);
            return Path.Combine(dir, file);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            */

            //order of events

            //update key registers
            //turn key registers into stateChangingEvents
            //doSCE
            //alter movement based off of state behavior
            //do movement
            //check ground collision
            //doSCE
            //check hit collision
            //doSCE
            //do Behavior special (spawn fireball)
            //draw
            //current frame increment and if endOfState add it to the events
            //do ECS

            // TODO: Add your update logic here

            //fps
            numFrames++;
            _framesRendered++;
            if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
            {
                // one second has elapsed 

                _fps = _framesRendered;
                _framesRendered = 0;
                _lastTime = DateTime.Now;
            }
            Debug.WriteLine(numFrames + " FPS: " + _fps);



            foreach (IController controller in _controllerList)
            {
                controller.Update();
            } //new keyboard inputs are taken

            foreach (ICharacter chara in _characterList) { chara.ProcessButtons(); } //action list includes new events

            foreach (ICharacter chara in _characterList) { chara.UpdateState(); } //State is actually changed

            foreach (ICharacter chara in _characterList) { chara.ApplyMovementBehavior(); }
            foreach (ICharacter chara in _characterList) { chara.gravity(gameTime); }

            foreach (ICharacter chara in _characterList) { chara.MoveCharacter(gameTime); }

            foreach (ICharacter chara in _characterList) { chara.checkGroundCollision(); } //right now this is actually called under process buttons
            foreach (ICharacter chara in _characterList) { chara.UpdateState(); } //State is actually changed
            //mario.checkHitCollision
            //mario.UpdateState(); //State is actually changed
            //mario.doSpecialBehaviors

            foreach (ICharacter chara in _characterList) { chara.doBehavior(); } //is every action commented out above

            //mario.draw(_spriteBatch);
            //foreach (ICharacter chara in _characterList) { chara.debugState(); } //effectively also draw

            numFrames++;//nothing to do with mario

            foreach (ICharacter chara in _characterList) {chara.Animate(gameTime);}

            foreach (ICharacter chara in _characterList) {chara.UpdateState();} //if animate ends the current frame the event endOfState was added

            //System.Threading.Thread.Sleep(50);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here

            Texture2D marioSheet = Content.Load<Texture2D>("MarioTransparentSpriteSheet");
            //Texture2D luigi = this.Content.Load<Texture2D>("luigi");
            _spriteBatch.Begin();

            foreach (ICharacter chara in _characterList)
            {
                chara.draw(_spriteBatch);
            }
            //    _spriteBatch.DrawString(_font, "Lots of Marios!!", new Vector2(400, 850), Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
