using KirbStomp.ECSV2.Components.IECSComponents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Components
{
	internal class ExamplePlayerState : IECSComponent, IUpdatableECSComponent
	{
		public string name;
		public int playerCondition;
		public int playerHealth;
		public bool exState1; 
		public bool exState2;

		public float movementVel;
		public Vector2 normalMovementDirection;
		public bool isWalking;
		public bool isJumping;

		public ExamplePlayerState(string name)
		{
			this.name = name;
		}
		public void Update(float deltatTime)
		{
			Debug.WriteLine("DEBUG: Player Name: {0}, exState1: {1}, exState2 {2}, playerHealth {3}, playerCondition {4}", name, exState1, exState2, playerHealth, playerCondition);
			Console.WriteLine("CONSOLE: Player Name: {0}, exState1: {1}, exState2 {2}, playerHealth {3}, playerCondition {4}", name, exState1, exState2, playerHealth, playerCondition);
		}


			
	}
}
