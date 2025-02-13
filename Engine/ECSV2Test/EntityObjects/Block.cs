using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2Test.EntityObjects
{
	internal class Block
	{
		public static Entity CreateEntity()
		{
			EntityManager manager = EntityManager.GetInstance();
			Entity entity = manager.CreateEntity();
			entity.SetPosition(200, 200);
			manager.AddComponent(entity, new SpriteComponent(default, new(), 100, 100, Color.White));
			manager.AddComponent(entity, new AnimationComponent(( "PlatformBlocks", "Brick" )));
			manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.STATIC));
			(string, string)[] animations = { ( "PlatformBlocks", "Brick" ), 
				("PlatformBlocks", "Steel"),
				("PlatformBlocks", "Wood"),
				("PlatformBlocks", "Pipes"),
				("PlatformBlocks", "Dirt")
			};
			manager.AddComponent(entity, new CycleAnimationsComponent(entity, animations, animations.Count(), Keys.Y, Keys.T));
			return default;
		}
	}
}
