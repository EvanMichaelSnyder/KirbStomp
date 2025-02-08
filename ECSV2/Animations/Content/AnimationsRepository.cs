using KirbStomp.ECSV2.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace KirbStomp.ECSV2.Animations.Content
{
    internal class AnimationsRepository
    {
        private static Dictionary<string, Animation> animationDictionary;
        private static AnimationsRepository instance;
        public static AnimationsRepository GetInstance()
        {
            if (instance == null) instance = new AnimationsRepository();
            return instance;
        }
        private AnimationsRepository()
        {
            animationDictionary = new();
            LoadFromXML();
        }
        public void LoadFromXML()
        {
			string projectPath = Environment.CurrentDirectory.ToString(); // is in Proj/bin/debug/net8.0, go back three time ../../., Now we'r ein our projectfile file with bin, content, and other coding files
            string filePath = projectPath + "..\\..\\..\\..\\ECSV2\\Animations\\Content\\Animation.XML";

			Debug.WriteLine("CurrentDir: {0}", projectPath);
			XDocument document = XDocument.Load(filePath);

            foreach (XElement animationElement in document.Descendants("Animation"))
            {
                if (!AddAnimationElement(animationElement))
                {
                    Debug.WriteLine("Animation Failed to Load");
                }
            }
        }


        public Animation GetAnimation(string animationName)
        {
            return animationDictionary.TryGetValue(animationName, out var animation) ? animation : animationDictionary["default"];
        }


        // Helper methods for LoadFromXML to make it more readable
        private bool AddAnimationElement(XElement animationElement)
        {
            bool output = false;
            string animationName = animationElement.Attribute("name").Value;
            string animationTextureName = animationElement.Attribute("textureName").Value;
            float animationDuration = float.Parse(animationElement.Attribute("duration").Value);
            bool animationLoops = bool.Parse(animationElement.Attribute("loop").Value);
			XAttribute isDefaultAttribute = animationElement.Attribute("isDefault");
			bool isDefault = isDefaultAttribute != null && bool.Parse(isDefaultAttribute.Value);

			List<Rectangle> animationFrames = new();
            Animation animation;


            foreach (XElement frame in animationElement.Descendants("Frame"))
            {
                animationFrames.Add(GetRectangleFromFrame(frame));
            }
            if (animationName != null && animationTextureName != null)
            {
                animation = new Animation(animationName, animationTextureName,
                    animationFrames, animationDuration / animationFrames.Count, animationLoops);
				
                output = animationDictionary.TryAdd(animationName, animation);
				if (isDefault) animationDictionary["default"] = animation;
            }
            return output;
        }
        private Rectangle GetRectangleFromFrame(XElement frame)
        {
            XElement rectElement = frame.Element("SourceRectangle");
            int x = int.Parse(rectElement.Attribute("x").Value);
            int y = int.Parse(rectElement.Attribute("y").Value);
            int width = int.Parse(rectElement.Attribute("width").Value);
            int height = int.Parse(rectElement.Attribute("height").Value);
            return new Rectangle(x, y, width, height);
        }


    }
}
