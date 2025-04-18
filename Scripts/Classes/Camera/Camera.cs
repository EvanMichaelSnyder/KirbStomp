using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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

		private int allowedDistanceFromPlatform = 2500;
		private int allowedHeightFromPlatform = 1900;

		private float maxZoomOut = 0.7f;
		private float minZoomIn = 2.0f;

		private Vector2 minCameraWindowSize = new(160, 90);

		private float aspectRatio;

		private Rectangle initialLargestRectangle;
		private Vector2 initialPos = new();

		private float yOffsetForCamera;
		private Rectangle maxCameraWindow;

		public Camera2D(GraphicsDevice graphicsDevice, Point windowSize)
		{
			position.X = 0;
			position.Y = 0;
			this.cameraWindow = new Vector2(Game1.Get().GetScreenWindow().GetXSize(), Game1.Get().GetScreenWindow().GetYSize());
			this.windowSize = cameraWindow.ToPoint();	// 0.5f is the correction constant, likely a mistake in the window system which causes the need for it
			this.graphicsDevice = graphicsDevice;
			this.currentWindowScale = 1.0f;
			this.translation = Matrix.Identity;
			initialLargestRectangle = new();
			aspectRatio = windowSize.X / (float)windowSize.Y;
			yOffsetForCamera = windowSize.Y * (float)Game1.Get().GetScreenWindow().globalScaleY * 0.025f;
			maxCameraWindow = new();
		}


		// These Key press stuff is tempoary
		public void Update(GameTime gameTime, List<Vector2> positions = null)
		{
			
			BasicCameraMovement(gameTime);  
			if (Keyboard.GetState().IsKeyDown(Keys.M)) // Reset
            {
				position = new();
            }
			UpdateCamera(positions);
		}

		// Actual window is around 0 - 750
		// TODO Make camera clamped
		public void UpdateCamera(List<Vector2> positions)
		{

			// Initial window is windowSize

			// If positions are within playable area, do nothing
			// playable area will be hard coded to 50% of window size

			if (initialLargestRectangle.Equals(new()))
			{
				initialLargestRectangle = GetWindowOfPlayers(positions, minCameraWindowSize);
				initialPos = initialLargestRectangle.Center.ToVector2();
				maxCameraWindow = new Rectangle((initialPos - new Vector2(500.0f)).ToPoint(), (new Vector2() * 2 * 500).ToPoint());
			}

			// otherwise, adjust window zoom or position
			Rectangle largest = GetLargestWindow(positions);

			Point avgPosition = largest.Center;
			// if avgPosition is within cameraWindow, with top left being position, bottom right being position + cameraWindow, 
			// Don't move the camera
			// Otherwise have an offset
			Rectangle currentWindow = new Rectangle((position).ToPoint(), windowSize);
			currentWindow.Y -= (int)yOffsetForCamera;
			
			float windowScale = (float)Game1.Get().GetScreenWindow().globalScaleX;
			Vector2 avgOffset = (-1 * avgPosition.ToVector2()) * windowScale;
			avgOffset = new();


			if (currentWindow.Contains(largest))
			{
				avgOffset = new();
			} else
			 {
				// This means the camera should move
				
				// If rectangle is to the left of the box, move left by that amount
				float left = largest.Left - currentWindow.Left;
				float right = -1 *  ( largest.Right - currentWindow.Right);
				float top = largest.Top - currentWindow.Top;
				float bottom = -1 *  ( largest.Bottom- currentWindow.Bottom);


				if(left < 0 || right < 0)
					avgOffset.X -= left < 0 ? left: -1 * right;
				if(top < 0 || bottom < 0)
					avgOffset.Y -= top < 0 ? top: -1 * bottom;


			}


			avgOffset.Y += yOffsetForCamera;
			AdjustCamera(0.8f, avgOffset.ToPoint());			

		}
		
		public void SetInitialWindow(Rectangle initialWindow)
		{
			this.initialLargestRectangle = initialWindow;
		}
		

		public void AdjustCamera(float scale, Point translation)
		{
			Matrix scaleMat = Matrix.Identity;
			var (x, y) = Game1.Get().GetScreenWindow().GetAdjustedWindowSize();
			Vector2 window = new(x, y);
			offset = window * (1 - 1/ scale);
			scaleMat *= Matrix.CreateTranslation(new Vector3(-0.5f * (offset.X), -0.5f * (offset.Y), 0.0f));
			scaleMat *= Matrix.CreateScale(scale);

			Matrix transformMat = Matrix.CreateTranslation(translation.X, translation.Y, 0) * 1 / scale;

			this.translation = transformMat * scaleMat;
		}


		public Rectangle GetWindowOfPlayers(List<Vector2> positions, Vector2 minWindowSize)
		{
			// Get Positions (0, 0) is top left
			Rectangle largestRectangle = GetLargestWindow(positions);
			Vector2 rectangleSize = new Vector2(float.Max(minWindowSize.X, largestRectangle.Width), float.Max(minWindowSize.Y, largestRectangle.Height));
			Vector2 centerPosition = largestRectangle.Center.ToVector2();
			// Whether to scale the x or y
			// Use the one that gives the larger value
			float adjustedWidth = rectangleSize.Y * aspectRatio;

			// Use the larger with to make sure everything fits in the window
			float actualWindowWidth = float.Max(rectangleSize.X, adjustedWidth);

			Vector2 actualWindowSize = new Vector2(actualWindowWidth, actualWindowWidth / aspectRatio);

			return new Rectangle((centerPosition - (actualWindowSize / 2.0f)).ToPoint(), actualWindowSize.ToPoint());
		}

		public Rectangle GetLargestWindow(List<Vector2> positions)	
		{
			// Get Positions (0, 0) is top left
			int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
			Rectangle output = new Rectangle(0, 0, 0, 0);
			
			foreach (Vector2 pos in positions)
			{
				minX = int.Min((int)pos.X, minX);
				minY = int.Min((int)pos.Y, minY);
				maxX = int.Max((int)pos.X, maxX);
				maxY = int.Max((int)pos.Y, maxY);
			}
			if(positions.Count != 0)
			{
				output = new(minX, minY, maxX - minX, maxY - minY);
			}
			// Position should be in the middle
			if(output.IsEmpty)
			{
				return new(0, 0, 160, 90);
			}
			return output;
			}
		public Matrix GetTranslationMatrix()
		{
			return this.translation;
		}

		private void BasicCameraMovement(GameTime gameTime)
		{

		}
	}
}
