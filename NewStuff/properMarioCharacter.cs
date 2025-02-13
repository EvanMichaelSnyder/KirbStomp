
using KirbStomp;
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    internal class properMarioCharacter
    {
        public properMarioCharacter()
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
            return entity;
        }
    }