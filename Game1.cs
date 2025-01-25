using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    public ISprite currentSprite { get; set; }
    private ISprite textSprite;
    private IController keyboardController;
    private IController mouseController; 
    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // new spriteBatch for drawing multiple sprites
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // load the sprite
        Texture2D sleepKirby = Content.Load<Texture2D>("sleepKirby");
        // initialize the sprite frames with hardcoded data (obtained from testings)
        Rectangle[] kirbySpriteFrame = {
            new Rectangle(0, 0, 35, 35),
            new Rectangle(35, 0, 33, 36),
            new Rectangle(68, 0, 30, 38),
            new Rectangle(98, 0, 30, 38),
            new Rectangle(130, 0, 31, 38),
            new Rectangle(161, 0, 32, 39),
            new Rectangle(193, 0, 33, 35),
            new Rectangle(226, 0, 33, 35)
        };

        // create an array of 4 ISprite to be displayed
        ISprite[] spritesList =
        [
            new StaticSprite(sleepKirby, new Vector2(100, 50), kirbySpriteFrame[6], 5),
            new AnimatedStaticSprite(sleepKirby, new Vector2(500, 50), kirbySpriteFrame, 0, 5, 5),
            new MovingSprite(sleepKirby, new Vector2(100, 200), kirbySpriteFrame[7], 5, new Vector2(0, 100), new Vector2(100, 0), new Vector2(100, 250)),
            new AnimatedMovingSprite(sleepKirby, new Vector2(500, 200), kirbySpriteFrame, 0, 5, 5, new Vector2(100, 0), new Vector2(100, 200), new Vector2(600, 200))
        ];


        // load font to be used for text
        SpriteFont font = Content.Load<SpriteFont>("Arial");
        // text to be displayed on screen
        String text = "Credits\nProgram made by: JJ Kiratikosolrak\nKirby Sprite Source URL: https://www.spriters-resource.com/snes/\nkirbysuperstarkirbysfunpak/sheet/2909/";
        // set textSprite
        textSprite = new TextSprite(font, text, new Vector2(50, 350));

        // set the static sprite as the first sprite
        currentSprite = spritesList[0];

        // Intialize all possible commands
        ICommand[] commands =
        [
            new QuitCommand(),
            new DisplayStaticSpriteCommand(this, spritesList[0]),
            new DisplayAnimatedStaticSpriteCommand(this, spritesList[1]),
            new DisplayMovingSpriteCommand(this, spritesList[2]),
            new DisplayAnimatedMovingSpriteCommand(this, spritesList[3])
        ];
        // Initialize mouse controller
        mouseController = new MouseController(commands);

        // Match keys with each command 
        Dictionary<Keys, ICommand> keyCommands = new Dictionary<Keys, ICommand> 
        { 
            { Keys.D0, commands[0] }, 
            { Keys.D1, commands[1] }, 
            { Keys.D2, commands[2] }, 
            { Keys.D3, commands[3] }, 
            { Keys.D4, commands[4] } 
        }; 
        // Initialize keyboard controller 
        keyboardController = new KeyboardController(keyCommands);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // use keyboard and mouse controller to update the sprite
        keyboardController.Update();
        mouseController.Update();

        // Console.WriteLine($"Current Sprite: {currentSprite}");
        
        // update the sprite
        currentSprite.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.LightSkyBlue);
                
        // draw the text sprite
        textSprite.Draw(spriteBatch);

        // draw the sprite
        currentSprite.Draw(spriteBatch);

        base.Draw(gameTime);
    }
}
