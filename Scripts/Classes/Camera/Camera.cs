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
namespace KirbStomp
{
	internal class Camera2D
	{

		private Vector2 position;
		private Vector2 offset;
		private Point windowSize;
		private Vector2 cameraWindow;
		private GraphicsDevice graphicsDevice;
		private float currentWindowScale;

		private Matrix translation;

		public Camera2D(GraphicsDevice graphicsDevice, Point windowSize)
		{
			position.X = 0;
			position.Y = 0;
			this.cameraWindow = windowSize.ToVector2();
			this.windowSize = windowSize;
			this.graphicsDevice = graphicsDevice;
			this.currentWindowScale = 1.0f;
			this.translation = Matrix.Identity;
			pos1 = new();
			pos2 = windowSize.ToVector2();
		}


		// Temp debugging variables
		private Vector2 pos1;
		private Vector2 pos2;
		// These Key press stuff is tempoary
		public void Update(GameTime gameTime)
		{
			/*
			float zoom = 1.0f;
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
				zoom = 1.0f;
			}
			if(Keyboard.GetState().IsKeyDown(Keys.OemPlus)) // Zoom out
			{
				zoom = 2.0f;
			}
			if(Keyboard.GetState().IsKeyDown(Keys.OemMinus)) // Zoom out
			{
				zoom = 0.2f;
			}
			ZoomCamera(zoom);

			 */


            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.H)) // Left
                {
                    pos1.X -= 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.K)) // Right
                {
                    pos1.X += 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.U)) // Up
                {
                    pos1.Y -= 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.J)) // Down
                {
                    pos1.Y += 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
			{
                if (Keyboard.GetState().IsKeyDown(Keys.H)) // Left
                {
                    pos2.X -= 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.K)) // Right
                {
                    pos2.X += 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.U)) // Up
                {
                    pos2.Y -= 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.J)) // Down
                {
                    pos2.Y += 5000 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

			}
            if (Keyboard.GetState().IsKeyDown(Keys.M)) // Reset
            {
                pos1.X = 0.0f;
                pos1.Y = 0.0f;
				pos2 = windowSize.ToVector2();
            }

            List<Vector2> positions = new();
			positions.Add(pos1);
			positions.Add(pos2);

			UpdateCamera(positions);
		}

		public void ZoomCamera(float scale)
		{
			currentWindowScale = scale;
			offset = windowSize.ToVector2() * (1 - 1/ scale);
			translation = Matrix.Identity;
			translation = Matrix.CreateTranslation(new Vector3(-0.5f * (position.X + offset.X), -0.5f * (position.Y + offset.Y), 0.0f));
			translation *= Matrix.CreateScale(currentWindowScale);
			
		}

		public void ZoomCamera2D(float scaleX, float scaleY)
		{
			offset = windowSize.ToVector2();
			offset.X *= (1 - 1 / scaleX);
			offset.Y *= (1 - 1 / scaleY);

			translation = Matrix.Identity;
			translation = Matrix.CreateTranslation(new Vector3(-0.5f * (position.X + offset.X), -0.5f * (position.Y + offset.Y), 0.0f));
			translation *= Matrix.CreateScale(scaleX, scaleY, 1.0f);
		}
		public Vector3 GetPosition()
		{
			return new Vector3(position + offset, 0.0f);
		}


		public void UpdateCamera(List<Vector2> positions)
		{
			var (windowPosition, scale) = GetCameraData(positions, new(160, 90));
			if(scale > 1)
			{
				Debug.WriteLine("aaa");
			}
			ZoomCamera(scale);
			this.position = windowPosition.ToVector2();
		}

		public (Point, float) GetCameraData(List<Vector2> positions, Vector2 minWindowSize)
		{
			float minX = 9999;	// Some large num
			float minY = 9999;
			float maxX = -9999; // Some small num
			float maxY = -9999;

			float aspectRatio = cameraWindow.X / cameraWindow.Y;
			float windowWidth;
			float windowHeight;
			Vector2 windowPos;
			Vector2 centerPos;

			foreach (Vector2 pos in positions)
			{
				minX = minX > pos.X ? pos.X : minX;
				minY = minY > pos.Y ? pos.Y : minY;
				maxX = maxX < pos.X ? pos.X : maxX;
				maxY = maxY < pos.Y ? pos.Y : maxY;
			}
			
			// Get the current rectangle screen
			windowWidth = maxX - minX;
			windowHeight = maxY - minY;

			centerPos = new Vector2(windowWidth / 2.0f, windowHeight / 2.0f);

			if(windowWidth > windowHeight * aspectRatio)
			{
				windowHeight = windowWidth / aspectRatio;
			} else
			{
				windowWidth = windowHeight * aspectRatio;
			}

			windowWidth = windowWidth > windowSize.X ? windowWidth : windowSize.X;
			windowHeight = windowHeight > windowSize.Y ? windowHeight : windowSize.Y;

			windowPos = centerPos - new Vector2(windowWidth / 2.0f, windowHeight / 2.0f);

			

			return (windowPos.ToPoint(), windowSize.X * 0.9f / windowWidth);
		}
		public Matrix GetTranslationMatrix()
		{
			return this.translation;
		}
	}
}
