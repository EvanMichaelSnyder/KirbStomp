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
        private readonly EntityManager manager;
        private float xOffSet,yOffSet;
        public DrawSpriteSystem()
        {
            manager = EntityManager.GetInstance();
            this.xOffSet = 0;
            this.yOffSet = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float scale;
            int yPos, xPos, xOff, yOff, width, height;
            float scaleX = (float)Game1.globalScaleX;
            float scaleY = (float)Game1.globalScaleY;
            var (gWidth, gHeight) = Game1.GetAdjustedWindowSize();
            float tempOffset = 0;
            List<Entity> entities = getDrawableEntity();
            foreach (Entity entity in entities)
            {
                SpriteComponent sprite = this.manager.GetComponent<SpriteComponent>(entity);
				scale = sprite.getScale();
                xOff = (int)sprite.getOffSet().X;
                yOff = (int)sprite.getOffSet().Y;
                Vector2 pos = entity.getPosition();
                xPos = (int)(scaleX * (pos.X + (xOff) ));
                yPos = (int)(scaleY * (pos.Y + (yOff) ));
                width = (int)(scaleX * (sprite.getSpriteWidth() * scale));
                height = (int)(scaleX * (sprite.getSpriteHeight() * scale));

                spriteBatch.Draw(sprite.GetTexture(), new Rectangle(xPos, yPos, width, height), sprite.GetSpriteSrc(), sprite.GetColor());
                //Debug.WriteLine($"Drew Sprite {0} at position ({1}, {2})", entity.ToString(), position.posX, position.posY);
            }


        }

        private List<Entity> getDrawableEntity()
        {
            List<Entity> list = new List<Entity>();
            List<Entity> entities = this.manager.getEntities();
            foreach (Entity entity in entities)
            {
                if (this.manager.hasComponent<SpriteComponent>(entity))
                {
                    list.Add(entity);
                }
            }
            return list;
        }
    }
}
