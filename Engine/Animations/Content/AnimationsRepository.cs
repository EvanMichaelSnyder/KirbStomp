using KirbStomp.Engine.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace KirbStomp.Engine.Animations.Content
{
	internal class AnimationsRepository
	{
		public static Dictionary<string, Animation> animationDictionary;
		public static Dictionary<string, float> perTextureScale;
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
			LoadAnimationsFromXML("mario.XML");
			LoadAnimationsFromXML("Items.XML");
			LoadAnimationsFromXML("NewMario.XML");
			LoadAnimationsFromXML("Link.XML");
			LoadAnimationsFromXML("MegaMan.XML");

		}
		public void LoadAnimationsFromXML(string fileToLoad, [CallerFilePath] string currentFile = "")
		{
			string directory = Path.GetDirectoryName(currentFile); // path w/ AnimationRepository.cs
			string xmlPath = Path.Combine(directory, fileToLoad); // path to dir w/ AnimationRepository.cs
			string textureName;
			float textureScale;
			// All SpriteSheet.XML files are in this dir
			XDocument document = XDocument.Load(xmlPath);
			if(document == null)
			{
				Debug.WriteLine($"LoadFromXML failed to load {fileToLoad}/{currentFile}");
				return;
			}
			XElement animationsElement = document.Element("Animations");
			// all Animations must have Texture child with required attributes
			foreach (XElement elements in animationsElement.Elements())
			{
				Debug.WriteLine(elements.Name);
			}	
			XElement textureElement = animationsElement.Element("Texture");
			if(textureElement == null)
			{
				Debug.WriteLine($"Animations has no required child Texture in {fileToLoad}/{currentFile}");
				return;
			}
			(textureName, textureScale) = TextureElementToData(textureElement);
			if(!perTextureScale.TryAdd(textureName, textureScale))
			{
				Debug.WriteLine($"Texture {textureName} has already been added. \n Please add animation to appropriate file");
			}
			foreach (XElement animationElement in animationsElement.Elements("Animation"))
            {
                if (!AddAnimationElement(animationElement, textureName))
                {
                    Debug.WriteLine("Animation Failed to Load");
                }
            }
        }


        public Animation GetAnimation(string animationName)
        {
            return animationDictionary.TryGetValue(animationName, out var animation) ? animation : animationDictionary.First().Value;
        }


		// Helper methods for LoadFromXML to make it more readable
		// This parser can be split off into sections and cleaned up overall, for now this is essentially brute force with hard coded strings
		private bool AddAnimationElement(XElement animationElement, string textureName)
		{
			Animation animation = GetAnimationFromElement(animationElement, textureName);
			return animationDictionary.TryAdd(animation.animationName, animation);
		}
		
		private Animation GetAnimationFromElement(XElement animationElement, string textureName)
		{
			List<Rectangle> sourceFrames = new();
			List<Point> frameOffsets = new();
			Point position = new();
			Point size = new();
			Point perFrameOffset = new();

			var (name, duration, doesLoop, animationOffset) = AnimationAttributesToData(animationElement); // Has default outputs
			foreach (XElement frame in animationElement.Elements("Frame"))
			{
				FrameAttributesToData(frame, out position, out size, out perFrameOffset);
				sourceFrames.Add(new Rectangle(position, size));
				frameOffsets.Add(perFrameOffset + animationOffset);
			}
			return new Animation(name, textureName, sourceFrames, duration / sourceFrames.Count, doesLoop, frameOffsets);
		}
		
		private (string, float) TextureElementToData(XElement textureElement)
		{
			(string, float) output = ("Default", 1.0f);
			output.Item1 = GetValueOrDefault(textureElement, "spriteSheet", "default");
			output.Item2 = float.Parse(GetValueOrDefault(textureElement, "scale", "1.0"));
			return output;
		}
		private (string, float, bool, Point) AnimationAttributesToData(XElement animationElement)
		{
			string name = GetValueOrDefault(animationElement, "name", "mario");
			float duration = float.Parse(GetValueOrDefault(animationElement, "duration", "1.0"));
			bool loops = bool.Parse(GetValueOrDefault(animationElement, "loop", "true"));
			Point animationOffset = ParseIntoPoint(animationElement.Attribute("animationOffset").Value);

			return (name, duration, loops, animationOffset);
		}
		private void FrameAttributesToData(XElement Frame, out Point position, out Point size, out Point offset)
		{
			position = ParseIntoPoint(GetValueOrDefault(Frame, "position", "(0, 0)"));
			size = ParseIntoPoint(GetValueOrDefault(Frame, "size", "(50, 50)"));
			offset = ParseIntoPoint(GetValueOrDefault(Frame, "perFrameOffset", "(0, 0)"));
		}

		private string GetValueOrDefault(XElement element, string name, string defaultOutput)
		{
			string output = "";
			return (output = element.Attribute(name).Value) != null ? output : defaultOutput;
		}

		private Point ParseIntoPoint(string input)
		{
			string temp = input;
			temp = temp.Replace('(', ' ');
			temp = temp.Replace(')', ' ');
			string[] stringInts = temp.Split(',');
			return new Point(int.Parse(stringInts[0]), int.Parse(stringInts[1]));
		}
    }
}
