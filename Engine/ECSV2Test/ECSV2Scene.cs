using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.ECSV2.Systems.SystemsManagement;
using KirbStomp.Engine.ECSV2Test.EntityObjects;
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
        private List<ECSEntity> allEntities;
        private SystemsManager systemManager;
        public ECSV2Scene(Game1 game)
        {
			allEntities = new()
			{
				ExMarioCharacter.CreateExampleMarioCharacter(game),
				SwappingItems.CreateSwappingItemsEntity()
			};
            systemManager = new SystemsManager();
        }

        public void UpdateAll(float deltaTime)
        {
            systemManager.UpdateAllSystem(deltaTime);
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
