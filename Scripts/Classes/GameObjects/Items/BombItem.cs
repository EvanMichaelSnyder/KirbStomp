using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.GameObjects.ItemAbillity;
using KirbStomp.Scripts.Classes.Projectiles;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.GameObjects.Items
{
    public class BombItem : AItem
    {
        private readonly string TEXTURE_NAME = "LinkProjectile";
        private float _scale = 2f;
        private Sprite _sprite;
        private Rectangle SPRITE_SOURCE = new Rectangle(390, 3273, 18, 17);
        
        public BombItem(Vector2 pos)
        {
            this.Position = pos;
            this._sprite = new Sprite(AssetPool.GetTexture(TEXTURE_NAME), SPRITE_SOURCE, _scale);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            this._sprite.Draw(spriteBatch, this.Position);
            if(this.drawHitbox)
            {
                this._itemCarrier.HitboxManager.Draw(spriteBatch);
            }
        }

        public override void RegisterCollider()
        {
            this._dimension.Height = (int)(this._scale * this._dimension.Height);
            this._dimension.Width = (int)(this._scale * this._dimension.Width);
            this._itemCarrier.HitboxManager.basicUpdateHitbox(this._dimension);

            RegisterCollisionResponse(HitboxTypeEnum.Item,
                                    HitboxTypeEnum.Platform,
                                    (obj, ctx) => TouchGround(obj, ctx));
            RegisterCollisionResponse(HitboxTypeEnum.Item,
                                    HitboxTypeEnum.Body,
                                    (obj, ctx) => BodyCollide(obj, ctx));
        }

        public override void Update(float dt)
        {
            //prob do nothing
        }

        protected override void GiveItemAbillity(Character character)
        {
            AItemAbillity abill = new BombFieldAbillity(SceneManager.Get().GetCurrentScene().GetProjectileManager(),character);
            abill.ExectuteAbillity();
        }
    }
}
