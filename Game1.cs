using KirbStomp.Engine.ECSV2Test;
using KirbStomp.Engine.Inputs;
using KirbStomp.Engine.Inputs.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace KirbStomp;

public class Game1 : Game
{
	public GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	private GlobalInputs inputs;
	//singleton
	private static Game1 inst;
	//test stuff

	private ECSV2Scene sceneV2;

	private ECSV2Scene sceneV2;

	//return singleton of game1
	public static Game1 get()
	{
		if(inst == null)
		{
			inst = new Game1();
		}
		return inst;
	}
	private SpriteFont _font;
	public static double globalXBoundMax = 800;
	public static double globalYBoundMax = 480;
	public static double globalScaleX = 1.0;
	public static double globalScaleY = 1.0;
	public static double globalAspectRatio = 5 / 3.0;
	private Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
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
		// TODO: InitializeAll();
		inputs = GlobalInputs.GetInstance();

		var (width, height) = GetAdjustedWindowSize();
		//width = 1600;
		//height = 900;
		globalScaleX = width / globalXBoundMax;
		globalScaleY = height / globalYBoundMax;
		_graphics.PreferredBackBufferWidth = width;
		_graphics.PreferredBackBufferHeight = height;
		//Custom Graphics Settings
		_graphics.ApplyChanges();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		this.sceneV2 = new ECSV2Scene(this);
		sceneV2.LoadAll(Content);
	}

	protected override void Update(GameTime gameTime)
	{
		float dT = (gameTime.ElapsedGameTime.Milliseconds) / 1000.0f;
		inputs.UpdateAllControllers();
		// THIS CAN BE MOVED INTO A CLASS
		if (inputs.IsInputJustPressed(Keys.Escape) || inputs.IsInputJustReleased(MouseButtons.RightMouseButton))
			Exit();

		// TODO: Add your update logic here

		sceneV2.UpdateAll(dT);
		// UpdateAll, including Mouse & Keyboard inputs, Sprite state, Aniamtion state, 
		base.Update(gameTime);

		// TODO QUADS LABELING (OPTIONAL), TEXT SPRITE CREDITS (TODO), 
		// OPTIONALLY, use Sprite2, design version 2 for all sprites and animations using data driven programming
	}
	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();

        //TODO , current is bad implentation, update shouldnt be in draw ...update scene***********************%
		this.sceneV2.DrawAll(_spriteBatch);
        _spriteBatch.End();
		base.Draw(gameTime);
	}

	public SpriteBatch GetSpriteBatch() { return _spriteBatch; }

	//TODO make GlobalInputs public
	internal GlobalInputs GetGlobalInputs()
	{
		return this.inputs;
	}
}

