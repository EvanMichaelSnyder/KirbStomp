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
        private Mario mario;
        private IController controller;



        int numFrames = 0;//just for debugging

        internal static double globalXBoundMax = 800;
        internal static double globalYBoundMax = 480;
        internal static double globalScaleX = 1.0;
        internal static double globalScaleY = 1.0;
        internal static double globalAspectRatio = 5 / 3.0;

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
            string MarioSpriteSheetName = "MarioTransparentSpriteSheet";
            string LinkSpriteSheetName = "LinkTransparentSpriteSheet";
            Texture2D marioSheet = Content.Load<Texture2D>(MarioSpriteSheetName);
            Texture2D linkSheet = Content.Load<Texture2D>(LinkSpriteSheetName);
            mario = new Mario(marioSheet, MarioSpriteSheetName);
            //rio = new Mario(marioSheet, MarioSpriteSheetName);
            controller = new KeyboardController(mario.GetButtonDataManager);
            //string xmlPath = GetRelativeFilePath("Link.XML");
            string xmlPath = GetRelativeFilePath("Mario.XML");
            AnimationSystem.LoadAnimationsFromXml(xmlPath);
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

            controller.Update(); //new keyboard inputs are taken

            mario.ProcessButtons(); //action list includes new events

            mario.UpdateState(); //State is actually changed

            mario.ApplyMovementBehavior();
            mario.gravity(gameTime);

            mario.MoveCharacter(gameTime);

            mario.checkGroundCollision(); //right now this is actually called under process buttons
            mario.UpdateState(); //State is actually changed
            //mario.checkHitCollision
            //mario.UpdateState(); //State is actually changed
            //mario.doSpecialBehaviors

            mario.doBehavior(); //is every action commented out above

            //mario.draw(_spriteBatch);
            mario.debugState(); //effectively also draw

            numFrames++;//nothing to do with mario

            mario.Animate(gameTime);

            mario.UpdateState(); //if animate ends the current frame the event endOfState was added

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


             mario.draw(_spriteBatch);
            //    _spriteBatch.DrawString(_font, "Lots of Marios!!", new Vector2(400, 850), Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
