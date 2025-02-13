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
    internal class ExamplePlayerPhysicsSystem : ISystem, IUpdatableSystem
    {
        private readonly EntityManager manager;
        public ExamplePlayerPhysicsSystem()
        {
            manager = EntityManager.GetInstance();
        }

        public void Update(float deltaTime)
        {
            Vector2 previousPosition;
            foreach (var (entity, rigidBody, playerState) in manager.GetEntitiesWithComponents<RigidBody2DComponent, ExamplePlayerState>())
            {
                previousPosition = entity.GetPosition();

                // ALl the magic numbers should be somewhere else
                if (entity.GetPosition().Y < 400)
                {
                    rigidBody.SetYAcceleration(980);
                }
                if (entity.GetPosition().Y > 400)
                {
					entity.SetPosition(entity.GetPosition().X, 400);
					rigidBody.SetYVelocity(0);
					rigidBody.SetYAcceleration(0);
				}
            }
        }
    }
}
