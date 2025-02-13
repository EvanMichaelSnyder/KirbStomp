using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class DrawSpriteSystem : IDrawableSystem
    {
        private readonly EntityManager manager;
        public DrawSpriteSystem()
        {
            manager = EntityManager.GetInstance();
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
                float tempOffsetX = sprite.GetOffSet().X;
                float tempOffsetY = sprite.GetOffSet().Y;
                pointPos = entity.GetPosition().ToPoint();
				scale = sprite.GetScale();
				xPos = (int)(scaleX * (pointPos.X + (tempOffset) * scale));
                yPos = (int)(scaleY * (pointPos.Y + (tempOffset) * scale));
                width = (int)(scaleX * (sprite.GetSpriteWidth() * scale));
                height = (int)(scaleX * (sprite.GetSpriteHeight() * scale));

                spriteBatch.Draw(sprite.GetTexture(), new Rectangle(xPos, yPos, width, height), sprite.GetSpriteSource(), sprite.GetColor());
                //Debug.WriteLine($"Drew Sprite {0} at position ({1}, {2})", entity.ToString(), position.posX, position.posY);
            }
        }
    }
}
