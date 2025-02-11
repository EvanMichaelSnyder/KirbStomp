using KirbStomp.Engine.ECSV2.Components.IComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
    internal class SetTrajectory : Component
	{
		public int currentFrame;
		public int currentIndex;
		public int totalFrames;
		public float durationSinceLast;
		public float durationPerFrame;
		// At frame int, the direction of the trajectory switches to Vector2 with float velocity
		public List<(int, float, Vector2)> trajectoryData;
		public SetTrajectory(List<(int, float, Vector2)> trajectory, int startingFrame = 0, float durationOfPath = 1.0f)
		{
			trajectory.Sort(TrajectoryComparison); // trajectory WILL be sorted
			trajectoryData = trajectory;
			currentFrame = startingFrame;
			totalFrames = trajectory.Last().Item1;
			durationPerFrame = durationOfPath / totalFrames;
			durationSinceLast = 0.0f;
		}
		public void ChangeTrajectory(List<(int, float, Vector2)> trajectory)
		{
			trajectory.Sort(TrajectoryComparison);
			trajectoryData = trajectory;
			if (currentFrame > trajectory.Last().Item1) currentFrame = 0;
		}
		private int TrajectoryComparison((int, float, Vector2) obj1, (int, float, Vector2) obj2)
		{
			return obj1.Item1 - obj2.Item1;
		}
	}

}
