using System;
using System.Collections.Generic;
using System.Xml.Linq;
using KirbStomp;
using Microsoft.Xna.Framework;

public static class AttackDataRepository
{
    #region Data Structures
    public class AttackData
    {
        public bool hasData = false;
        public float Damage { get; private set; }
        public Vector2 Impulse { get; private set; }
        public bool CanFlipX { get; private set; }
        public bool CanFlipY { get; private set; }
        public bool HitboxAngleShearing { get; private set; }
        public float ClockTime { get; private set; }

        public static AttackData FromXml(XElement element)
        {
            return new AttackData
            {
                Damage = Convert.ToSingle(element.Attribute("Damage").Value),
                Impulse = new Vector2(
                    Convert.ToSingle(element.Attribute("ForceX").Value),
                    Convert.ToSingle(element.Attribute("ForceY").Value)
                ),
                CanFlipX = Convert.ToBoolean(element.Attribute("canFlipX").Value),
                CanFlipY = false,
                HitboxAngleShearing = Convert.ToBoolean(element.Attribute("hitboxAngleShearing").Value),
                ClockTime = Convert.ToSingle(element.Attribute("clock").Value)
            };
        }
    }
    #endregion

    #region Repository

    // [CharacterName][AnimationName] -> AttackData
    private static readonly Dictionary<string, Dictionary<string, AttackData>> _attackDatabase =
        new Dictionary<string, Dictionary<string, AttackData>>(StringComparer.OrdinalIgnoreCase);
    #endregion

    #region Public Interface
    public static void LoadAttackDataFromXml(string xmlPath)
    {
        XDocument doc = XDocument.Load(xmlPath);

        foreach (XElement textureElement in doc.Root.Elements("Texture"))
        {
            string character = textureElement.Attribute("character").Value;
            var animations = new Dictionary<string, AttackData>(StringComparer.OrdinalIgnoreCase);

            foreach (XElement animElement in textureElement.Elements("Animation"))
            {
                string animName = animElement.Attribute("name").Value;
                animations[animName] = AttackData.FromXml(animElement);
            }

            _attackDatabase[character] = animations;
        }
    }

    public static AttackData GetAttackData(string characterName, string animationName)
    {
        if (!_attackDatabase.TryGetValue(characterName, out var characterAnims))
            throw new KeyNotFoundException($"No animations found for character '{characterName}' ");

        AttackData data = new AttackData();

        if(!characterAnims.ContainsKey(animationName))
        {
            data.hasData = false;
        }
        else
        {
            data = characterAnims[animationName];
            data.hasData = true;
        }
        return data;
    }

    // Overload for enum-based access
    public static AttackData GetAttackData(string characterName, StateEnum animationState)
    {
        return GetAttackData(characterName, animationState.ToString());
    }
    #endregion
}