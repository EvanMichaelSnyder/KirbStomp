using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class SetTrajectory : IECSComponent, IUpdatableECSComponent
	{
		public int currentFrame;
		public int totalFrames;
		// At frame int, the direction of the trajectory switches to Vector2 with float velocity
		public List<(int, float, Vector2)> trajectoryData;
		public SetTrajectory(List<(int, float, Vector2)> trajectory, int startingFrame = 0)
		{
			trajectoryData = trajectory;
			currentFrame = startingFrame;
			totalFrames = trajectory.Count();
		}
		public void Update(float deltaTime)
		{
			
		}
	}
}
