using ECSV2.Systems.ISystems;
using KirbStomp.ECSV2.Components;
using KirbStomp.ECSV2.ECSEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Systems
{
	internal class DrawSpriteSystem : IDrawableSystem
	{
		private readonly ECSManager manager;
		public DrawSpriteSystem()
		{
			this.manager = ECSManager.GetInstance();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			int xPos, yPos;
			int width, height;
			Point pointPos;
			foreach (var (entity, rigidBody, sprite) in manager.GetEntitiesWithComponents<RigidBody2DComponent, SpriteComponent>())
			{
				pointPos = rigidBody.position.ToPoint();
				xPos = pointPos.X;
				yPos = pointPos.Y;
				width = sprite.spriteDimensions.X;
				height = sprite.spriteDimensions.Y;
				spriteBatch.Draw(sprite.spriteSheet, new Rectangle(xPos, yPos, width, height), sprite.spriteSource, sprite.color);
				//Debug.WriteLine($"Drew Sprite {0} at position ({1}, {2})", entity.ToString(), position.posX, position.posY);
			}
		}
	}
}	
