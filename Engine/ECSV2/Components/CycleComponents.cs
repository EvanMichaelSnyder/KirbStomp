using KirbStomp.Engine.ECSV2.Components.IComponents;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.Events;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class CycleComponents	: Component, IDisposable
	{
		private List<Component> components;
		private Type componentsType;
		private EntityManager manager;
		private Keys keyForwards;
		private Keys keyBack;
		private int componentIndex;
		private GlobalEvents events;
		public CycleComponents(Entity entity, List<Component> components, Keys cycleForwardKey, Keys cycleBackKey)
		{
			this.SetEntity(entity);
			this.manager = EntityManager.GetInstance();
			this.components = components;
			componentsType = components.LastOrDefault().GetType();
			Logger.Log($"{componentsType}");
			events = GlobalEvents.GetInstance();
			events.SubscribeToKeyEvents(entity, cycleForwardKey, Inputs.InputStatus.JustPressed, CycleForward);
			events.SubscribeToKeyEvents(entity, cycleBackKey, Inputs.InputStatus.JustPressed, CycleBackward);
		}

		private void CycleForward(Entity entity)
		{

			manager.RemoveComponent(entity, components[componentIndex]);
			components[componentIndex].SetEntity(default);
			IncrementIndex();
			manager.AddComponent(entity, components[componentIndex]);
		}

		private void CycleBackward(Entity entity)
		{
			manager.RemoveComponent(entity, components[componentIndex]);
			DecrementIndex();
			manager.AddComponent(entity, components[componentIndex]);
		}


		private void IncrementIndex()
		{
			componentIndex++;
			if (componentIndex >= components.Count())
				componentIndex = 0;
		}
		private void DecrementIndex()
		{
			componentIndex--;
			if (componentIndex < 0)
				componentIndex = components.Count() - 1;
		}
		public void Dispose()
		{
			manager.RemoveComponent(this.GetEntity(), components[componentIndex]);
			events.UnsubscriptAllEntityEvents(this.GetEntity());
		}
	}
}
