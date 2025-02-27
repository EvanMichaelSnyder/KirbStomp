using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

internal class Camera2D
{

	private Vector2 position;
	private Vector2 offset;
	private Point windowSize;
	private Vector2 cameraWindow;
	private GraphicsDevice graphicsDevice;
	private float currentWindowScale;

	private Matrix translation;
	private Matrix view;

	private Rectangle cam;
	public Camera2D(GraphicsDevice graphicsDevice, Point windowSize)
	{
		position.X = 0;
		position.Y = 0;
		this.cameraWindow = windowSize.ToVector2();
		this.windowSize = windowSize;
		this.graphicsDevice = graphicsDevice;
		this.currentWindowScale = 1.0f;
		this.translation = Matrix.Identity;
		this.view = Matrix.Identity;
		cam = new Rectangle(0, 0, windowSize.X, windowSize.Y);
	}

	public Matrix GetSpriteTransformMatrix()
	{
		Matrix output = Matrix.CreateTranslation(position.X, position.Y, 0.0f);
		
		return default;
	}
	public void Update(GameTime gameTime)
	{
		if(Keyboard.GetState().IsKeyDown(Keys.H)) // Left
		{
			position.X -= 1000 *(float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		if(Keyboard.GetState().IsKeyDown(Keys.K)) // Right
		{
			position.X += 1000 *(float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		if(Keyboard.GetState().IsKeyDown(Keys.U)) // Up
		{
			position.Y -= 1000 *(float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		if(Keyboard.GetState().IsKeyDown(Keys.J)) // Down
		{
			position.Y += 1000 *(float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		if(Keyboard.GetState().IsKeyDown(Keys.M)) // Reset
		{
			position.X = 0.0f;
			position.Y = 0.0f;
			ZoomCamera(1.0f);
		}
		if(Keyboard.GetState().IsKeyDown(Keys.OemPlus)) // Zoom out
		{
			ZoomCamera(2.0f);
		}
		if(Keyboard.GetState().IsKeyDown(Keys.OemMinus)) // Zoom out
		{
			ZoomCamera(0.5f);
		}

	}

	public void ZoomCamera(float scale)
	{
		currentWindowScale = scale;
		offset = windowSize.ToVector2() * (1 - 1/ scale);
		translation = Matrix.Identity;
		translation = Matrix.CreateTranslation(new Vector3(-0.5f * (position.X + offset.X), -0.5f * (position.Y + offset.Y), 0.0f));
		translation *= Matrix.CreateScale(currentWindowScale);
		Debug.WriteLine(position.ToString());
		Debug.WriteLine(offset.ToString());
		
	}
	public Vector3 GetPosition()
	{
		return new Vector3(position + offset, 0.0f);
	}
	public Matrix GetViewMatrix()
	{
		return default;
	}
	public Matrix GetTranslationMatrix()
	{
		return this.translation;
	}


}

