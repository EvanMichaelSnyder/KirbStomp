using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Classes.GameObjects.ItemAbillity;
using KirbStomp.Scripts.Classes.Projectiles;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.GameObjects.Items
{
    public class HamburgerItem : AItem
    {
        private Sprite _sprite;
        private Rectangle _spriteSrc = new Rectangle(452, 12, 128, 128);
        private float _scale = .5f;
        private float _healAmount = 20;
        public HamburgerItem(Vector2 startPosition) 
        {
            this.Position = startPosition;
            this.Velocity = new Vector2(0, 150);
            this._dimension = new Rectangle((int)startPosition.X, (int)startPosition.Y, 128, 128);
            this._sprite = new Sprite(AssetPool.GetTexture("Items"), this._spriteSrc, this._scale);     
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            this._itemCarrier.HitboxManager.Draw(spriteBatch);
            this._sprite.Draw(spriteBatch, this.Position);
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
            this.Position += this.Velocity * dt;
        }

        protected override void GiveItemAbillity(Character c)
        {
            AItemAbillity aItemAbillity = new HealItemAbillity(c, this._healAmount);
            c.RecieveItemAbillity(aItemAbillity);
            aItemAbillity.ExectuteAbillity();
        }
    }
}
