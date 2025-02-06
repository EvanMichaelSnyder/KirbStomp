using KirbStomp.Inputs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ecs.Components
{
	enum ItemCategory
	{
		PermanentConsumable, TemporaryConsumable
	}
	enum ItemTypes
	{
		Hamburger, Star
	}
	/// <summary>
	/// How will we do items? 
	/// Item will have category and type, CATEGORY determines what kind of effect, while TYPE determines the details of the effect
	/// For now, item isn't consumable, it will purely be a sprite 
	/// </summary>
	internal class Item : Component
	{

		public ItemCategory itemCategory; 
		public ItemTypes itemType;
		private bool isActive;
		private GlobalInputs inputs = GlobalInputs.GetInstance();
		public Item()
		{

		}
		public override void Update(float deltaTime)
		{
			// Process animation
			// Process am I active, if I'm not, remove me from the scene
			if (inputs.IsInputJustPressed(Keys.P))
			{
				isActive = false;
				Debug.WriteLine("NO LONGER ACTIVE");
			}
			if (inputs.IsInputJustPressed(Keys.L))
			{
				isActive = true;
				Debug.WriteLine("NOW ACTIVE");
			}
			// TEMP
		}
	}
}
