using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
	internal class GlobalMovementSystem : IUpdatableSystem
	{
		private EntityManager manager;
		public GlobalMovementSystem()
		{
			manager = EntityManager.GetInstance();
		}

		public void Update(float deltaTime)
		{
			foreach (var (entity, rigidBody) in manager.GetEntitiesWithComponent<RigidBody2DComponent>())
			{
				rigidBody.Update(deltaTime);
			}
		}
	}
}
