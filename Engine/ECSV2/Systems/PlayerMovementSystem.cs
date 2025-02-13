
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
			int movingLeft, movingRight;
			bool jumped;
			foreach (var (entity, playerControls, playerState) in manager.GetEntitiesWithComponents<PlayerControlsComponent, ExamplePlayerState>())
			{
				movingLeft = Convert.ToInt32(inputs.IsInputPressed(playerControls.moveLeftKey));
				movingRight = Convert.ToInt32(inputs.IsInputPressed(playerControls.moveRightKey));
				jumped = inputs.IsInputJustPressed(playerControls.jumpKey);
				playerState.SetWalkingDirection(new Vector2(movingRight - movingLeft, 0.0f));
				//playerState.SetJumpState(jumped);
			}
        }
    }
}
