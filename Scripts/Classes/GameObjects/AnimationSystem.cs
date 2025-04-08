using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KirbStomp.Scripts.Projectiles.Animation;

namespace KirbStomp.Scripts.Projectiles
{
    public class AnimationSystem
    {
        private Dictionary<String,Animation> _animations;
        private Animation _currentAnimation;
        private Sprite _animatingSprite;

        private int _frameIndex;
        private float _elapsedTime;
        private bool _IsAnimationDone = false;
        public AnimationSystem(Sprite animatingSprite) 
        { 
            this._IsAnimationDone = false;
            this._animations = new Dictionary<String,Animation>();
            this._animatingSprite = animatingSprite;
        }

        public void AddAnimation(Animation animation)
        {
            if (_currentAnimation == null)
            {
                this._currentAnimation = animation;
                
            }


            if (this._animations.ContainsKey(animation.GetName()))
            {
                this._animations[animation.GetName()] = animation;
            }
            else
            {
                this._animations.Add(animation.GetName(), animation);
            }
            
        }

        public void Animate(float dt)
        {
            if (_IsAnimationDone) return;
            _elapsedTime += dt;
            if (_elapsedTime >= this._currentAnimation.GetFrame(this._frameIndex).GetFrameTime())
            {
                this._elapsedTime = 0;
                this.ChangeFrame();
            }
        }

        private void ChangeFrame()
        {
            this._frameIndex++;
            if (this._frameIndex >= this._currentAnimation.GetAnimationLength())
            {
                if (this._currentAnimation.IsLooping())
                {
                    this._frameIndex = 0;
                    this._elapsedTime = 0;
                    this._frameIndex = 0;
                   


                }
                else
                {//if not loop, stay same frame
                    this._frameIndex--;
                    this._IsAnimationDone = true;
                }
            }
            this._animatingSprite.SetOffset(this._currentAnimation.GetFrame(_frameIndex).GetOffset());
            this._animatingSprite.SetSrcRectangle(this._currentAnimation.GetFrame(this._frameIndex).GetSrcRectangle());
            //Debug.WriteLine(this._currentAnimation.GetFrame(this._frameIndex).GetSrcRectangle().ToString());
        }

        public void SetAnimation(String name)
        {
            this._IsAnimationDone = false;
            if (!this._animations.ContainsKey(name))
            {
                throw new Exception("ANIMATION SYSTEM DOES NOT CONTAIN: " + name);
            }
            this._currentAnimation = this._animations[name];
            this._elapsedTime = 0;
            this._frameIndex = 0;

            this._animatingSprite.SetOffset(this._currentAnimation.GetFrame(_frameIndex).GetOffset());
            this._animatingSprite.SetSrcRectangle(this._currentAnimation.GetFrame(this._frameIndex).GetSrcRectangle());
        }

        public Animation GetCurrentAnimation()
        {
            return this._currentAnimation;
        }

        public bool IsAnimationDone()
        {
            return this._IsAnimationDone;
        }
    }
}
