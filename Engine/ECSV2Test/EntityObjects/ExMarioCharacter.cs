
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KirbStomp.Engine.ECSV2Test.EntityObjects
{
    internal class ExMarioCharacter
    {
        public ExMarioCharacter()
        {
        }

        public static Entity CreateExampleMarioCharacter(Game1 game1)
        {
            EntityManager manager = EntityManager.GetInstance();
            Entity entity = manager.CreateEntity();
			manager.AddComponent(entity, new SpriteComponent(default, new(), 100, 100, Color.White));
			manager.AddComponent(entity, new AnimationComponent(("mario", "Running")));
			manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.DYNAMIC));
			manager.AddComponent(entity, new ExamplePlayerState("Player 1"));
            manager.AddComponent<PlayerControlsComponent>(entity, new());
			List<(float, Vector2)> path = new()
			{
				(1.0f, new Vector2(500, 0))
			};
			manager.AddComponent(entity, new CreateProjectileComponent(entity, ("Items", "Hamburger"), path, Keys.Z));
            return entity;
        }
    }
}
