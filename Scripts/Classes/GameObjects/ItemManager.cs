using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Projectiles
{
    public class ItemManager
    {
        private List<AItem> _items;
        private List<AItem> _removedPool;

        public ItemManager()
        {
            this._removedPool = new List<AItem>();
            this._items = new List<AItem>();
        }

        public void Update(float dt)
        {
            foreach (AItem go in this._items)
            {
                //go.Update(dt);
            }

            foreach (AItem go in this._removedPool)
            {
                this._items.Remove(go);
            }
            this._removedPool.Clear();




        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AItem go in this._items)
            {
                go.Draw(spriteBatch);
            }
        }

        public void RemoveProjectile(AItem item)
        {
            if (!this._items.Contains(item))
            {
                throw new Exception("projectile cannot be removed, dne: ");
            }
            this._removedPool.Add(item);
        }


        public void AddItem(AItem item)
        {

            this._items.Add(item);
            item.SetItemManager(this);
        }
    }
}
