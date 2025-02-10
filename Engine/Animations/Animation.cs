using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
namespace KirbStomp.Engine.Animations
{
    internal class Animation
    {
        public string animationName { get; set; }
        public string textureName { get; set; }
        public Texture2D spriteSheet { get; private set; }
        public List<Rectangle> sourceFrames { get; set; }
		public List<Point> perFrameOffset { get; set; }
        public int numberOfFrames { get; set; }
        public float frameDuration { get; set; }
        public bool loop { get; set; }

        public Animation() { }
        public Animation(string name, string texture, List<Rectangle> frames, float perFrameDuration, bool loops, List<Point> perFrameOffset)
        {
            animationName = name;
            textureName = texture;
            sourceFrames = frames;
            frameDuration = perFrameDuration;
            loop = loops;
            numberOfFrames = sourceFrames.Count;
			this.perFrameOffset = perFrameOffset;
        }

        public void Load(ContentManager content)
        {
			try
			{
			spriteSheet = content.Load<Texture2D>(textureName);
			} catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Debug.WriteLine(e.Message);
				spriteSheet = content.Load<Texture2D>("mario"); // Default
			}

        }
    }
}
