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

        public InputsCallBackToEntities()
        {
			fnEntityDictionary = new();
			keyEntityFNCallbacks = new();
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

		public bool AddEntityKeyCallBack(ECSEntity entity, Keys key, EntitysKeyCallBackFN fn)
		{
			if (fnEntityDictionary.ContainsKey(fn)) return false;
			fnEntityDictionary.Add(fn, entity);

			if (!keyEntityFNCallbacks.ContainsKey(key)) keyEntityFNCallbacks.Add(key, new());
			keyEntityFNCallbacks[key].Add(fn);

			return true;
		}

    }
}
