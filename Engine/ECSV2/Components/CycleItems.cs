using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.Events;
using KirbStomp.Engine.Events.Commands;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        public CycleItems(ECSEntity entity, string[] items, int numberOfItems, Keys cycleForwardKey, Keys cycleBackKey)
        {
			index = 0;
			GlobalEvents events = GlobalEvents.GetInstance();
			events.SubscriptToKeyEvents(entity, cycleForward, Inputs.InputStatus.JustPressed, new DelegateToIKeyCommand(IncrementIndex));
        }
		public void Cycle()
		{
			
		}
		public bool IncrementIndex(ECSEntity entity)
		{
			this.index++;
			if (index >= totalItems) index = 0;
			return true;
		}
		public bool DecrementIndex(ECSEntity entity)
		{
			this.index--;
			if (index < 0) index = totalItems - 1;
			return false;
		}
    }
}
