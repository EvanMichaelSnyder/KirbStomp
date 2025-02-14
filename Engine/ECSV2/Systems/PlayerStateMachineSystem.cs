using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
	internal class PlayerStateMachineSystem : IUpdatableSystem
	{
		private EntityManager manager;
		public PlayerStateMachineSystem()
		{
			manager = EntityManager.GetInstance();
		}
		public void Update(float deltaTime)
		{
			Vector2 walkingDir;
			AnimationComponent component;
			(string, string) newAnimationName;
			foreach (var (entity, playerState, rigidBody) in manager.GetEntitiesWithComponents<ExamplePlayerState, RigidBody2DComponent>())
			{
				walkingDir = playerState.GetWalkingDirection();
				rigidBody.SetXVelocity(walkingDir.X * playerState.GetMovementVelocity());
				component = manager.GetComponent<AnimationComponent>(entity);
				newAnimationName = ("MarioTransparentSpriteSheet", playerState.GetWalkingAnimation());
				if(playerState.GetStartedWalkingState() || playerState.GetStoppedWalkingState())
				{
					AnimationSystem.ChangeEntitysCurrentAnimation(entity, newAnimationName);
				}
				if(playerState.GetPlayerJumped())
				{
					rigidBody.SetYVelocity(playerState.GetJumpVelocity());
				}
				if(playerState.PlayerAttacks())
				{
					newAnimationName = (newAnimationName.Item1, playerState.GetAttackAnimation());
					AnimationSystem.ChangeEntitysCurrentAnimation(entity, newAnimationName);
					AnimationSystem.ChangeEntitysNextAnimation(entity, (newAnimationName.Item1, "Idle"));
				}
			}
		}
	}
}
