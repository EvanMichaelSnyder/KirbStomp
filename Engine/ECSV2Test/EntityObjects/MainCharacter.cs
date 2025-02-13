using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.Components.IComponents;
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
	internal class MainCharacter
	{
        public static Entity CreateEntity()
        {
            EntityManager manager = EntityManager.GetInstance();
            Entity entity = manager.CreateEntity();
			manager.AddComponent(entity, new SpriteComponent());
			manager.AddComponent(entity, new AnimationComponent(("mario", "Running")));	

			(string, string)[] animations = { ( "MarioTransparentSpriteSheet", "Idle" ), 
				("MegaManTransparentSpriteSheet", "Idle"),
				("mario", "Idle")
			};
			manager.AddComponent(entity, new CycleAnimationsComponent(entity, animations, animations.Count(), Keys.I, Keys.U));

			manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.DYNAMIC));
			manager.AddComponent(entity, new ExamplePlayerState("Player 1"));
            manager.AddComponent<PlayerControlsComponent>(entity, new());
			List<(float, Vector2)> path = new()
			{
				(1.0f, new Vector2(500, 0))
			};

			List<(float, Vector2)> path2 = new()
			{
				(1.0f, new Vector2(500, 0)),
				(1.0f, new Vector2(-500, 0))
			};
			List<(float, Vector2)> path3= new()
			{
				(0.25f, new Vector2(1000, 0)),
				(0.25f, new Vector2(0, -1000)),
				(0.25f, new Vector2(-1000, 0)),
				(0.25f, new Vector2(0, 1000))
			};

			//manager.AddComponent(entity, new CreateProjectileComponent(entity, ("Items", "Hamburger"), path, Keys.Z));
			List<Component> cyclingComponents = new()
			{ 
				new CreateProjectileComponent(entity, ("Items", "Hamburger"), path, Keys.D1) ,
				new CreateProjectileComponent(entity, ("MarioTransparentSpriteSheet", "AttackNeutral1"), path, Keys.D2),
				new CreateProjectileComponent(entity, ("Items", "Star"), path3, Keys.D3) ,
			};

			manager.AddComponent(entity, new CycleComponents(entity, cyclingComponents, Keys.D9, Keys.D0));
            return entity;
        }

	}
}
