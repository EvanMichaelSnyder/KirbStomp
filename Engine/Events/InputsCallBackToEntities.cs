using KirbStomp.Engine.ECSV2;
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
        private Dictionary<(Entity, Keys), List<EntitysKeyCallBackFN>> keyEntityFNCallbacks;
		private Dictionary<Keys, List<Entity>> keysEntitiesDictionary;

		private int executionDepth;
		private Queue<(Entity, Keys, EntitysKeyCallBackFN)> queueToAddCBFN;
		private Queue<(Entity, Keys, EntitysKeyCallBackFN)> queueToRemoveCBFN;
        public InputsCallBackToEntities()
        {
			keyEntityFNCallbacks = new();
			keysEntitiesDictionary = new();
			executionDepth = 0;
			queueToAddCBFN = new();
			queueToRemoveCBFN = new();
        }

		// Since this execution may occur mid execution, we must have some method of tracking execution depth
		// This is done through member variable executionDepth which will be non 0 during execution
		public void Execute(Keys key)
        {
			List<Entity> entities;
			List<EntitysKeyCallBackFN> callBackFunctions;
			this.executionDepth++;
			if (!keysEntitiesDictionary.TryGetValue(key, out entities)) return; 
			
			foreach(Entity entity in entities)
			{
				if (!keyEntityFNCallbacks.TryGetValue((entity, key), out callBackFunctions)) continue;

				foreach(EntitysKeyCallBackFN fn in callBackFunctions)
				{
					fn(entity);
				}
		
			}
			if(--this.executionDepth == 0) ProcessMidExecuteChanges(); 		
		}
		private void ProcessMidExecuteChanges()
		{
			foreach (var (entity, key, fn) in queueToAddCBFN)
			{
				AddToDictionaries(entity, key, fn);
			}
			queueToAddCBFN.Clear();
			foreach(var (entity, key, fn) in queueToRemoveCBFN)
			{
				RemoveFromDictionaries(entity, key, fn);
			}
			queueToRemoveCBFN.Clear();

		}

		public void AddEntityKeyCallBack(Entity entity, Keys key, EntitysKeyCallBackFN fn)
		{
			if(executionDepth == 0)
			{
				AddToDictionaries(entity, key, fn);
			} else
			{
				queueToAddCBFN.Enqueue((entity, key, fn));
			}
		}

		public bool RemoveKeyCallBackFn(Entity entity, Keys key, EntitysKeyCallBackFN fn)
		{
			if(executionDepth == 0)
			{
				RemoveFromDictionaries(entity, key, fn);
			}
			else
			{
				queueToRemoveCBFN.Enqueue((entity, key, fn));
			}
			return true;
		}

		
		public int RemoveAllEntitysCallBacks(Entity entity)
		{
			int count = 0;
			List<(Entity, Keys, EntitysKeyCallBackFN)> thingsToRemove = new();
			foreach (var (entityInDictionary, keyInDictionary) in keyEntityFNCallbacks.Keys)
			{
				if (!entityInDictionary.Equals(entity)) continue;
				foreach (EntitysKeyCallBackFN fn in keyEntityFNCallbacks[(entityInDictionary, keyInDictionary)])
				{
					thingsToRemove.Add((entityInDictionary, keyInDictionary, fn));
				}
			}
			if(executionDepth == 0)
			{
				foreach (var removeData in thingsToRemove)
				{
					RemoveFromDictionaries(removeData.Item1, removeData.Item2, removeData.Item3);
					count++;
				}
			} else
			{
				foreach (var removeData in thingsToRemove)
				{
					queueToRemoveCBFN.Enqueue((removeData.Item1, removeData.Item2, removeData.Item3));
					count++;
				}
			}
			return count;
		}
		private void AddToDictionaries(Entity entity, Keys key, EntitysKeyCallBackFN fn)
		{
			if (!keyEntityFNCallbacks.ContainsKey((entity, key))) keyEntityFNCallbacks.Add((entity, key), new());
			keyEntityFNCallbacks[(entity, key)].Add(fn);
			if (!keysEntitiesDictionary.ContainsKey(key)) keysEntitiesDictionary.Add(key, new());
			keysEntitiesDictionary[key].Add(entity);
		}
		private void RemoveFromDictionaries(Entity entity, Keys key, EntitysKeyCallBackFN fn)
		{			
			if(keyEntityFNCallbacks.ContainsKey((entity, key)))
			{
				Logger.Log($"No Callback functions under the entity: {entity.GetID()} and key: {key.ToString()} to remove!");
				return;
			}
			keyEntityFNCallbacks[(entity, key)].Remove(fn);
			if (keyEntityFNCallbacks[(entity, key)].Count() == 0) keysEntitiesDictionary[key].Remove(entity);
			if (keysEntitiesDictionary[key].Count() == 0) keysEntitiesDictionary.Remove(key);
		}
    }
}
