using ECSV2.Systems.ISystems;
using KirbStomp.ECSV2.Components;
using KirbStomp.ECSV2.Components.IECSComponents;
using KirbStomp.ECSV2.ECSEntities;
using KirbStomp.Inputs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2
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
            foreach (var (entity, playerControls, playerState) in manager.GetEntitiesWithComponents<PlayerControls, ExamplePlayerState>())
            {
                playerState.isWalking = inputs.IsInputPressed(playerControls.moveLeftKey) ^ inputs.IsInputPressed(playerControls.MoveRightKey);
                if (playerState.isWalking) Debug.WriteLine("WALKING, AND I SET THE STATE OF PlayerState Component");
            }
        }
    }
}
