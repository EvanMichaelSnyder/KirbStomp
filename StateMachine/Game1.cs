using System.Collections;
using System.Diagnostics;
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
        Mario mario;
        IController controller;
        int numFrames = 0;
        internal static double globalScaleX = 1.0;
        internal static double globalScaleY = 1.0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            numFrames = 0;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Texture2D marioSheet = Content.Load<Texture2D>("MarioTransparentSpriteSheet");
            mario = new Mario(marioSheet);
            controller = new KeyboardController(mario.GetButtonDataManager);



            base.Initialize();
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

            // TODO: Add your update logic here

            controller.Update();
            mario.ProcessButtons();
            mario.updateState();
            mario.doBehavior();
            mario.debugState();
            numFrames++;
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


             mario.draw(_spriteBatch, gameTime);
            //    _spriteBatch.DrawString(_font, "Lots of Marios!!", new Vector2(400, 850), Color.White);

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
