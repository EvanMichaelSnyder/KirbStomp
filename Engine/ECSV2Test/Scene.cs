
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.ECSV2.Systems.SystemsManagement;
using KirbStomp.Engine.ECSV2Test.EntityObjects;
using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2Test
{
    //scene abstractly is like the level and everything that contains. This includes entites, hud, etc.
    internal class Scene
    {
        private SystemsManager systemManager;
        private EntityManager entityManager;
        public Scene(Game1 game)
        {
            this.entityManager = EntityManager.GetInstance();
            systemManager = new SystemsManager();
            //TODO REMOVE TEST
            Entity mario = ExMarioCharacter.CreateExampleMarioCharacter(game);
            Entity swap = SwappingItems.CreateSwappingItemsEntity();

			
            
        }

        public void UpdateAll(float deltaTime)
        {
            systemManager.UpdateAllSystem(deltaTime);

			// FORCING AN ENTITY REMOVAL FOR TESTING THIS SHOULD NOT BE HERE OTHERWISE
			if(GlobalInputs.GetInstance().IsInputJustPressed(Microsoft.Xna.Framework.Input.Keys.L))
			{
                EntityManager.GetInstance().RemoveEntity(this.entityManager.getEntities()[0]);
			}
        }

        public void LoadAll(ContentManager content)
        {
            systemManager.LoadAlLSystem(content);
        }
        public void DrawAll(SpriteBatch spriteBatch)
        {
            systemManager.DrawAllSystem(spriteBatch);
        }
    }
}
