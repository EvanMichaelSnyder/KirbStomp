using System.Collections.Generic;
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
            SortedDictionary<int,List<Entity>> entitySortedByZIndex = this.getDrawableEntitySortedByZIndex();
            foreach (KeyValuePair<int, List<Entity>> zIndexEntityPair in entitySortedByZIndex)
            {// cycle through entity by zIndex order
                foreach(Entity entity in zIndexEntityPair.Value)
                {
                    //alr checked if entity had spriteComponent
                    SpriteComponent sprite = manager.GetComponent<SpriteComponent>(entity);
                    float tempOffsetX = sprite.GetOffSet().X;
                    float tempOffsetY = sprite.GetOffSet().Y;
                    pointPos = entity.GetPosition().ToPoint();
                    scale = sprite.GetScale();
                    xPos = (int)(scaleX * (pointPos.X + (tempOffsetX) * scale));
                    yPos = (int)(scaleY * (pointPos.Y + (tempOffsetY) * scale));
                    width = (int)(scaleX * (sprite.GetSpriteWidth() * scale));
                    height = (int)(scaleY * (sprite.GetSpriteHeight() * scale));

                    spriteBatch.Draw(sprite.GetTexture(), new Rectangle(xPos, yPos, width, height), sprite.GetSpriteSource(), sprite.GetColor());
                    //Debug.WriteLine($"Drew Sprite {0} at position ({1}, {2})", entity.ToString(), position.posX, position.posY);
                }
            }
        }
        //returns dictionary of list of entity sorted by z-index
        private SortedDictionary<int, List<Entity>> getDrawableEntitySortedByZIndex()
        {
            SortedDictionary<int, List<Entity>> dictByZIndex = new SortedDictionary<int, List<Entity>>();
            foreach (var (entity, sprite) in manager.GetEntitiesWithComponent<SpriteComponent>())
            {
                if (!dictByZIndex.ContainsKey(sprite.GetZIndex()))
                {
                    dictByZIndex.Add(sprite.GetZIndex(), new List<Entity>());
                }
                dictByZIndex[sprite.GetZIndex()].Add(entity);
            }
            return dictByZIndex;

        }
    }
}
