using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
    internal class SpriteComponent : Component
    {
        private Texture2D spriteSheet;
        private Rectangle spriteSource;
        private float width, height;
        private Color color;
        private float scale;
        private float rotation;
        //priotiry of drawing lower goes first
        private int zIndex;

        private float Xoffset, Yoffset;
        public SpriteComponent(Texture2D texture, Rectangle source, float width, float height, Color color, int zIndex = 100, float scale = 1.0f)
        {
            spriteSheet = texture;
            spriteSource = source;
            this.width = width;
            this.height = height;
            this.color = color;
            this.scale = scale;
            this.zIndex = zIndex;
            //TODO implement rotation if wanted
            rotation = 0;
            Xoffset = 0;
            Yoffset = 0;
        }

        public void setOffSet(Vector2 offset)
        {
            Xoffset = offset.X;
            Yoffset = offset.Y;
        }
        public Vector2 getOffSet()
        {
            return new Vector2(Xoffset, Yoffset);
        }

        public void setRotation(float rotation)
        {
            this.rotation = rotation;
        }
        public float getRotation()
        {
            return rotation;
        }

        public void setZIndex(int zindex)
        {
            zIndex = zindex;
        }
        public int getZIndex()
        {
            return zIndex;
        }

        public void setScale(float scale)
        {
            this.scale = scale;
        }

        public float getScale()
        {
            return scale;
        }
        public void setTexture(Texture2D tex)
        {
            spriteSheet = tex;
        }

        public void setSpriteSrc(Rectangle src)
        {
            //dont allow non sprite class to change this using reference
            spriteSource = new Rectangle(src.X, src.Y, src.Width, src.Height);
        }

        public void setSpriteDimensions(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public float getSpriteWidth()
        {
            return width;
        }

        public float getSpriteHeight()
        {
            return height;
        }

        public void setSpriteHeight(float height)
        {
            this.height = height;
        }

        public void setSpriteWidth(float width)
        {
            this.width = width;
        }

        public Color GetColor()
        {
            return color;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }
        public Texture2D GetTexture()
        {
            return spriteSheet;
        }
        public Rectangle GetSpriteSrc()
        {
            return spriteSource;
        }

    }
}
