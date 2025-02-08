using KirbStomp.Engine.Commands;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.Events.Commands;
using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.Events
{
    internal class GlobalEvents
    {
        private static GlobalEvents instance;
        private InputsCallBackToEntities inputCallBacks;
        private GlobalEvents()
        {
            //SetInputCallBack();
        }
        public static GlobalEvents GetInstance()
        {
            if (instance == null) instance = new GlobalEvents();
            return instance;
        }

        public void SubscriptToInputEvents(ECSEntity entity, IKeyCommands callbackFN)
        {
			
        }
        public void SubscriptToKeyEvents(ECSEntity entity, Keys key, InputStatus status, IKeyCommands callbackFN)
        {

        }


    }
}


