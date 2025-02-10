using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ecs
{
    public class GameObject
    {
        //only add components to this
        private List<Component> components;
        private Vector2 position;
        public GameObject()
        {
            components = new List<Component>();
            position = new Vector2();
        }
        //updates gameobject, and its components
        public void Update(float dt)
        {
            foreach (var component in components)
            {
                component.Update(dt);
            }
        }

        //adds component to go
        public void AddComponent(Component component)
        {
            components.Add(component);
            component.setGameObject(this);
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        //TODO removes and destroy go
        public void Destroy()
        {
            components.Clear();
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 pos)
        {
            position = pos;
        }

        //return null if cant find component with id, return component otherwise... use hasComponentWithID first
        public Component getComponentWithID(int id)
        {
            foreach (Component component in components)
            {
                if (component.GetID() == id)
                {
                    return component;
                }
            }

            return null;
        }
        //return if has component with ID
        public bool hasComponentWithID(int id)
        {
            bool hasID = false;
            foreach (Component component in components)
            {
                if (component.GetID() == id)
                {
                    hasID = true;
                }
            }
            return hasID;
        }

    }
}
