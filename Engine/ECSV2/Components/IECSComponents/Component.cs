using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;

namespace KirbStomp.Engine.ECSV2.Components.IECSComponents
{
    public abstract class Component
    {
        //can use to keep track of certain components that may repeat in a gameobject
        private static int IdTracker;
        private int id = -1;
        //parent gameobject, concrete components can use as is protected
        protected Entity entity;
        //returns unique id for component
        public int GetID()
        {
            if (id < 0)
            {
                id = IdTracker++;
            }
            return id;
        }
        //sets components parent entity, so that components can use it
        public void setEntity(Entity entity)
        {
            //components probaly shouldn't change their entity
            if (this.entity == null)
            {
                this.entity = entity;
            }
        }
    }
}
