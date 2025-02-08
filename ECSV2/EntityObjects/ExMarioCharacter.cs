using KirbStomp.ECSV2.Components;
using KirbStomp.ECSV2.ECSEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KirbStomp.ECSV2.EntityObjects
{
	internal class ExMarioCharacter
	{
		public ExMarioCharacter()
		{
		}

		public static ECSEntity CreateExampleMarioCharacter(Game1 game1)
		{
			ECSManager manager = ECSManager.GetInstance();
			ECSEntity entity = manager.CreateEntity();
			manager.AddComponent<SpriteComponent>(entity, new SpriteComponent(game1.Content.Load<Texture2D>("mario"), new(), new(), Color.White));
			manager.AddComponent<AnimationComponent>(entity, new AnimationComponent("Runnnning"));
			manager.AddComponent<RigidBody2DComponent>(entity, new RigidBody2DComponent(new Vector2(0, 0), new Vector2(50, 50), new Vector2(0, 0)));
			manager.AddComponent<ExamplePlayerState>(entity, new ExamplePlayerState("Player 1"));
			manager.AddComponent<PlayerControls>(entity, new());
			return entity;
		}
	}
}
