using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.Events.Commands;
using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.Events
{
    public delegate void EntitysKeyCallBackFN(Entity entity);
    internal class GlobalEvents
    {
        private static GlobalEvents instance;
        private InputsCallBackToEntities inputPressedCallBacks;
        private InputsCallBackToEntities inputJustPressedCallBacks;
        private InputsCallBackToEntities inputJustReleasedCallBacks;
		private GlobalInputs inputs;
		private Dictionary<InputStatus, InputsCallBackToEntities> dict;
        private GlobalEvents()
        {
			dict = new()
			{
				{	InputStatus.Pressed, inputPressedCallBacks = new()},
				{	InputStatus.JustPressed, inputJustPressedCallBacks = new() },
				{   InputStatus.JustReleased, inputJustReleasedCallBacks = new() }

			};
			inputs = GlobalInputs.GetInstance();
			inputs.ResetAllCallBacks();
        }
        public static GlobalEvents GetInstance()
        {
            if (instance == null) instance = new GlobalEvents();
            return instance;
        }
		public static void ResetInstance()
		{
			instance = new GlobalEvents();
		}

        public void SubscribeToKeyEvents(Entity entity, Keys key, InputStatus status, EntitysKeyCallBackFN callbackFN)
        {
			InputsCallBackToEntities command = dict[status];
			command.AddEntityKeyCallBack(entity, key, callbackFN);
			inputs.AddCommandToKeyStatus(key, command, status);
        }
		public void UnsubscribeToKeyEvents(Entity entity, Keys key, InputStatus status, EntitysKeyCallBackFN callbackFN)
		{
			InputsCallBackToEntities command = dict[status];
			command.RemoveKeyCallBackFn(entity, key, callbackFN);
			inputs.RemoveCommandToKeyStatus(key, command, status);
		}
		public void UnsubscriptAllEntityEvents(Entity entity)
		{
			inputPressedCallBacks.RemoveAllEntitysCallBacks(entity);
			inputJustPressedCallBacks.RemoveAllEntitysCallBacks(entity);
			inputJustReleasedCallBacks.RemoveAllEntitysCallBacks(entity);
		}


    }
}


