using KirbStomp.Engine.ECSV2.ECSEntityManagement;
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
        }
        public static GlobalEvents GetInstance()
        {
            if (instance == null) instance = new GlobalEvents();
            return instance;
        }

        public void SubscribeToInputEvents(Entity entity, IKeyCommands callbackFN)
        {
			Debug.WriteLine("NOT IMPLEMENTED YET");
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
			command.RemoveEntityKeyCallBackFn(key, callbackFN);
			inputs.RemoveCommandToKeyStatus(key, command, status);
		}
		public void UnsubscriptAllEntityEvents(Entity entity)
		{
			inputPressedCallBacks.RemoveAlLEntitiesCallBacks(entity);
			inputJustPressedCallBacks.RemoveAlLEntitiesCallBacks(entity);
			inputJustReleasedCallBacks.RemoveAlLEntitiesCallBacks(entity);
		}


    }
}


