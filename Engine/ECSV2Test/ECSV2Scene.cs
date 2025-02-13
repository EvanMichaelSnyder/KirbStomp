using KirbStomp.Engine.ECSV2.Components.IComponents;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.SystemsManagement;
using KirbStomp.Engine.ECSV2Test.EntityObjects;
using KirbStomp.Engine.Events;
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
    internal class ECSV2Scene
    {
        private List<Entity> allEntities;
        private SystemsManager systemManager;
        public ECSV2Scene(Game1 game)
        {
			allEntities = new()
			{
				Block.CreateEntity(),
				Items.CreateEntity(),
				MainCharacter.CreateEntity()
			};
            systemManager = new SystemsManager();
        }

        public void UpdateAll(float deltaTime)
        {
            systemManager.UpdateAllSystem(deltaTime);

			// FORCING AN ENTITY REMOVAL FOR TESTING THIS SHOULD NOT BE HERE OTHERWISE
			if(GlobalInputs.GetInstance().IsInputJustPressed(Microsoft.Xna.Framework.Input.Keys.L))
			{
				EntityManager.GetInstance().RemoveEntity(allEntities[1]);
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

		public void ResetAll(ContentManager content)
		{
			allEntities.Clear();
			EntityManager.ResetEntityManager();
			systemManager.ResetAllSystems();
			GlobalEvents.ResetInstance();
			allEntities = new()
			{
				Block.CreateEntity(),
				Items.CreateEntity(),
				MainCharacter.CreateEntity()
			};
			LoadAll(content);
		}

    }
}
