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
    internal class InputsCallBackToEntities : IKeyCommands
    {
        private Dictionary<EntitysKeyCallBackFN, Entity> fnEntityDictionary;
		private Dictionary<EntitysKeyCallBackFN, Keys> callBackToKeyDictionary;
        private Dictionary<Keys, List<EntitysKeyCallBackFN>> keyEntityFNCallbacks;

        public InputsCallBackToEntities()
        {
			fnEntityDictionary = new();
			callBackToKeyDictionary = new();
			keyEntityFNCallbacks = new();
        }

        public void Execute(Keys key)
        {
            Entity entity;
            if (keyEntityFNCallbacks.TryGetValue(key, out List<EntitysKeyCallBackFN> CBFNList))
            {
                foreach (EntitysKeyCallBackFN fn in CBFNList)
                {
                    entity = fnEntityDictionary[fn];
                    fn(entity);
                }
            }
        }

		public bool AddEntityKeyCallBack(Entity entity, Keys key, EntitysKeyCallBackFN fn)
		{
			if (fnEntityDictionary.ContainsKey(fn)) return false;
			fnEntityDictionary.Add(fn, entity);
			callBackToKeyDictionary.Add(fn, key);
			if (!keyEntityFNCallbacks.ContainsKey(key)) keyEntityFNCallbacks.Add(key, new());
			keyEntityFNCallbacks[key].Add(fn);

			return true;
		}

		public bool RemoveEntityKeyCallBackFn(Keys key, EntitysKeyCallBackFN fn)
		{
			Keys dictKey = callBackToKeyDictionary[fn];
			fnEntityDictionary.Remove(fn);
			callBackToKeyDictionary.Remove(fn);
			keyEntityFNCallbacks[dictKey].Remove(fn);
			return false;
		}
		
		public bool RemoveAlLEntitiesCallBacks(Entity entity)
		{
			List<EntitysKeyCallBackFN> list = new();
			foreach(var pair in fnEntityDictionary)
			{
				if ((pair.Value).GetID() == entity.GetID()) list.Add(pair.Key);
			}
			Keys key;
			EntitysKeyCallBackFN fn;
			while(list.Count > 0)
			{
				fn = list.Last();
				key = callBackToKeyDictionary[fn];
				RemoveEntityKeyCallBackFn(key, fn);
				list.RemoveAt(list.Count()- 1);
			}
			return true;
		}
    }
}
