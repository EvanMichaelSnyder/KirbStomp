using KirbStomp.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Components
{
    internal class SpriteComponent : IECSComponent
    {
        public Texture2D spriteSheet;
        public Rectangle spriteSource;
        public Color color;
        public SpriteComponent(Texture2D texture, Rectangle source, Color color)
        {
            spriteSheet = texture;
            spriteSource = source;
            this.color = color;
        }
    }
}
