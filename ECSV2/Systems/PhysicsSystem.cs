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
	internal class PhysicsSystem : ISystem, IUpdatableSystem
	{
		private readonly ECSManager manager;

		public void Update(float deltaTime)
		{
			Vector2 previousPosition;
			foreach(var (entity, rigidBody) in manager.GetEntitiesWithComponent<RigidBody2DComponent>())
			{
				previousPosition = rigidBody.position;
				rigidBody.Update(deltaTime);

			}
		}
	}
}
