using KirbStomp.Engine.ECSV2.Components.IComponents;
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
		//higher number z index draws on top.
		private int zIndex;

		private float Xoffset, Yoffset;
		public SpriteComponent()
		{	
			spriteSheet = default;
			spriteSource = new();
			this.width = 0;
			this.height = 0;
			this.color = Color.White;
			this.scale = 1.0f;
			this.zIndex = 0;
			//TODO implement rotation if wanted
			rotation = 0;
			Xoffset = 0;
			Yoffset = 0;

		}
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

		public void SetOffSet(Vector2 offset)
		{
			Xoffset = offset.X;
			Yoffset = offset.Y;
		}
		public Vector2 GetOffSet()
		{
			return new Vector2(Xoffset, Yoffset);
		}

		public void SetRotation(float rotation)
		{
			this.rotation = rotation;
		}
		public float GetRotation()
		{
			return rotation;
		}

		public void SetZIndex(int zindex)
		{
			zIndex = zindex;
		}
		public int GetZIndex()
		{
			return zIndex;
		}

		public void SetScale(float scale)
		{
			this.scale = scale;
		}

		public float GetScale()
		{
			return scale;
		}
		public void SetTexture(Texture2D tex)
		{
			spriteSheet = tex;
		}

		public void SetSpriteSource(Rectangle src)
		{
			//dont allow non sprite class to change this using reference
			spriteSource = new Rectangle(src.X, src.Y, src.Width, src.Height);
		}

		public void SetSpriteDimensions(float width, float height)
		{
			this.width = width;
			this.height = height;
		}

		public float GetSpriteWidth()
		{
			return width;
		}

		public float GetSpriteHeight()
		{
			return height;
		}

		public void SetSpriteHeight(float height)
		{
			this.height = height;
		}

		public void SetSpriteWidth(float width)
		{
			this.width = width;
		}

		public Color GetColor()
		{
			return color;
		}

		public void SetColor(Color color)
		{
			this.color = color;
		}
		public Texture2D GetTexture()
		{
			return spriteSheet;
		}
		public Rectangle GetSpriteSource()
		{
			return spriteSource;
		}

	}
}