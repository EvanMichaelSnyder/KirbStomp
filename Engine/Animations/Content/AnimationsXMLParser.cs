using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KirbStomp.Engine.Animations.Content
{
	internal static class AnimationsXMLParser
	{
		// fileToLoad is local to AnimationsXMLParser
		public static (string, float, Dictionary<string, Animation>) LoadAnimationsFromXML(string fileDirectoryToLoad)
		{
			float scale = 1.0f;
			Dictionary<string, Animation> animationDictionary = new();
			string textureName = "Err";


			if (!TryLoadDocument(fileDirectoryToLoad, out XDocument document)) return (textureName, scale, animationDictionary);
			if (!TryGetXElement(document, "Animations", out XElement animationsElement, fileDirectoryToLoad)) return (textureName, scale, animationDictionary);
			if(!TryGetXElement(animationsElement, "Texture", out XElement textureElement)) return (textureName, scale, animationDictionary);

			(textureName, scale) = TextureElementToData(textureElement);
			foreach (XElement animationElement in animationsElement.Elements("Animation"))
			{
				var (animationName, animation) = GetAnimationFromElement(animationElement, textureName);
				if(!animationDictionary.TryAdd(animationName, animation))
				{
					Debug.WriteLine($"Animation {animationName} conflicts with another animation within the sprite sheet");
					Console.WriteLine($"Animation {animationName} conflicts with another animation within the sprite sheet");
				}
			}


			return (textureName, scale, animationDictionary);
		}
		private static bool TryLoadDocument(string path, out XDocument document)
		{
			document = XDocument.Load(path);
			if(document == null)
			{
				Debug.WriteLine($"Document failed to load from path {path}");
				Console.WriteLine($"Document failed to load from path {path}");
				return false;
			}
			return true;
		}
		private static bool TryGetXElement(XElement root, string childName, out XElement childElement)
		{
			childElement = root.Element(childName);
			if(childElement == null)
			{
				Debug.WriteLine($"{root.Name} has no child XElement {childName}");
				Console.WriteLine($"{root.Name} has no child XElement {childName}");

				return false;
			}
			return true;
		}
		// Document name is purely for debugging purposes, since document doesn't store it's own name,
		// but we can get it through the method it's being called by
		private static bool TryGetXElement(XDocument document, string childName, out XElement childElement, string documentPath="")
		{
			childElement = document.Element(childName);
			if(childElement == null)
			{
				string debugMessage = $"Document given has no child XElement {childName}";
				if (documentPath != "") debugMessage = $"Document from {documentPath} has Errored" + debugMessage;
				Debug.WriteLine(debugMessage);
				Console.WriteLine(debugMessage);

				return false;
			}
			return true;
		}



		private static (string, Animation) GetAnimationFromElement(XElement animationElement, string textureName)
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
			return (name, new Animation(name, textureName, sourceFrames, duration / sourceFrames.Count, doesLoop, frameOffsets));
		}
		private static void FrameAttributesToData(XElement Frame, out Point position, out Point size, out Point offset)
		{
			position = ParseIntoPoint(GetValueOrDefault(Frame, "position", "(0, 0)"));
			size = ParseIntoPoint(GetValueOrDefault(Frame, "size", "(50, 50)"));
			offset = ParseIntoPoint(GetValueOrDefault(Frame, "perFrameOffset", "(0, 0)"));
			//offset = new Point(0, 0);
		}
		private static (string, float, bool, Point) AnimationAttributesToData(XElement animationElement)
		{
			string name = GetValueOrDefault(animationElement, "name", "mario");
			float duration = float.Parse(GetValueOrDefault(animationElement, "duration", "1.0"));
			bool loops = bool.Parse(GetValueOrDefault(animationElement, "loop", "true"));
			// This one is a bit dangerous, as it assumes proper notation of least having a certain format (x, y)
			Point animationOffset = ParseIntoPoint(animationElement.Attribute("animationOffset").Value); 

			return (name, duration, loops, animationOffset);
		}
		private static (string, float) TextureElementToData(XElement textureElement)
		{
			(string, float) output = ("Default", 1.0f);
			output.Item1 = GetValueOrDefault(textureElement, "spriteSheet", "default");
			output.Item2 = float.Parse(GetValueOrDefault(textureElement, "scale", "1.0"));
			return output;
		}



		private static string GetValueOrDefault(XElement element, string name, string defaultOutput)
		{
			string output = ""; // Let program know it went to a defaulted value TODO
			return (output = element.Attribute(name).Value) != null ? output : defaultOutput;
		}
		private static Point ParseIntoPoint(string input)
		{
			string temp = input;
			temp = temp.Replace('(', ' ');
			temp = temp.Replace(')', ' ');
			string[] stringInts = temp.Split(',');
			return new Point(int.Parse(stringInts[0]), int.Parse(stringInts[1]));
		}



	}



	
}
