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
		private List<string> animationFileNames;
		private Dictionary<string, (float, Dictionary<string, Animation>)> spriteSheetDataDictionary;
		public AnimationsRepository(List<string> animationXMLFiles)
		{
			this.animationFileNames = animationXMLFiles;
			this.spriteSheetDataDictionary = new();
		}
		public void InitializeAnimations()
		{
			string textureName;
			float textureScale;
			Dictionary<string, Animation> animationsDictionary; // TODO there's ambiguity rn
			string path;
			foreach (string name in animationFileNames)
			{
				path = GetRelativeFilePath(name + ".XML");
				(textureName, textureScale, animationsDictionary) = AnimationsXMLParser.LoadAnimationsFromXML(path);
				spriteSheetDataDictionary.Add(textureName, (textureScale, animationsDictionary));
			}
		}
		private string GetRelativeFilePath(string file, [CallerFilePath] string currentPath="")
		{
			string dir = Path.GetDirectoryName(currentPath);
			return Path.Combine(dir, file);
		}
		public Animation GetAnimation(string spriteSheetName, string animationName)
		{
			if (spriteSheetDataDictionary.TryGetValue(spriteSheetName, out var scaleAndAnimationDictionary))
			{
				return scaleAndAnimationDictionary.Item2.TryGetValue(animationName, out Animation animation) ? animation : null;
			}
			return null;
		}
		// This one is temporary, just to let things work output
		public Animation GetAnimation(string animationName)
		{
			foreach(var spriteSheetAndData in spriteSheetDataDictionary)
			{
				if (spriteSheetAndData.Value.Item2.ContainsKey(animationName))
				{
					return spriteSheetAndData.Value.Item2[animationName];
				}
			}
			return GetAnimation("mario", animationName);
		}
		public float GetTexturesScale(string textureName)
		{
			if (spriteSheetDataDictionary.TryGetValue(textureName, out var scaleAndAnimationDictionary))
			{
				return scaleAndAnimationDictionary.Item1;
			}
			return 1.0f;

		}
		public IEnumerable<Animation> GetAllAnimations()
		{
			foreach (var (scale, animations) in spriteSheetDataDictionary.Values)
			{
				foreach(Animation animation in animations.Values)
				{
					yield return animation;
				}
			}
		}
    }
}
