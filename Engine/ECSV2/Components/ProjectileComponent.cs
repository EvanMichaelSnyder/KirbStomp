using KirbStomp.Engine.ECSV2.Components.IComponents;
using KirbStomp.Engine.ECSV2.EntityManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	public delegate void CallBackOnProjectileEnd(Entity entity);
	internal class ProjectileComponent : Component
	{
		private readonly Entity currentEntity;
		private List<(float, Vector2)> path;
		private float durationOnCurrentSegment;
		private int currentIndex;
		private CallBackOnProjectileEnd callbackFN;
		public ProjectileComponent(Entity entity, List<(float, Vector2)> projectilePath, CallBackOnProjectileEnd callbackFN)
		{
			this.currentEntity = entity;
			this.durationOnCurrentSegment = 0.0f;
			this.currentIndex = 0;
			this.callbackFN = callbackFN;
			this.path = projectilePath;
		}
		public float UpdateDurationOnCurrentSegment(float deltaTime)
		{
			return this.durationOnCurrentSegment += deltaTime;
		}
		public void SetDurationOnCurrentSegment(float duration)
		{
			this.durationOnCurrentSegment = duration;
		}
		public int GetCurrentIndex()
		{
			return this.currentIndex;
		}
		public void SetCurrentIndex(int index)
		{
			this.currentIndex = index;
		}
		public Vector2 GetCurrentProjectileVelocity()
		{
			return this.path[currentIndex].Item2;
		}
		public int UpdateProjectile(float deltaTime)
		{
			durationOnCurrentSegment += deltaTime;
			if(durationOnCurrentSegment >= path[currentIndex].Item1)
			{
				durationOnCurrentSegment = 0;
				this.currentIndex++;
				if(currentIndex >= path.Count())
				{
					currentIndex = 0;
					callbackFN(currentEntity);
					Logger.Log("Projectile Just Ended");
				}
			}
			return currentIndex;
		}
	}
}
