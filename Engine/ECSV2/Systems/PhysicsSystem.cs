using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class PhysicsSystem : ISystem, IUpdatableSystem
    {
        private readonly EntityManager manager;
        public PhysicsSystem()
        {
            manager = EntityManager.GetInstance();
        }

        public void Update(float deltaTime)
        {
            Vector2 previousPosition;
            List<Entity> entities = this.getPhysicsEntity();
            foreach (Entity entity in entities)
            {
                RigidBody2DComponent rigidBody = this.manager.GetComponent<RigidBody2DComponent>(entity);
                //TODO ALl the magic numbers should be somewhere else... this is for test
                if (entity.GetPosition().Y < 400)
                {
                    rigidBody.SetYAcceleration(500);
                }
                if (entity.GetPosition().Y > 400)
                {
                    rigidBody.SetYVelocity(0);
                    rigidBody.SetYAcceleration(0);
                }
                //***********^TEST

                //rigidBody.Update(deltaTime);

            }
        }

        private List<Entity> getPhysicsEntity()
        {
            List<Entity> list = new List<Entity>();
            List<Entity> entities = this.manager.getEntities();
            foreach (Entity entity in entities)
            {
                if (this.manager.hasComponent<RigidBody2DComponent>(entity) && this.manager.hasComponent<ExamplePlayerState>(entity))
                {
                    list.Add(entity);
                }
            }

            return list;
        }
    }
}
