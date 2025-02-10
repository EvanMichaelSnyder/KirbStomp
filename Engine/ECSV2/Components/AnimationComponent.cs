using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.Components.IECSComponents;

namespace KirbStomp.Engine.ECSV2.Components
{
    internal class AnimationComponent : IECSComponent
    {
        public string animationName { get; set; }
        public int currentFrame { get; set; }

        public string nextAnimation { get; set; }
        public int nextStartingFrame { get; set; }
        public float durationSinceLastFrame { get; set; }
        public AnimationComponent(string animationName = "Idle", int startingFrame = 0, float durationSpentOnCurrentFrame = 0.0f)
        {
            this.animationName = animationName;
            nextAnimation = animationName;
            currentFrame = startingFrame;
            durationSinceLastFrame = durationSpentOnCurrentFrame;
        }

        // By default, animation's will continue cycling through the 
        public void CycleAnimation()
        {
            animationName = nextAnimation;
            nextAnimation = animationName;
            currentFrame = nextStartingFrame;
            nextStartingFrame = 0;
        }
    }
}
