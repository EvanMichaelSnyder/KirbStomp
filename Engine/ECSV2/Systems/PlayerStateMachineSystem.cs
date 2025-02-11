using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class PlayerStateMachineSystem : IUpdatableSystem
    {
        private EntityManager manager;
        public PlayerStateMachineSystem()
        {
            manager = EntityManager.GetInstance();
        }
        public void Update(float deltaTime)
        {
            foreach (var (entity, playerState, rigidBody) in manager.GetEntitiesWithComponents<ExamplePlayerState, RigidBody2DComponent>())
            {
				rigidBody.SetVelocity(new(playerState.normalMovementDirection.X * playerState.movementVel,
					rigidBody.GetYVelocity() + Convert.ToInt32(playerState.jumped) * playerState.jumpVelocity));
            }
        }
    }
}
