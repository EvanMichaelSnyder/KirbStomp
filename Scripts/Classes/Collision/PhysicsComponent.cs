
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.Collision
{
    public class PhysicsComponent
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Gravity = new Vector2(0, 10.0f);
    }
}