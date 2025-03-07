using KirbStomp.Data;
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
			Name = "default";
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
		private static Dictionary<string, TempCharacterStats> characters = new();
		

	
		public static void LoadCharacter(string name)
		{
			if(!characters.ContainsKey(name))
			{
				characters.Add(name, ParseCharacterXML(name));
			}
			// if(!characters.TryAdd(name, ParseCharacterXML(name)))
			// {
			// 	throw new Exception($"Already added character {name}");
			// }
			
		}
	
		private static TempCharacterStats GetCharacterStats(string name)
		{
			TempCharacterStats stats;
			if(characters.TryGetValue(name, out stats))
			{
				return stats;
			}
			return default;
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
	}
}
