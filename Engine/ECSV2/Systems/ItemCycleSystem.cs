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
	internal class ItemCycleSystem : IUpdatableSystem
	{
		private EntityManager manager;
		public ItemCycleSystem()
		{
			manager = EntityManager.GetInstance();
		}
		public void Update(float deltaTime)
		{
			foreach(var (entity, cyclingComponent) in manager.GetEntitiesWithComponent<CycleItemsComponent>())
			{

			}
		}
	}
}
