using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp.Scripts.Classes.Projectiles
{
    public abstract class AItem 
    {
        private ItemManager _itemManager;
        public abstract void Draw(SpriteBatch spriteBatch);

        public void SetItemManager(ItemManager itemManager)
        {
            if (itemManager == null) throw new Exception("no passing null itemManger, you are probaly not allowed to use function");
            if(this._itemManager == null)
            {
                this._itemManager = itemManager;
            }
        }


    }
}
