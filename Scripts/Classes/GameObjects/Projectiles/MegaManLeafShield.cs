using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.GameObjects.Projectiles
{
    public class MegaManLeafShield : AProjectile
    {
        //some variables to stop magic num/string!
        private float SCALE = 1.75f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "LeafShield";
        private float X_SPEED = 150f;
        private float DAMAGE = 15f;
        private float TIME_TO_LIVE = 5f;

        //variable for calculating circular motion
        private float RADIUS = 50f;             // Radius of the circular path
        private float ANGULAR_SPEED = 10f;      // Speed of rotation (radians per second)
        private float _currentAngle = 20f;      // Current angle (in radians)
        private int _rotationDirection = 1;     // 1 for clockwise, -1 for counterclockwise
        private Vector2 center;

        public MegaManLeafShield(Vector2 startPos, bool facingRight)
        {
            this.center = startPos;
            this._scale = SCALE;
            this._spriteName = SPR_NAME;
            this._animName = ANIM_NAME;
            this._animState = ANIM_STATE_SHOOT;
            this._damage = DAMAGE;
            this._timeToLive = TIME_TO_LIVE;
            SetVelocity(new Vector2(X_SPEED, 0));

            SetupAnimation(startPos, facingRight);

            if (!facingRight)
            {
                this._rotationDirection = -1;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            center += Velocity * dt;

            // increment the angle based on the rotation direction and speed
            _currentAngle += _rotationDirection * ANGULAR_SPEED * dt;

            // setting the angle limit to [0, 2PI]
            if (_currentAngle > 2 * (float)Math.PI)
            {
                _currentAngle -= 2 * (float)Math.PI;
            } else if (_currentAngle < 0)
            {
                _currentAngle += 2 * (float)Math.PI;
            }

            // calculate the new position based on the center and angle
            Position.X = center.X + RADIUS * (float)Math.Cos(_currentAngle);
            Position.Y = center.Y + RADIUS * (float)Math.Sin(_currentAngle);

        }
    }
}
