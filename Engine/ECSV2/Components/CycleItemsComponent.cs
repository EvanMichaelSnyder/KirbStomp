using KirbStomp.Engine.ECSV2.Components.IComponents;


using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems;
using KirbStomp.Engine.Events;
using KirbStomp.Engine.Events.Commands;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class CycleItemsComponent : Component, IDisposable
	{
		public readonly Entity entity;
		public int index;
		public int totalItems;
		public Keys cycleForward;
		public Keys cycleBack;
		public string[] items;
		public bool changeHandled;
		public CycleItemsComponent(Entity entity, string[] items, int numberOfItems, Keys cycleForwardKey, Keys cycleBackKey)
		{
			this.entity = entity;
			index = 0;
			GlobalEvents events = GlobalEvents.GetInstance();
			cycleForward = cycleForwardKey;
			cycleBack = cycleBackKey;
			events.SubscribeToKeyEvents(entity, cycleForward, Inputs.InputStatus.JustPressed, IncrementIndex);
			events.SubscribeToKeyEvents(entity, cycleBack, Inputs.InputStatus.JustPressed, DecrementIndex);
			changeHandled = true;

			this.items = items;
			this.totalItems = numberOfItems;

		}
		public void Dispose()
		{
			GlobalEvents events = GlobalEvents.GetInstance();
			events.UnsubscriptAllEntityEvents(entity);
		}
		public void IncrementIndex(Entity entity)
		{
			this.index++;
			if (index >= totalItems) index = 0;
			changeHandled = false;
			AnimationSystem.ChangeEntitysCurrentAnimation(entity, ("mario", items[index]));
			Debug.WriteLine($"Incremented: We're on item {items[index].ToString()}");
		}
		public void DecrementIndex(Entity entity)
		{
			this.index--;
			if (index < 0) index = totalItems - 1;
			changeHandled = false;
			AnimationSystem.ChangeEntitysCurrentAnimation(entity, ("mario", items[index]));
			Debug.WriteLine($"Decremented: We're on item {items[index].ToString()}");
		}
	}
}