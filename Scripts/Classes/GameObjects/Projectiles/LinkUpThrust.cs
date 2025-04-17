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
    public class LinkUpThrust : AProjectile
    {
        //some variables to stop magic num/string!
        private float _scale = 1f;
        private string SPR_NAME = "LinkProjectile";
        private string ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private string ANIM_STATE_SHOOT = "UpAerialThrustShoot";
        private string ANIM_STATE_AIR = "UpAerialThrustAir";
        private int WIDTH = 32;
        private int HEIGHT = 48;
        private float Y_SPEED = -600f;
        private float DAMAGE = 20f;
        private float TIME_TO_LIVE = 5f;

        private bool _doneShoot = false;


        private Sprite _sprite;
        private AnimationSystem _animationSystem;


        public LinkUpThrust(Vector2 startPos, bool facingRight)
        {
            this.Position = startPos;
            Texture2D texture = AssetPool.GetTexture(SPR_NAME);
            this._sprite = new Sprite(texture, new Rectangle()/*doesnt matter, anim will change*/, _scale);
            this._animationSystem = new AnimationSystem(_sprite);

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_SHOOT));
            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_AIR));
            this._animationSystem.SetAnimation(ANIM_STATE_SHOOT);

            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(_scale * WIDTH), (int)(_scale * HEIGHT));

            this.Velocity = new Vector2(0, Y_SPEED);

            if (!facingRight)
            {
                this._sprite.FlipTextureX();
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
            if (this.DrawHitbox) {
                this._bodyCarrier.HitboxManager.Draw(spriteBatch);
            }  

        }

        public void HitPlayer(CollisionObject obj, CollisionContext context)
        {
            this.Destroy();
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

        public override void Update(float dt)
        {
            this._animationSystem.Animate(dt);

            this._dimension.Width = (int)(_scale * _sprite.GetSrcRectangle().Width);
            this._dimension.Height = (int)(_scale * _sprite.GetSrcRectangle().Height);

            this._yOffSetCollider = this._sprite.GetYOffset();
            this._xOffSetCollider = this._sprite.GetXOffset();
            
            TIME_TO_LIVE -= dt;
            if (TIME_TO_LIVE < 0)
            {
                this.Destroy();
                return;
            }

            if (!_doneShoot)
            {
                if (this._animationSystem.IsAnimationDone())
                {
                    this._doneShoot = true;
                    this._animationSystem.SetAnimation(ANIM_STATE_AIR);
                    this._dimension.Width = (int)(this._sprite.GetSrcRectangle().Width * _scale);
                    this._xOffSetCollider = this._sprite.GetXOffset();
                }
            }


            Position += Velocity * dt;

        }
    }
}
