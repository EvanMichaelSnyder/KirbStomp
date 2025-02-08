using KirbStomp.Engine.ecs;
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
	private Scene scene;

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

	private Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		// TODO: InitializeAll();
		inputs = GlobalInputs.GetInstance();
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		this.scene = SceneLoader.LoadScene("doesnt matter rn");
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
        this.scene.Update((float)(gameTime.ElapsedGameTime.TotalSeconds));
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

