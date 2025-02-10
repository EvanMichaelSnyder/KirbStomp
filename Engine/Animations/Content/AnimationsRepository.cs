using KirbStomp.Engine.Animations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace KirbStomp.Engine.Animations.Content
{
	internal class AnimationsRepository
	{
		public static Dictionary<string, Animation> animationDictionary;
		private static Dictionary<string, float> perTextureScale;
		private static AnimationsRepository instance;
		public static AnimationsRepository GetInstance()
		{
			if (instance == null) instance = new AnimationsRepository();
			return instance;
		}
		private AnimationsRepository()
		{
			animationDictionary = new();
			perTextureScale = new();
			LoadFromXML();
		}
		public void LoadFromXML([CallerFilePath] string currentFile = "")
		{
			string projectPath = Environment.CurrentDirectory.ToString(); // is in Proj/bin/debug/net8.0, go back three time ../../., Now we'r ein our projectfile file with bin, content, and other coding files
																		  //string filePath = projectPath + "\\Engine\\Animations\\Content\\Animation.XML";
			//string filePath = projectPath + "/Engine/Animations/Content/Animation.XML";
			Debug.WriteLine("CurrentDir: {0}", currentFile);
			string directory = Path.GetDirectoryName(currentFile);
			string xmlPath = Path.Combine(directory, "Animation.XML");
			XDocument document = XDocument.Load(xmlPath);
			
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
		// This parser can be split off into sections and cleaned up overall, for now this is essentially brute force with hard coded strings
        private bool AddAnimationElement(XElement animationElement)
        {
            bool output = false;
			string animationName = GetValueOrDefault(animationElement, "name", "Idle");
			string animationTextureName = GetValueOrDefault(animationElement, "textureName", "mario");
			float animationDuration = float.Parse(GetValueOrDefault(animationElement, "duration", "1,0"));
            bool animationLoops = bool.Parse(GetValueOrDefault(animationElement, "loop", "true"));
			int xAnimationOffset = int.Parse(GetValueOrDefault(animationElement, "xAnimationOffset", "0"));
			int yAnimationOffset = int.Parse(GetValueOrDefault(animationElement, "yAnimationOffset", "0"));
            XAttribute isDefaultAttribute = animationElement.Attribute("isDefault");
            bool isDefault = isDefaultAttribute != null && bool.Parse(isDefaultAttribute.Value);

            List<Rectangle> animationFrames = new();
            Animation animation;
			List<Point> perFrameOffset = new();
			(Rectangle, Point) frameData;
            foreach (XElement frame in animationElement.Descendants("Frame"))
			{
				frameData = GetRectangeAndOffsetFromFrame(frame);
				animationFrames.Add(frameData.Item1);
				perFrameOffset.Add(frameData.Item2);
            }

			perTextureScale.TryAdd(animationTextureName, 1.2f);

            if (animationName != null && animationTextureName != null)
            {
                animation = new Animation(animationName, animationTextureName,
                    animationFrames, animationDuration / animationFrames.Count, animationLoops, perFrameOffset);

                output = animationDictionary.TryAdd(animationName, animation);
                if (isDefault) animationDictionary["default"] = animation;
            }
            return output;
        }

		private string GetValueOrDefault(XElement element, string name, string defaultOutput)
		{
			string output = "";
			return (output = element.Attribute(name).Value) != null ? output : defaultOutput;
		}



        private (Rectangle, Point) GetRectangeAndOffsetFromFrame(XElement frame)
		{
            XElement rectElement = frame.Element("SourceRectangle");
			int x = int.Parse(GetValueOrDefault(rectElement, "x", "0")); 
			int y = int.Parse(GetValueOrDefault(rectElement, "y", "0")); 
			int width = int.Parse(GetValueOrDefault(rectElement, "width", "100")); 
			int height = int.Parse(GetValueOrDefault(rectElement, "height", "100"));
			int xOffset = int.Parse(GetValueOrDefault(rectElement, "xPerFrameOffset", "0"));
			int yOffset = int.Parse(GetValueOrDefault(rectElement, "yPerFrameOffset", "0"));
			return (new Rectangle(x, y, width, height), new Point(xOffset, yOffset));
        }
    }
}
