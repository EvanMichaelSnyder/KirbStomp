using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
    internal class SpriteComponent : IECSComponent
    {
        public Texture2D spriteSheet;
        public Rectangle spriteSource;
        public Point spriteDimensions;
        public Color color;
		public float scale;
        public SpriteComponent(Texture2D texture, Rectangle source, Point spriteDimensions, Color color)
        {
            spriteSheet = texture;
            spriteSource = source;
            this.spriteDimensions = spriteDimensions;
            this.color = color;
			this.scale = 1.0f;
        }
    }
}
