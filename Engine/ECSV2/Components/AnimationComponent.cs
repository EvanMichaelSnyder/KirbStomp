using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.Components.IComponents;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class AnimationComponent : Component
	{
		private (string, string) fullAnimationName;
		private int currentFrame;
		private float durationSinceLastFrame;

		private (string, string, int) nextAnimationData;
		public AnimationComponent((string, string) fullAnimationName, int startingFrame = 0, float durationSpentOnCurrentFrame = 0.0f)
		{
			this.fullAnimationName = fullAnimationName;
			currentFrame = startingFrame;
			durationSinceLastFrame = durationSpentOnCurrentFrame;
			nextAnimationData = (fullAnimationName.Item1, fullAnimationName.Item2, startingFrame);
;
		}

		// By default, animation's will continue cycling through the 
		public void ChangeCurrentAnimationData(string character, string animation, int startingFrame = 0)
		{
			fullAnimationName = (character, animation);
			currentFrame = startingFrame;
			nextAnimationData = (fullAnimationName.Item1, fullAnimationName.Item2, 0);
		}

		public void ChangeNextAnimationData(string characterName, string animationName, int startingFrame = 0)
		{
			this.nextAnimationData = (characterName, animationName, startingFrame);
		}
		

		public (string, string) GetFullAnimationName()
		{
			return fullAnimationName;
		}
		public int GetCurrentFrame()
		{
			return currentFrame;
		}
		public void IncrementCurrentFrame()
		{
			this.currentFrame++;
		}
		public void SetCurrentFrame(int frame)
		{
			this.currentFrame = frame;
		}
		public void AddDurationSinceLastFrame(float deltaTime)
		{
			this.durationSinceLastFrame += deltaTime;
		}
		public float GetDurationSinceLastFrame()
		{
			return this.durationSinceLastFrame;
		}
		public void SetDurationSinceLastFrame(float time)
		{
			this.durationSinceLastFrame = time;
		}
		public void CycleAnimation()
		{
			fullAnimationName = (nextAnimationData.Item1, nextAnimationData.Item2);
			nextAnimationData = (fullAnimationName.Item1, fullAnimationName.Item2, 0);
			currentFrame = nextAnimationData.Item3;
		}
	}
}