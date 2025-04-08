using KirbStomp.Data;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KirbStomp
{

	// Intermediary for character stats and character
	struct TempCharacterStats
	{
		string Name;
		float Health;
		MovementStats characterMovementStats;
		PhysicsStats characterPhysicsStats;
		public TempCharacterStats()
		{
			Name = "defaul";
			Health = 300.0f;
			characterMovementStats = new();
			characterPhysicsStats = new();
		}
		public TempCharacterStats(string name, float health, MovementStats movementStats, PhysicsStats physicsStats)
		{
			Name = name;
			Health = health;
			characterMovementStats = movementStats;
			characterPhysicsStats = physicsStats;
		}
	}
	internal class CharacterXMLParser
	{

		public static void LoadCharacter(string name)
		{
      
		}

		private static TempCharacterStats ParseCharacterXML(string fileName)
		{
			XElement characterElement = GetCharacterStatsXElement(fileName);

			string characterName = GetXElementOrAssert("Name", characterElement).Value;
			float characterHealth = float.Parse(GetXElementOrAssert("Health", characterElement).Value);
			MovementStats movementStats = GetMovementStatsFromCharacter(characterElement);
			PhysicsStats physicsStats = GetPhysicsStatsFromCharacter(characterElement);
			return new TempCharacterStats(characterName, characterHealth, movementStats, physicsStats);
		}
		
		public static CharacterStats LoadCharacterStatsFile(string fileName)
		{
			XElement characterElement = GetCharacterStatsXElement(fileName);

			string characterName = GetXElementOrAssert("Name", characterElement).Value;
			float characterHealth = float.Parse(GetXElementOrAssert("Health", characterElement).Value);
			string positionStr = GetXElementOrAssert("Position", characterElement).Value;
			int[] positionInts = Array.ConvertAll(positionStr.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
            Vector2 position = new(positionInts[0], positionInts[1]);
			MovementStats movementStats = GetMovementStatsFromCharacter(characterElement);
			PhysicsStats physicsStats = GetPhysicsStatsFromCharacter(characterElement);
			AvailableAttacks availableAttacks = GetAvailableAttacksFromCharacter(characterElement); // Not actually implemented

			XElement UIs = GetXElementOrAssert("UI", characterElement);
			UIIconData stockIcon = GetUIIconData(GetXElementOrAssert("StockIcon", UIs));
			UIIconData portraitIcon = GetUIIconData(GetXElementOrAssert("PortraitIcon", UIs));


			return new CharacterStats(characterName, characterHealth, position, movementStats, physicsStats, availableAttacks, stockIcon, portraitIcon);
		}


		private static XElement GetCharacterStatsXElement(string name)
		{
			string dataFolder = XMLData.GetDataFolder();
			string characterStatsFile= Path.Combine(dataFolder, "CharacterStats", name + ".XML");
			XDocument doc = XDocument.Load(characterStatsFile);
			XElement root = doc.Root; // For all characters, this should be Character
			if(root.Name != "Character")
			{
				throw new ArgumentException($"File at {characterStatsFile} has wrong root of {root.Name} and not Character");
			}

			return root;
		}

		private static XElement GetXElementOrAssert(string name, XElement parent)
		{
			XElement output = parent.Element(name);
			if(output == null)
			{
				throw new Exception($"XElement did not contain {name} element in Parent: {parent.Name}");
			}
			return output;
		}
		private static MovementStats GetMovementStatsFromCharacter(XElement characterElement)
		{
			XElement movementElement		=	GetXElementOrAssert("Movement", characterElement);
			float baseWalkSpeed = float.Parse(GetXElementOrAssert("BaseWalkSpeed", movementElement).Value);
			float baseRunSpeed = float.Parse(GetXElementOrAssert("BaseRunSpeed", movementElement).Value);
			float maxRunSpeed = float.Parse(GetXElementOrAssert("MaxRunSpeed", movementElement).Value);

			XElement accelerationElement	=	GetXElementOrAssert("Accelerations", movementElement);
			float walkingAcceleration = float.Parse(GetXElementOrAssert("Walk", accelerationElement).Value);
			float runningAcceleration = float.Parse(GetXElementOrAssert("Run", accelerationElement).Value);
			float inAirAcceleration = float.Parse(GetXElementOrAssert("Air", accelerationElement).Value);
		
			return new MovementStats(baseWalkSpeed, baseRunSpeed, maxRunSpeed, walkingAcceleration, runningAcceleration, inAirAcceleration);
		}

		private static PhysicsStats GetPhysicsStatsFromCharacter(XElement characterElement)
		{
			XElement physicsElement = GetXElementOrAssert("PhysicsEffects", characterElement);
			float knockbackScale = float.Parse(GetXElementOrAssert("KnockbackScalar", physicsElement).Value);
			float gravity = float.Parse(GetXElementOrAssert("Gravity", physicsElement).Value);
			float speedDecay = float.Parse(GetXElementOrAssert("SpeedDecay", physicsElement).Value);
			return new PhysicsStats(knockbackScale, gravity, speedDecay);
		}

		
		private static UIIconData GetUIIconData(XElement uiIcon)
		{
			string spriteSheet = GetXElementOrAssert("SpriteSheet", uiIcon).Value.Replace(" ", string.Empty);

			string sourcePositionStr = GetXElementOrAssert("SourcePosition", uiIcon).Value;
			int[] positionInts = Array.ConvertAll(sourcePositionStr.Split(new char[] {' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
			string sourceSizeStr = GetXElementOrAssert("SourceSize", uiIcon).Value;
			int[] sizeInts = Array.ConvertAll(sourceSizeStr.Split(new char[] {' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);
			Rectangle source = new(positionInts[0], positionInts[1], sizeInts[0], sizeInts[1]);
			float iconScale = float.Parse(GetXElementOrAssert("Scale", uiIcon).Value);

			return new UIIconData(spriteSheet, source, iconScale);
		}

		private static AvailableAttacks GetAvailableAttacksFromCharacter(XElement characterElemenet)
		{
			XElement attacksElements = GetXElementOrAssert("Attacks", characterElemenet);
			

			return default;
		}

		private static void GetDirectionalAttacks(XElement directionalXElement, out bool front, out bool back, out bool up, out bool down)
		{
			front = bool.Parse(GetElementStringValueNoWhiteSpace("HasAttackFront", directionalXElement));
			back = bool.Parse(GetElementStringValueNoWhiteSpace("HasAttackBack", directionalXElement));
			up = bool.Parse(GetElementStringValueNoWhiteSpace("HasAttackUp", directionalXElement));
			down = bool.Parse(GetElementStringValueNoWhiteSpace("HasAttackDown", directionalXElement));
		}

		private static int GetHasAttacks(XElement elements, out List<int> attacksAvailable)
		{
			int attackNum = 0;
			int count = 0;
			attacksAvailable = new();
			foreach(XElement element in elements.Elements("HasAttack"))
			{
				if(!int.TryParse(element.Value, out attackNum))
				{
					throw new Exception($"Tried parsing {element.Value} into int for attacks and failed");
				}
				attacksAvailable.Add(attackNum);
				count++;
			}
			return count;
		}
		private static string GetElementStringValueNoWhiteSpace(string name, XElement element)
		{
			return GetXElementOrAssert(name, element).Value.Replace(" ", string.Empty);
		}
	}
}
