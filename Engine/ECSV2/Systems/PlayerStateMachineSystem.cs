using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
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
        private ECSManager manager;
        public PlayerStateMachineSystem()
        {
            manager = ECSManager.GetInstance();
        }
        public void Update(float deltaTime)
        {
            foreach (var (entity, playerState, rigidBody) in manager.GetEntitiesWithComponents<ExamplePlayerState, RigidBody2DComponent>())
            {
                rigidBody.velocity.X = playerState.normalMovementDirection.X * playerState.movementVel;
                rigidBody.velocity.Y -= Convert.ToInt32(playerState.jumped) * playerState.jumpVelocity;
            }
        }
    }
}
