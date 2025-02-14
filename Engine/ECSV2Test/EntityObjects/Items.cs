using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2Test.EntityObjects
{
	internal class Items
	{
		public static Entity CreateEntity()
		{
			EntityManager manager = EntityManager.GetInstance();
			Entity entity = manager.CreateEntity();
			entity.SetPosition(400, 200);
			manager.AddComponent(entity, new SpriteComponent(default, new(), 100, 100, Color.White));
			manager.AddComponent(entity, new AnimationComponent(("Items", "Firework")));
			manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.STATIC));
			(string, string)[] animations = { ( "Items", "Firework" ), 
				("Items", "Star"),
				("Items", "Hamburger")
			};
			manager.AddComponent(entity, new CycleAnimationsComponent(entity, animations, animations.Count(), Keys.I, Keys.U));
			return default;
		}

	}
}
