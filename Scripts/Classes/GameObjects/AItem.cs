using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Collision;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Projectiles
{
    public abstract class AItem : CollisionObject
    {
        private ItemManager _itemManager;
        //dont like default dimen value
        protected Rectangle _dimension = new Rectangle(200,200,16,16);
        protected ItemCarrier _itemCarrier;
        protected bool drawHitbox = false;
        public abstract void Draw(SpriteBatch spriteBatch);

        public void SetItemManager(ItemManager itemManager)
        {
            if (itemManager == null) throw new Exception("no passing null itemManger, you are probaly not allowed to use function");
            if(this._itemManager == null)
            {
                this._itemManager = itemManager;
            }
        }

        public void UpdateCollider()
        {
            this._dimension.X = (int)this.Position.X;
            this._dimension.Y = (int)this.Position.Y;
            this._itemCarrier.HitboxManager.basicUpdateHitbox(this._dimension);
           
        }

        public void ProvideItemCarriers()
        {
            _itemCarrier = new ItemCarrier { Parent = this };
            Carriers.Add(_itemCarrier);
        }


        public abstract void Update(float dt);

        public abstract void RegisterCollider();

        public void Destroy()
        {
           this._itemManager.RemoveItem(this);
        }


        public void BodyCollide(CollisionObject obj, CollisionContext context)
        {
            if(context.Other is Character)
            {
                this.GiveItemAbillity((Character)context.Other);
                this.Destroy();
               
            }
        }

        public void TouchGround(CollisionObject obj, CollisionContext context)
        {
            this.Position.Y -= context.Intersection.Height - 1;
            this.Velocity.Y = 0;
            
            
        }

        protected abstract void GiveItemAbillity(Character character);

    }
}
