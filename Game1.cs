
using KirbStomp.Inputs;
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

	public Game1()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		// TODO: InitializeAll();
		inputs = new GlobalInputs();
		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime)
	{
		inputs.UpdateAllControllers();
		// THIS CAN BE MOVED INTO A CLASS
		if (inputs.IsInputJustPressed(Keys.Escape) || inputs.IsInputJustReleased(Inputs.Controllers.MouseButtons.RightMouseButton))
			Exit();

		// TODO: Add your update logic here
		// UpdateAll, including Mouse & Keyboard inputs, Sprite state, Aniamtion state, 
		base.Update(gameTime);




		// TODO QUADS LABELING (OPTIONAL), TEXT SPRITE CREDITS (TODO), 
		// OPTIONALLY, use Sprite2, design version 2 for all sprites and animations using data driven programming

	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();

		_spriteBatch.End();
		base.Draw(gameTime);
	}
}
