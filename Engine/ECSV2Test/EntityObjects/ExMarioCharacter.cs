
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.Components.IComponents;
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
			manager.AddComponent(entity, new AnimationComponent(("MarioTransparentSpriteSheet", "Idle")));
			manager.AddComponent(entity, new RigidBody2DComponent(RigidBody2DComponent.BodyType.DYNAMIC));
			manager.AddComponent(entity, new ExamplePlayerState("Player 1"));
            manager.AddComponent(entity, new PlayerControlsComponent());

			

            return entity;
        }
    }
}
