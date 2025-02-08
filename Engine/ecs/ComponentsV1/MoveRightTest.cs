using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ecs;

namespace KirbStomp.Engine.ecs.ComponentsV1
{
    //test class, just moves game object right, shows potential of ecs
    public class MoveRightTest : Component
    {
        public MoveRightTest() { }

        public override void Update(float dt)
        {
            Vector2 newPos = gameObject.getPosition();
            newPos.X += 30 * dt;
            gameObject.setPosition(newPos);
        }
    }
}
