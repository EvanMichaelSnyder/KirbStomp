using KirbStomp.Engine.Animations.Content;
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class DrawSpriteSystem : IDrawableSystem
    {
        private readonly ECSManager manager;
        public DrawSpriteSystem()
        {
            manager = ECSManager.GetInstance();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int xPos, yPos;
            int width, height;
            Point pointPos;
			float scaleX = (float)Game1.globalScaleX;
			float scaleY = (float)Game1.globalScaleY;
			var (gWidth, gHeight) = Game1.GetAdjustedWindowSize();

			float tempOffset = 0;
			float scale = 1.2f;
            foreach (var (entity, rigidBody, sprite) in manager.GetEntitiesWithComponents<RigidBody2DComponent, SpriteComponent>())
            {
                pointPos = rigidBody.position.ToPoint();
				scale = sprite.scale;
				xPos = (int)(scaleX * (pointPos.X + (tempOffset) * scale));
                yPos = (int)(scaleY * (pointPos.Y + (tempOffset) * scale));
                width = (int)(scaleX * (sprite.spriteDimensions.X * scale));
                height = (int)(scaleX * (sprite.spriteDimensions.X * scale));

                spriteBatch.Draw(sprite.spriteSheet, new Rectangle(xPos, yPos, width, height), sprite.spriteSource, sprite.color);
                //Debug.WriteLine($"Drew Sprite {0} at position ({1}, {2})", entity.ToString(), position.posX, position.posY);
            }
        }
    }
}
