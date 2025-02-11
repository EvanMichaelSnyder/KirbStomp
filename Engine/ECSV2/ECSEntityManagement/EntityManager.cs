using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.EntityManagement
{
    public class EntityManager
	{
		private readonly Dictionary<uint, List<Components.IComponents.Component>> entityComponents;
		private Dictionary<Type, List<Components.IComponents.Component>> componentsPool;
		private List<Entity> entities;
		private static EntityManager instance;
		private EntityManager()
		{
			entityComponents = new();
			componentsPool = new();
			this.entities = new List<Entity>();
		}
		public static EntityManager GetInstance()
		{
			if (instance == null) instance = new EntityManager();
			return instance;
		}
		public Entity CreateEntity()
		{
			Entity entity = new Entity();
			entityComponents[entity.GetID()] = new();
			this.entities.Add(entity);
			return entity;
		}

		// T is an implementation of type IECSComponent, therefore for each unique IECSComponent type added to a component,
		// therefore for each implementation of IECSComponent we have, we have a component pool of each instance added to the pool
		public bool AddComponent<T>(Entity entity, T component) where T : Components.IComponents.Component
		{
			if (!entityComponents.ContainsKey(entity.GetID())) return false;
			entityComponents[entity.GetID()].Add(component);

			if (!componentsPool.ContainsKey(typeof(T))) componentsPool.Add(typeof(T), new());
			componentsPool[typeof(T)].Add(component);

			//make it so every component knows its entity.
			component.setEntity(entity);

			return true;
		}
		public bool RemoveComponent<T>(Entity entity, T component) where T : Components.IComponents.Component
		{
			bool removedFromEnityPool, removedFromComponentPool = false;
			if (!entityComponents.ContainsKey(entity.GetID())) return false;
			removedFromEnityPool = entityComponents[entity.GetID()].Remove(component);
			if (componentsPool.ContainsKey(typeof(T))) removedFromComponentPool = componentsPool[typeof(T)].Remove(component);

			if (component is IDisposable) ((IDisposable)component).Dispose();
			return removedFromEnityPool && removedFromComponentPool;
		}

		public T GetComponent<T>(Entity entity) where T : Components.IComponents.Component
		{
            List<Components.IComponents.Component> components;
			if (entityComponents.TryGetValue(entity.GetID(), out components))
			{
				return components.OfType<T>().FirstOrDefault(); // Either the first occurance of concrete type T, or default T type
			}
			// should crash program if dont exist
			return null;
		}

		public List<Entity> getEntities()
		{
			//return copy of list, dont allow any other class to change list
			List<Entity> entys = new List<Entity>();
			foreach (Entity entity in entities)
			{
				entys.Add(entity);
			}
			return entys;
		}

        public IEnumerable<(Entity, T1, T2)> GetEntitiesWithComponents<T1, T2>()
            where T1 : Components.IComponents.Component where T2 : Components.IComponents.Component
        {
            T1 component1;
            T2 component2;
            foreach (Entity entity in this.entities)
            {
                component1 = GetComponent<T1>(entity);
                component2 = GetComponent<T2>(entity);
                if (component1 != null && component2 != null)
                {
                    yield return (entity, component1, component2);
                }
            }
        }



        public IEnumerable<(Entity, T1)> GetEntitiesWithComponent<T1>()
            where T1 : Components.IComponents.Component
        {
            T1 component1;
            foreach (Entity entity in this.entities)
            {
                component1 = GetComponent<T1>(entity);
                if (component1 != null)
                {
                    yield return (entity, component1);
                }
            }
        }



        public List<Components.IComponents.Component> GetAllEntityComponents(Entity entity)
		{
			if (entityComponents.ContainsKey(entity.GetID())) return entityComponents[entity.GetID()];
			//crash program
			return null;
		}

		public bool hasComponent<T>(Entity entity) where T : Components.IComponents.Component
		{
			List<Components.IComponents.Component> components = new List<Components.IComponents.Component> ();
			if (entityComponents.ContainsKey(entity.GetID()))
			{
				components = entityComponents[entity.GetID()];
			}
			else
			{
				return false;
			}
            return components.OfType<T>().FirstOrDefault() != null;
        }

		public bool hasComponent<T>(uint entityID)
		{
            List<Components.IComponents.Component> components = new List<Components.IComponents.Component>();
            if (entityComponents.ContainsKey(entityID))
            {
                components = entityComponents[entityID];
            }
            else
            {
                return false;
            }
            return components.OfType<T>().FirstOrDefault() != null;
        }
		public void RemoveEntity(Entity entity)
		{
            List<Components.IComponents.Component> list = GetAllEntityComponents(entity);
			while (list.Count > 0)
			{
                RemoveComponent(entity, list.Last());
			}
		}

		
	}
}
