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
		private static List<string> spriteSheetsToLoad;
		public static AnimationsRepository GetInstance()
		{
			if (instance == null) instance = new AnimationsRepository();
			return instance;
		}
		private AnimationsRepository()
		{
			animationDictionary = new();
			perTextureScale = new();
			spriteSheetsToLoad = new()
			{
				"mario.XML",
				"Items.XML",
				"NewMario.XML",
				"Link.XML",
				"MegaMan.XML"
			};

		}
		public static void LoadAllAnimations([CallerFilePath] string currentFile = "")
		{
			string textureName;
			float textureScale;
			Dictionary<string, Animation> animationsDictionary; // TODO there's ambiguity rn
			string path;
			foreach (string file in spriteSheetsToLoad)
			{
				path = Path.Combine(currentFile, file);
				(textureName, textureScale, animationsDictionary) = AnimationsXMLParser.LoadAnimationsFromXML(path);
			}
		}
    }
}
