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
        private float _scale = 1.75f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "LeafShield";
        private float X_SPEED = 150f;
        private float DAMAGE = 5f;
        private float TIME_TO_LIVE = 5f;

        //variable for calculating circular motion
        private float RADIUS = 50f;             // Radius of the circular path
        private float ANGULAR_SPEED = 10f;      // Speed of rotation (radians per second)
        private float _currentAngle = 20f;      // Current angle (in radians)
        private int _rotationDirection = 1;     // 1 for clockwise, -1 for counterclockwise
        private Vector2 center;

        private Sprite _sprite;
        private AnimationSystem _animationSystem;


        public MegaManLeafShield(Vector2 startPos, bool facingRight)
        {
            this.Position = startPos;
            this.center = startPos;
            Texture2D texture = AssetPool.GetTexture(SPR_NAME);
            this._sprite = new Sprite(texture, new Rectangle()/*doesnt matter, anim will change*/, _scale);
            this._animationSystem = new AnimationSystem(_sprite);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_SHOOT));
            this._animationSystem.SetAnimation(ANIM_STATE_SHOOT);

            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(_scale * _sprite.GetSrcRectangle().Width), (int)(_scale * _sprite.GetSrcRectangle().Height));

            this.Velocity = new Vector2(X_SPEED, 0);
            if (!facingRight)
            {
                this.Velocity.X *= -1f;
                this._sprite.FlipTextureX();
                this._rotationDirection = -1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
            _bodyCarrier.HitboxManager.Draw(spriteBatch);

        }

        public override void RegisterCollider()
        {
            this._attackCarrier.SetDamage(DAMAGE);
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            RegisterCollisionResponse(HitboxTypeEnum.Attack,
                                    HitboxTypeEnum.Body,
                                    (obj, ctx) => HitPlayer(obj, ctx));

        }

        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
            this.Destroy();
        }

        public override void Update(float dt)
        {
            this._animationSystem.Animate(dt);
            TIME_TO_LIVE -= dt;
            if (TIME_TO_LIVE < 0)
            {
                this.Destroy();
                return;
            }


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
