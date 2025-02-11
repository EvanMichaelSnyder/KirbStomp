using KirbStomp.Engine.ECSV2.Components.IECSComponents;
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
        public Keys moveLeftKey;
        public Keys moveRightKey;
        public Keys jumpKey;
        public Keys crouchKey;


        public PlayerControlsComponent(Keys defaultMoveLeftKey = Keys.A, Keys defaultMoveRightKey = Keys.D)
        {
            moveLeftKey = defaultMoveLeftKey;
            moveRightKey = defaultMoveRightKey;
            jumpKey = Keys.Space;
        }
        public void Update(float deltaTime)
        {
            // Movement can be handled here depending on if we think components should have access to global inputs
        }
    }
}
