using System;
using System.Collections.Generic;
using System.Xml.Linq;
using KirbStomp;
using Microsoft.Xna.Framework;

public static class AttackHitboxRepository
{
    #region Data Structures
    public class HitboxData
    {
        public Vector2 Position;
        public Vector2 Size;

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
    }

    public class FrameData
    {
        public List<HitboxData> Hitboxes = new List<HitboxData>();
    }

    public class AnimationData
    {
        public Dictionary<string, FrameData> Frames = new Dictionary<string, FrameData>();
    }

    public class CharacterData
    {
        public string SpriteSheet;
        public Dictionary<string, AnimationData> Animations = new Dictionary<string, AnimationData>();
    }
    #endregion

    #region Repository
    private static readonly Dictionary<string, CharacterData> _hitboxDatabase =
        new Dictionary<string, CharacterData>();

    private static Vector2 ParseVector(string vectorString)
    {
        string[] parts = vectorString.Trim('(', ')').Split(',');
        return new Vector2(
            float.Parse(parts[0].Trim()),
            float.Parse(parts[1].Trim())
        );
    }
    #endregion

    #region Public Interface
    public static void LoadHitboxesFromXml(string xmlPath)
    {
        XDocument doc = XDocument.Load(xmlPath);
        XElement root = doc.Root;

        foreach (XElement textureElement in root.Elements("Texture"))
        {
            string character = textureElement.Attribute("character").Value;
            string spriteSheet = textureElement.Attribute("spriteSheet").Value;

            if (!_hitboxDatabase.TryGetValue(character, out CharacterData charData))
            {
                charData = new CharacterData { SpriteSheet = spriteSheet };
                _hitboxDatabase[character] = charData;
            }

            foreach (XElement animElement in textureElement.Elements("Animation"))
            {
                string animName = animElement.Attribute("name").Value;
                string hasFrames = animElement.Attribute("hasFrames").Value;
                var animationData = new AnimationData();

                int frameIndex = 0;
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

                    animationData.Frames[frameIndex.ToString()] = frameData;
                    frameIndex++;
                }

                charData.Animations[animName] = animationData;
            }
        }
    }

    public static (FrameData frame, string sourceSheet) GetFrameData(
        string name,
        StateEnum animationName,
        int frameIndex)
    {
        string animName = animationName.ToString();
        if (!_hitboxDatabase.TryGetValue(name, out CharacterData charData))
            throw new ArgumentException($"Character '{name}' not found in hitbox database");

        if (!charData.Animations.TryGetValue(animName, out AnimationData animData))
            throw new ArgumentException($"Animation '{animName}' not found for {name}");

        if (!animData.Frames.TryGetValue(frameIndex.ToString(), out FrameData frameData))
            throw new IndexOutOfRangeException($"Frame {frameIndex} not found in {animName}");

        return (frameData, charData.SpriteSheet);
    }
    #endregion
}
