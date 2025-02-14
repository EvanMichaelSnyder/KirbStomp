using KirbStomp.Engine.ECSV2.Components.IComponents;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class PlayerControlsComponent : Component, IUpdate
	{
		// These should be set to private with get setters later
		public Keys attack1Key { get; private set; }
		public Keys attack2Key {get; private set;}
		public Keys moveLeftKey {get; private set;}
		public Keys moveRightKey {get; private set;}
		public Keys jumpKey {get; private set;}
		public Keys crouchKey {get; private set;}
		public Keys selfDamageKey {get; private set;}


		public PlayerControlsComponent(Keys defaultMoveLeftKey = Keys.A, Keys defaultMoveRightKey = Keys.D)
		{
			moveLeftKey = defaultMoveLeftKey;
			moveRightKey = defaultMoveRightKey;
			jumpKey = Keys.Space;
			attack1Key = Keys.Z;
			attack2Key = Keys.N;
			selfDamageKey = Keys.E;
		}
		public void Update(float deltaTime)
		{
			// Movement can be handled here depending on if we think components should have access to global inputs
		}
	}
}