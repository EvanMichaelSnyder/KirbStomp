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
    public class Bomb : AProjectile
    {
        //the magic stuff
        private readonly float DAMAGE = 20;
        private readonly float EXPLODE_TIME = .8f;
        private readonly float THROW_SPEED = 200f;
        private float GRAVITY = 300f;
        private readonly String TEXTURE_NAME = "LinkProjectile";
        private string ANIM_NAME = "LinkProjectileTransparentSpriteSheet";
        private string ANIM_STATE_AIR = "BombAir";
        private string ANIM_STATE_DEATH = "BombDeath";
        private string ANIM_STATE_SHOOT = "BombShoot";
        private int EXPLODE_HEIGHT = 30;
        private int EXPLODE_WIDTH = 30;
        private int EXPLODE_OFFSET = 10;
        private int WIDTH = 20;
        private int HEIGHT = 20;
        //neccesity stuff
        private Sprite _sprite;
        private AnimationSystem _animationSystem;
        private bool _hasBegunDeath = false;
        private float _shootTimer = 1f;
        private float _deathTimer = .75f;
        private bool _hasBegunShoot = false;
        private float _scale = 1f;


        public Bomb(Vector2 startPos, bool facingRight) 
        {
            this.Position = startPos;
            this.Velocity = new Vector2(THROW_SPEED, 0);
            this._sprite = new Sprite(AssetPool.GetTexture(TEXTURE_NAME), new Rectangle()/** doesnt matter, animate overwrite **/, _scale);
            this._animationSystem = new AnimationSystem(_sprite);
            if (!facingRight)
            {
                this.Velocity.X *= -1;
                this._sprite.FlipTextureX(true);
            }
            

            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_SHOOT));
            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_AIR));
            this._animationSystem.AddAnimation(AssetPool.GetAnimation(ANIM_NAME, ANIM_STATE_DEATH));

            this._animationSystem.SetAnimation(ANIM_STATE_AIR);

            this._dimension = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(WIDTH * _scale),(int)(_scale * HEIGHT));

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, Position);
            if (_hasBegunDeath)
            {
                this._attackCarrier.HitboxManager.Draw(spriteBatch);
            }
            if (this.DrawHitbox) {
                this._bodyCarrier.HitboxManager.Draw(spriteBatch);
            }  
        }

        public override void RegisterCollider()
        {
            this._attackCarrier.SetDamage(DAMAGE);
            this._bodyCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
            this._attackCarrier.IsDisabled = true;

            RegisterCollisionResponse(HitboxTypeEnum.Body,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => HitGround(obj, ctx));

        }

        public void HitGround(CollisionObject obj, CollisionContext context)
        {
            if (!_hasBegunShoot)
            {
                this._animationSystem.SetAnimation(ANIM_STATE_SHOOT);
            }
            this._hasBegunShoot = true;
            this.Position.Y -= context.Intersection.Height;
            this.Velocity.X = 0;
            this.Velocity.Y = 0;
            

        }

        public override void Update(float dt)
        {
            this._animationSystem.Animate(dt);

            this._dimension.Width = (int)(_scale * _sprite.GetSrcRectangle().Width);
            this._dimension.Height = (int)(_scale * _sprite.GetSrcRectangle().Height);

            this._yOffSetCollider = this._sprite.GetYOffset();
            this._xOffSetCollider = this._sprite.GetXOffset();
            
            if (this._hasBegunDeath) {
                this._deathTimer -= dt;
                if (this._deathTimer < 0)
                {
                    this.Destroy();
                }
            }
            else if (this._hasBegunShoot)
            {
                this._shootTimer -= dt;
                if(this._shootTimer < 0)
                {//begin explosion
                    this._hasBegunDeath = true;
                    this._bodyCarrier.IsDisabled = true;
                    this._attackCarrier.IsDisabled = false;
                    this._dimension.Width = (int)(EXPLODE_WIDTH * _scale);
                    this._dimension.Height = (int)(EXPLODE_HEIGHT * _scale);
                    this.GRAVITY = 0;
                    this.Velocity.Y = 0;
                    this._animationSystem.SetAnimation(ANIM_STATE_DEATH);
                    this.Position.Y -= EXPLODE_OFFSET;
                    
                }
            }

            this.Velocity.Y += GRAVITY * dt;
            this.Position += Velocity * dt;
        }


    }
}
