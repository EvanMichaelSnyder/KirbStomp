
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
using KirbStomp.Engine.ECSV2.EntityManagement;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class PlayerMovementSystem : IUpdatableSystem
    {
        private EntityManager manager;
        private GlobalInputs inputs;
        public PlayerMovementSystem()
        {
            manager = EntityManager.GetInstance();
            inputs = GlobalInputs.GetInstance();
        }
        public void Update(float deltaTime)
        {
			foreach (var (entity, playerControls, playerState) in manager.GetEntitiesWithComponents<PlayerControlsComponent, ExamplePlayerState>())
			{
				playerState.SetStartedWalkingLeftState(inputs.IsInputJustPressed(playerControls.moveLeftKey));
				playerState.SetStartedWalkingRightState(inputs.IsInputJustPressed(playerControls.moveRightKey));
				playerState.SetStoppedWalkingState(!inputs.IsInputPressed(playerControls.moveLeftKey) && !inputs.IsInputPressed(playerControls.moveRightKey));
				playerState.UpdateMovementState();
							//playerState.SetJumpState(jumped);
			}
        }
    }
}
