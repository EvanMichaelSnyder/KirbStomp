using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using KirbStomp;
using Microsoft.Xna.Framework;

namespace KirbStomp {
	public static class AnimationRepository
	{
		#region Data Structures
		public class FrameData
		{
			public Vector2 Position;
			public Vector2 Size;
			public Vector2 PerFrameOffset;

			public Rectangle ToRectangle(DirectionEnum direction, float boundX)
			{
				if (direction == DirectionEnum.Left)
				{
					return new Rectangle((int)(boundX - Position.X - Size.X),
										(int)Position.Y,
										(int)Size.X,
										(int)Size.Y);
				}
				return new Rectangle((int)Position.X,
									(int)Position.Y,
									(int)Size.X,
									(int)Size.Y);
			}


			public Rectangle ToRectanglePretty(DirectionEnum direction, float boundX)
			{
				throw new NotImplementedException();
				if (direction == DirectionEnum.Left)
				{
					return new Rectangle((int)(boundX - Position.X - Size.X - 1),
										(int)Position.Y - 1,
										(int)Size.X + 1,
										(int)Size.Y + 1);
				}
				return new Rectangle((int)Position.X - 1,
									(int)Position.Y - 1,
									(int)Size.X + 1,
									(int)Size.Y + 1);
			}
		}

		public class AnimationData
		{
			public float Duration;
			public bool Loop;
			public Vector2 AnimationOffset;
			public bool IsDefault;
			public List<FrameData> Frames = new List<FrameData>();
		}

		public class SpriteSheetData
		{
			public string SpriteSheet;
			public float Scale;
			public float BoundX;
			public float OffsetDirectional;
			public Dictionary<string, AnimationData> Animations = new Dictionary<string, AnimationData>();
		}
		#endregion

		#region Main Stuff
		private static readonly Dictionary<string, SpriteSheetData> _animationDatabase =
			new Dictionary<string, SpriteSheetData>();

		private static Vector2 ParseVector(string vectorString)
		{
			string[] parts = vectorString.Trim('(', ')').Split(',');
			return new Vector2(
				float.Parse(parts[0].Trim()),
				float.Parse(parts[1].Trim())
			);
		}
		public static AnimationData GetAnimationData(string name, StateEnum animationName)
		{
			string animationNameString = animationName.ToString();
			if (!_animationDatabase.TryGetValue(name, out SpriteSheetData sheetData))
				return null;

			if (!sheetData.Animations.TryGetValue(animationNameString, out AnimationData animData))
				return null;

			return animData;
		}
		#endregion



		#region Public Interactions
		public static void LoadAnimationsFromXml(string xmlPath)
		{
			XDocument doc = XDocument.Load(xmlPath);
			XElement root = doc.Root;

			// Parse Texture element
			XElement texture = root.Element("Texture");
			string name = texture.Attribute("character").Value;
			string sheetName = texture.Attribute("spriteSheet").Value;
			float scale = float.Parse(texture.Attribute("scale").Value);
			float boundX = float.Parse(texture.Attribute("boundX").Value);
			float offsetDirectional = float.Parse(texture.Attribute("offsetDirectional").Value);

			// Get or create sprite sheet entry
			if (!_animationDatabase.TryGetValue(name, out SpriteSheetData sheetData))
			{
				sheetData = new SpriteSheetData
				{
					SpriteSheet = sheetName,
					Scale = scale,
					BoundX = boundX,
					OffsetDirectional = offsetDirectional
				};
				_animationDatabase[name] = sheetData;
			}

			// Parse Animation elements
			foreach (XElement animElement in root.Elements("Animation"))
			{
				var animData = new AnimationData
				{
					Duration = float.Parse(animElement.Attribute("duration").Value),
					Loop = bool.Parse(animElement.Attribute("loop").Value),
					AnimationOffset = ParseVector(animElement.Attribute("animationOffset").Value),
					IsDefault = animElement.Attribute("isDefault")?.Value.ToLower() == "true"
				};

				string animName = animElement.Attribute("name").Value;

				// Parse Frame elements
				foreach (XElement frameElement in animElement.Elements("Frame"))
				{
					animData.Frames.Add(new FrameData
					{
						Position = ParseVector(frameElement.Attribute("position").Value),
						Size = ParseVector(frameElement.Attribute("size").Value),
						PerFrameOffset = ParseVector(frameElement.Attribute("perFrameOffset").Value)
					});
				}

				sheetData.Animations[animName] = animData;
			}
		}

		public static (FrameData frame, Vector2 totalOffset,string sourceSheet, float scale, float boundX, float offSetDirectional) GetFrameData(
			string name,
			StateEnum animationName,
			int frameIndex)
		{
			string animationNameString = animationName.ToString();
			if (!_animationDatabase.TryGetValue(name, out SpriteSheetData sheetData))
				throw new ArgumentException($"Character '{name}' not found");

			if (!sheetData.Animations.TryGetValue(animationNameString, out AnimationData animData))
				throw new ArgumentException($"Animation '{animationNameString}' not found in {name}");

			if (frameIndex < 0 || frameIndex >= animData.Frames.Count)
				throw new IndexOutOfRangeException($"Invalid frame index {frameIndex} for {animationNameString}");

			var frame = animData.Frames[frameIndex];
			Vector2 totalOffset = animData.AnimationOffset + frame.PerFrameOffset;

			return (frame, totalOffset, sheetData.SpriteSheet, sheetData.Scale, sheetData.BoundX, sheetData.OffsetDirectional);
		}

		#endregion
	}
}