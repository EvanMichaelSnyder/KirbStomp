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
    public delegate void EntitysKeyCallBackFN(ECSEntity entity);
    internal class InputsCallBackToEntities : IKeyCommands
    {
        private Dictionary<EntitysKeyCallBackFN, ECSEntity> fnEntityDictionary;
        private Dictionary<Keys, List<EntitysKeyCallBackFN>> keyEntityFNCallbacks;
        private GlobalInputs inputs;

        public InputsCallBackToEntities()
        {
            inputs = GlobalInputs.GetInstance();
        }

        public void Execute(Keys key)
        {
            ECSEntity entity;
            if (keyEntityFNCallbacks.TryGetValue(key, out List<EntitysKeyCallBackFN> CBFNList))
            {
                foreach (EntitysKeyCallBackFN fn in CBFNList)
                {
                    entity = fnEntityDictionary[fn];
                    fn(entity);
                }
            }

        }
    }
}
