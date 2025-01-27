using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Sprint0
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private ArrayList _controllerList;
        private ArrayList _characterList;

        private SpriteFont _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            Texture2D luigiSheet = this.Content.Load<Texture2D>("luigi");
            _font = this.Content.Load<SpriteFont>("Credits");

            //  Character List
            _characterList = new ArrayList();
            CharacterLuigi luigi = new CharacterLuigi(luigiSheet);
            _characterList.Add(luigi);


            //  Controller List
            _controllerList = new ArrayList();
            _controllerList.Add(new KeyboardController(this, luigi));
            _controllerList.Add(new MouseController(this, luigi));




        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
            foreach (IController controller in _controllerList)
            {
                controller.Update();
            }
            foreach (CharacterLuigi character in _characterList)
            {
                character.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            Texture2D luigi = this.Content.Load<Texture2D>("luigi");
            _spriteBatch.Begin();

            foreach (CharacterLuigi character in _characterList)
            {
                character.Draw(_spriteBatch);
            }
            _spriteBatch.DrawString(_font, "Credits:\nProgram Made By: Evan Snyder\nURL:https://www.mariouniverse.com/wp-content/img/sprites/nes/smb/luigi.png", new Vector2(100, 100), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
