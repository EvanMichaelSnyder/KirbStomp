using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
	internal class SetPathSystem : ISystem, IUpdatableSystem
	{
		private EntityManager manager;
		public SetPathSystem()
		{
			manager = EntityManager.GetInstance();
		}
		public void Update(float deltaTime)
		{
			List<(int, float, Vector2)> path;
			int next;
			foreach(var (entity, trajectory, rigidBody) in manager.GetEntitiesWithComponents<SetTrajectory, RigidBody2DComponent>())
			{
				path = trajectory.trajectoryData;
				trajectory.durationSinceLast += deltaTime;
				next = trajectory.currentIndex + 1 < path.Count() ? trajectory.currentIndex + 1 : path.Count() - 1;
				if(trajectory.durationSinceLast >= trajectory.durationPerFrame)
				{
					trajectory.durationSinceLast = 0.0f;
					trajectory.currentFrame++;
					if(trajectory.currentFrame >= path[next].Item1)
					{
						trajectory.currentIndex++;
					}
					if(trajectory.currentFrame >= trajectory.totalFrames)
					{
						trajectory.currentFrame = 0;
						trajectory.currentIndex = 0;
					}
				}
				rigidBody.setVelocity(path[trajectory.currentIndex].Item3 * path[trajectory.currentIndex].Item2);
			}
		}
	}
}
