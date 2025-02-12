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
	internal class CycleAnimationsComponent
		: Component, IDisposable
	{
		private readonly Entity entity;
		private int index;
		private int totalItems;
		private Keys cycleForward;
		private Keys cycleBack;
		private (string, string)[] animations;
		private bool changeHandled;
		public CycleAnimationsComponent(Entity entity, (string, string)[] animations, int numberOfItems, Keys cycleForwardKey, Keys cycleBackKey)
		{
			this.entity = entity;
			index = 0;
			GlobalEvents events = GlobalEvents.GetInstance();
			cycleForward = cycleForwardKey;
			cycleBack = cycleBackKey;
			events.SubscribeToKeyEvents(entity, cycleForward, Inputs.InputStatus.JustPressed, IncrementIndex);
			events.SubscribeToKeyEvents(entity, cycleBack, Inputs.InputStatus.JustPressed, DecrementIndex);
			changeHandled = true;

			this.animations = animations;
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
			AnimationSystem.ChangeEntitysCurrentAnimation(entity, animations[index]);
			Logger.Log($"Incremented: We're on item {animations[index].ToString()}");
		}
		public void DecrementIndex(Entity entity)
		{
			this.index--;
			if (index < 0) index = totalItems - 1;
			changeHandled = false;
			AnimationSystem.ChangeEntitysCurrentAnimation(entity, animations[index]);
			Logger.Log($"Decremented: We're on item {animations[index].ToString()}");
		}
	}
}