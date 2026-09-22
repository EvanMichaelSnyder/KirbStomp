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
    public class ArrowStormItem : AItem
    {
        private readonly String TEXTURE_NAME = "LinkProjectile";
        private Sprite _sprite;
        private Rectangle _spriteSrc = new Rectangle(327, 2936, 27, 9);
        private float _scale = 2f;
        public ArrowStormItem(Vector2 startPosition) 
        {
            this.Position = startPosition;
            this.Velocity = new Vector2(0, 150);
            this._dimension = new Rectangle((int)startPosition.X, (int)startPosition.Y, _spriteSrc.Width, _spriteSrc.Height);
            this._sprite = new Sprite(AssetPool.GetTexture(TEXTURE_NAME), this._spriteSrc, this._scale);
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
            
        }

        protected override void GiveItemAbillity(Character character)
        {
            //TODO just for test undo comment character.RecieveItemAbillity(new ArrowStormItemAbillity(SceneManager.Get().GetCurrentScene().GetProjectileManager()));
            AItemAbillity test = new ArrowStormItemAbillity(SceneManager.Get().GetCurrentScene().GetProjectileManager(), character);
           
            test.ExectuteAbillity();
            
        }
    }
}
