using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
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
    internal class CycleItems : IECSComponent
    {

		private int index;
		private int totalItems;
		Keys cycleForward;
		Keys cycleBack;
		private string[] items;
        public CycleItems(ECSEntity entity, string[] items, int numberOfItems, Keys cycleForwardKey, Keys cycleBackKey)
        {
			index = 0;
			GlobalEvents events = GlobalEvents.GetInstance();
			cycleForward = cycleForwardKey;
			cycleBack = cycleBackKey;
			events.SubscriptToKeyEvents(entity, cycleForward, Inputs.InputStatus.JustPressed, IncrementIndex);
			events.SubscriptToKeyEvents(entity, cycleBack, Inputs.InputStatus.JustPressed, DecrementIndex);


			this.items = items;
			this.totalItems = numberOfItems;

        }
		public void IncrementIndex(ECSEntity entity)
		{
			this.index++;
			if (index >= totalItems) index = 0;
			Debug.WriteLine($"Incremented: We're on item {items[index].ToString()}");
		}
		public void DecrementIndex(ECSEntity entity)
		{
			this.index--;
			if (index < 0) index = totalItems - 1;
			Debug.WriteLine($"Decremented: We're on item {items[index].ToString()}");
		}
    }
}
