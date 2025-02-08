using ECSV2.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Systems.SystemsManagement
{
	internal class SystemsLoader
	{
		public static SystemsLoader instance;

		private SystemsLoader()
		{
			
		}

		public static void LoadAllSystem(SystemsManager manager)
		{
			manager.AddSystem(new AnimationSystem());
			manager.AddSystem(new ExamplePlayerPhysicsSystem());
			manager.AddSystem(new PlayerMovementSystem());
			manager.AddSystem(new DrawSpriteSystem());
		}
	}
}
