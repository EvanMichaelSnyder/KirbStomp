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
    public class MegaManMetalBlade : AProjectile
    {
        //some variables to stop magic num/string!
        private float _scale = 1.75f;
        private string SPR_NAME = "MegaManProjectile";
        private string ANIM_NAME = "MegaManProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "MetalBlade";
        private float X_SPEED = 400f;
        private float DAMAGE = 5f;
        private float TIME_TO_LIVE = 5f;

        private bool _doneShoot = false;


        private Sprite _sprite;
        private AnimationSystem _animationSystem;


        public MegaManMetalBlade(Vector2 startPos, bool facingRight)
        {
            this.Position = startPos;
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

            //does not get destroy when hitting player
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


            Position += Velocity * dt;

        }
    }
}
