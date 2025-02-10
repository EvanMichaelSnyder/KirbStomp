using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class PlayerMovementSystem : IUpdatableSystem
    {
        private ECSManager manager;
        private GlobalInputs inputs;
        public PlayerMovementSystem()
        {
            manager = ECSManager.GetInstance();
            inputs = GlobalInputs.GetInstance();
        }
        public void Update(float deltaTime)
        {
            foreach (var (entity, playerControls, playerState) in manager.GetEntitiesWithComponents<PlayerControlsComponent, ExamplePlayerState>())
            {
                playerState.normalMovementDirection.X = Convert.ToInt32(inputs.IsInputPressed(playerControls.moveRightKey)) - Convert.ToInt32(inputs.IsInputPressed(playerControls.moveLeftKey));
                playerState.isWalking = playerState.normalMovementDirection.X != 0;
                playerState.isFalling = manager.GetComponent<RigidBody2DComponent>(entity).velocity.Y != 0;
                playerState.jumped = inputs.IsInputJustPressed(playerControls.jumpKey) && !playerState.isFalling;
                if (playerState.jumped) Debug.WriteLine("Jumped!");
                if (playerState.isWalking) Debug.WriteLine("WALKING, AND I SET THE STATE OF PlayerState Component");
            }
        }
    }
}
