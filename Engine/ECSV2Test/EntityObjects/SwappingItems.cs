using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2Test.EntityObjects
{
    internal class SwappingItems
    {
        public static Entity CreateSwappingItemsEntity()
        {
            EntityManager manager = EntityManager.GetInstance();
            Entity entity = manager.CreateEntity();
            manager.AddComponent(entity, new SpriteComponent(default, new(), 100, 100, Color.White));
            manager.AddComponent(entity, new AnimationComponent("Idle"));
            manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.DYNAMIC));
			string[] items = { "Idle", "Running", "Hamburger", "Star", "Firework"};
			manager.AddComponent(entity, new CycleItemsComponent(entity, items, items.Count(), Keys.K, Keys.J));
			List<(int, float, Vector2)> path = new()
			{
				(0, 200, new(0, 1)),
				(10, 100, new (1, 0)),
				(20, 100, new (1, -1)),
				(30, 100, new (-1, -1)),
				(40, 100, new(-1,0)),
				(50, 100, new (0, 1))

			};
			manager.AddComponent(entity, new SetTrajectory(path, 0, 2));
            return entity;

        }

    }
}
