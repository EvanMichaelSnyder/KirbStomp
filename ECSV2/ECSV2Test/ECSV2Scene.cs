using KirbStomp.ECSV2.Components.IECSComponents;
using KirbStomp.ECSV2.ECSEntities;
using KirbStomp.ECSV2.EntityObjects;
using KirbStomp.ECSV2.Systems.SystemsManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.ECSV2Test
{
    internal class ECSV2Scene
    {
		private List<ECSEntity> allEntities;
		private SystemsManager systemManager;
		public ECSV2Scene(Game1 game)
		{
			allEntities = new()
			{
				ExMarioCharacter.CreateExampleMarioCharacter(game)
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
