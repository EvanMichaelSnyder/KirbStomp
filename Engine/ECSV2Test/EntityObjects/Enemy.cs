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
	internal class Enemy
	{
		public static Entity CreateEnemyEntity()
		{
			EntityManager manager = EntityManager.GetInstance();
			Entity entity = manager.CreateEntity();
			entity.SetPosition(600, 300);
			manager.AddComponent(entity, new SpriteComponent(default, new(), 100, 100, Color.White));
			manager.AddComponent(entity, new AnimationComponent(("LinkTransparentSpriteSheet", "StartNeutralAttack")));
			manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.DYNAMIC));
			(string, string)[] animations = { ( "LinkTransparentSpriteSheet", "Idle" ), 
				("LinkTransparentSpriteSheet", "Run"),
				("LinkTransparentSpriteSheet", "Jump"),
				("LinkTransparentSpriteSheet", "Land"),
				("LinkTransparentSpriteSheet", "StartNeutralAttack")
			};
			manager.AddComponent(entity, new CycleAnimationsComponent(entity, animations, animations.Count(), Keys.P, Keys.O));
			List<(int, float, Vector2)> path = new()
			{
				(0, 200, new(0, 1)),
				(10, 200, new (1, 0)),
				(20, 100, new (0, -1)),
				(30, 100, new (0, -1)),
				(40, 200, new(-1,0)),
				(50, 100, new (0, 1))

			};
			manager.AddComponent(entity, new SetTrajectory(path, 0, 2));
			return entity;

		}

	}
}