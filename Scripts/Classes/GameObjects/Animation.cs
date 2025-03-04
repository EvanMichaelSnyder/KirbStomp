using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Projectiles
{
    public class Animation
    {
        public class Frame
        {
            private Rectangle _srcRectangle;
            private float _frameTime;
            public Frame(Rectangle srcRectangle, float frameTime)
            {
                _srcRectangle = srcRectangle;
                _frameTime = frameTime;
            }

            public Rectangle GetSrcRectangle()
            {
                return _srcRectangle;
            }

            public float GetFrameTime()
            {
                return _frameTime;
            }

        }
        private List<Frame> _frames;
        private bool _doesLoop;
        private String _name;
        public Animation(String animationName, bool doesLoop) {
            this._name = animationName;
            this._doesLoop = doesLoop;
            this._frames = new List<Frame>();
        }
        public void AddFrame(Frame frame)
        {
            _frames.Add(frame);
        }
        

        public Frame GetFrame(int index)
        {
            return this._frames[index];
        }
        

        public int GetAnimationLength()
        {
            return (int)this._frames.Count;
        }

        public String GetName()
        {
            return this._name;
        }

       public bool IsLooping()
        {
            return this._doesLoop;
        }
    }
}
