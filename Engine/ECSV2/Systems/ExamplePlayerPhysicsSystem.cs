using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
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
        private readonly ECSManager manager;
        public ExamplePlayerPhysicsSystem()
        {
            manager = ECSManager.GetInstance();
        }

        public void Update(float deltaTime)
        {
            Vector2 previousPosition;
            foreach (var (entity, rigidBody, playerState) in manager.GetEntitiesWithComponents<RigidBody2DComponent, ExamplePlayerState>())
            {
                previousPosition = rigidBody.position;
                rigidBody.Update(deltaTime);

                // ALl the magic numbers should be somewhere else
                if (rigidBody.position.Y < 400)
                {
                    rigidBody.acceleration.Y = 500;
                }
                if (rigidBody.position.Y > 400)
                {
                    rigidBody.position.Y = 400;
                    rigidBody.velocity.Y = 0;
                    rigidBody.acceleration.Y = 0;
                }
            }
        }
    }
}
