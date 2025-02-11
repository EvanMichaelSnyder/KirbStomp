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
		private List<string> animationXMLFiles;
		private Dictionary<string, (float, Dictionary<string, Animation>)> spriteSheetDataDictionary;
		public AnimationsRepository(List<string> animationXMLFiles)
		{
			this.animationXMLFiles = animationXMLFiles;
			this.spriteSheetDataDictionary = new();
		}
		public void LoadAllAnimations([CallerFilePath] string currentFile = "")
		{
			string textureName;
			float textureScale;
			Dictionary<string, Animation> animationsDictionary; // TODO there's ambiguity rn
			string path;
			foreach (string file in animationXMLFiles)
			{
				path = Path.Combine(currentFile, file);
				(textureName, textureScale, animationsDictionary) = AnimationsXMLParser.LoadAnimationsFromXML(path);
				spriteSheetDataDictionary.Add(textureName, (textureScale, animationsDictionary));
			}
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
			return GetAnimation("mario.XML", animationName);
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
