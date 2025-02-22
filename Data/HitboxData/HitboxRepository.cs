using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using KirbStomp;
using KirbStomp.Scripts.Classes.HitboxList;
using Microsoft.Xna.Framework;

public static class HitboxSystem
{
    #region Data Structures

    public class FrameData
    {
        public List<HitboxData> Hitboxes = new List<HitboxData>();
    }

    public class AnimationData
    {
        public List<FrameData> Frames = new List<FrameData>();
    }

    public class SpriteSheetData
    {
        public string SpriteSheet;
        public Dictionary<string, AnimationData> Animations = new Dictionary<string, AnimationData>();
    }
    #endregion

    #region Main Stuff
    private static readonly Dictionary<string, SpriteSheetData> _hitboxDatabase =
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
        if (!_hitboxDatabase.TryGetValue(name, out SpriteSheetData sheetData))
            return null;

        if (!sheetData.Animations.TryGetValue(animationNameString, out AnimationData animData))
            return null;

        return animData;
    }
    #endregion

    #region Public Interactions
    public static void LoadHitboxesFromXml(string xmlPath)
    {
        XDocument doc = XDocument.Load(xmlPath);
        XElement root = doc.Root;

        XElement texture = root.Element("Texture");
        string name = texture.Attribute("character").Value;
        string sheetName = texture.Attribute("spriteSheet").Value;

        if (!_hitboxDatabase.TryGetValue(name, out SpriteSheetData sheetData))
        {
            sheetData = new SpriteSheetData { SpriteSheet = sheetName };
            _hitboxDatabase[name] = sheetData;
        }

        foreach (XElement animElement in root.Elements("Animation"))
        {
            string animName = animElement.Attribute("name").Value;
            var animData = new AnimationData();

            foreach (XElement frameElement in animElement.Elements("Frame"))
            {
                var frameData = new FrameData();
                foreach (XElement hitboxElement in frameElement.Elements("Hitbox"))
                {
                    frameData.Hitboxes.Add(new HitboxData
                    {
                        Position = ParseVector(hitboxElement.Attribute("position").Value),
                        Size = ParseVector(hitboxElement.Attribute("size").Value)
                    });
                }
                animData.Frames.Add(frameData);
            }

            sheetData.Animations[animName] = animData;
        }
    }

    public static (FrameData frame, string sourceSheet) GetFrameData(
        string name,
        StateEnum animationName,
        int frameIndex)
    {
        string animationNameString = animationName.ToString();
        if (!_hitboxDatabase.TryGetValue(name, out SpriteSheetData sheetData))
            throw new ArgumentException($"Character '{name}' not found");

        if (!sheetData.Animations.TryGetValue(animationNameString, out AnimationData animData))
            throw new ArgumentException($"Animation '{animationNameString}' not found in {name}");

        if (frameIndex < 0 || frameIndex >= animData.Frames.Count)
            throw new IndexOutOfRangeException($"Invalid frame index {frameIndex} for {animationNameString}");

        return (animData.Frames[frameIndex], sheetData.SpriteSheet);
    }
    #endregion
}