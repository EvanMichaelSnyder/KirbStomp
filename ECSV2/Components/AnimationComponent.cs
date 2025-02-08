using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.ECSV2.Components.IECSComponents;

namespace KirbStomp.ECSV2.Components
{
    internal class AnimationComponent : IECSComponent
    {
        public string animationName { get; set; }
        public int currentFrame { get; set; }
        public float durationSinceLastFrame { get; set; }
        public AnimationComponent(string animationName = "Idle", int startingFrame = 0, float durationSpentOnCurrentFrame = 0.0f)
        {
            this.animationName = animationName;
            currentFrame = startingFrame;
            durationSinceLastFrame = durationSpentOnCurrentFrame;
        }
    }
}
