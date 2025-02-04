using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ecs
{
    public class GameObject
    {
        //only add components to this
        private List<Component> components;
        private Vector2 position;
        public GameObject() 
        {
            this.components = new List<Component>();
            this.position = new Vector2();
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
            this.components.Add(component);
            component.setGameObject(this);
        }

        public void RemoveComponent(Component component)
        {
            this.components.Remove(component);
        }

        //TODO removes and destroy go
        public void Destroy()
        {
            this.components.Clear();
        }

        public Vector2 getPosition()
        {
            return this.position;
        }

        public void setPosition(Vector2 pos)
        {
            this.position = pos;
        }

        //return null if cant find component with id, return component otherwise... use hasComponentWithID first
        public Component getComponentWithID(int id)
        {
            foreach (Component component in this.components)
            {
                if(component.GetID() == id)
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
            foreach (Component component in this.components)
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
