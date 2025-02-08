using KirbStomp.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Components
{
	internal class PlayerControls : IECSComponent, IUpdatableECSComponent
	{
		public float movementVelocity;
		public Keys moveLeftKey;
		public Keys MoveRightKey;

		public PlayerControls(float defaultMovementVel = 100, Keys defaultMoveLeftKey = Keys.A, Keys defaultMoveRightKey = Keys.D)
		{
			movementVelocity = defaultMovementVel;
			moveLeftKey = defaultMoveLeftKey;
			MoveRightKey = defaultMoveRightKey;
		}
		public void Update(float deltaTime)
		{
			// Movement can be handled here depending on if we think components should have access to global inputs
		}
	}
}
