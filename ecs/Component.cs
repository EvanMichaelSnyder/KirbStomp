using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ecs
{
    public abstract class Component
    {
        //can use to keep track of certain components that may repeat in a gameobject
        private static int IdTracker;
        private int id = -1;
        //parent gameobject, concrete components can use as is protected
        protected GameObject gameObject;
        //returns unique id for component
        public int GetID()
        {
            if (this.id < 0)
            {
                this.id = IdTracker++;
            }
            return this.id;
        }
        //sets components parent game object, so that components can use
        public void setGameObject(GameObject gameObject)
        {
            if (this.gameObject == null)
            {
                this.gameObject = gameObject;
            }
        }
        //update component
        public abstract void Update(float dt);
    }
}
