using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Inputs.Controllers;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp
{
    internal class BlockSprites
    {
        private Texture2D block;
        Dictionary<string, Rectangle> blockSprites;
        IController keyboard;

        public BlockSprites(Texture2D block)
        {
            this.block = block;
            blockSprites = new Dictionary<string, Rectangle>()
            {
                {"BrickBlock", new Rectangle(0, 0, 72, 80) },
                {"SteelBlock", new Rectangle(72, 0, 72, 80) },
                {"WoodBlock", new Rectangle(144, 0, 72, 80) },
                {"PipeBlock", new Rectangle(216, 0, 72, 80) },
                {"DirtBlock", new Rectangle(288, 0, 72, 80) }
            };
            //keyboard = new KeyboardController();
        }

        public void UpdateLeft()
        {
            
        }
        public void UpdateRight()
        {

        }
    }
}
