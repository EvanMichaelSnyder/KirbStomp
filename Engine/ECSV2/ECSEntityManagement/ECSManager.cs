using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.ECSEntityManagement
{
    internal class ECSManager
    {
        private readonly Dictionary<uint, List<IECSComponent>> entityComponents;
        private readonly Dictionary<Type, List<IECSComponent>> componentsPool;
        private static ECSManager instance;
        private ECSManager()
        {
            entityComponents = new();
            componentsPool = new();
        }
        public static ECSManager GetInstance()
        {
            if (instance == null) instance = new ECSManager();
            return instance;
        }
        public ECSEntity CreateEntity()
        {
            ECSEntity entity = new ECSEntity();
            entityComponents[entity.GetID()] = new();
            return entity;
        }

        // T is an implementation of type IECSComponent, therefore for each unique IECSComponent type added to a component,
        // therefore for each implementation of IECSComponent we have, we have a component pool of each instance added to the pool
        public bool AddComponent<T>(ECSEntity entity, T component) where T : IECSComponent
        {
            if (!entityComponents.ContainsKey(entity.GetID())) return false;
            entityComponents[entity.GetID()].Add(component);

            if (!componentsPool.ContainsKey(typeof(T))) componentsPool.Add(typeof(T), new());
            componentsPool[typeof(T)].Add(component);

            return true;
        }


        public T GetComponent<T>(ECSEntity entity) where T : IECSComponent
        {
            List<IECSComponent> components;
            if (entityComponents.TryGetValue(entity.GetID(), out components))
            {
                return components.OfType<T>().FirstOrDefault(); // Either the first occurance of concrete type T, or default T type
            }
            return default;
        }



        public List<IECSComponent> GetAllEntityComponents(ECSEntity entity)
        {
            if (entityComponents.ContainsKey(entity.GetID())) return entityComponents[entity.GetID()];
            return new();
        }



        public IEnumerable<(ECSEntity, T1, T2)> GetEntitiesWithComponents<T1, T2>()
            where T1 : IECSComponent where T2 : IECSComponent
        {
            ECSEntity entity;
            T1 component1;
            T2 component2;
            foreach (var entityId in entityComponents.Keys)
            {
                entity = new ECSEntity();
                entity.SetID(entityId);
                component1 = GetComponent<T1>(entity);
                component2 = GetComponent<T2>(entity);
                if (component1 != null && component2 != null)
                {
                    yield return (entity, component1, component2);
                }
            }
        }



        public IEnumerable<(ECSEntity, T1)> GetEntitiesWithComponent<T1>()
            where T1 : IECSComponent
        {
            ECSEntity entity;
            T1 component1;
            foreach (var entityId in entityComponents.Keys)
            {
                entity = new();
                entity.SetID(entityId);
                component1 = GetComponent<T1>(entity);
                if (component1 != null)
                {
                    yield return (entity, component1);
                }
            }
        }
    }
}
