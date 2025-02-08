using ECSV2.Systems.ISystems;
using KirbStomp.ECSV2.Components;
using KirbStomp.ECSV2.ECSEntities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Systems
{
	internal class ExamplePlayerPhysicsSystem : ISystem, IUpdatableSystem
	{
		private readonly ECSManager manager;
		public ExamplePlayerPhysicsSystem()
		{
			manager = ECSManager.GetInstance();
		}

		public void Update(float deltaTime)
		{
			Vector2 previousPosition;
			foreach(var (entity, rigidBody, playerState) in manager.GetEntitiesWithComponents<RigidBody2DComponent, ExamplePlayerState>())
			{
				previousPosition = rigidBody.position;
				rigidBody.Update(deltaTime);
				
				
				
				if(rigidBody.position.X < 400 && rigidBody.position.Y < 240)
				{
					
					playerState.exState1 = true;
					playerState.exState2 = true;
				} else
				{
					playerState.exState1 = false;
					playerState.exState2 = false;	
				}
			}
		}
	}
}
