using KirbStomp.Engine.Animations;
using KirbStomp.Engine.Animations.Content;
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class AnimationSystem : IUpdatableSystem, ILoadableSystem
    {
        private static readonly ECSManager manager = ECSManager.GetInstance();
        private AnimationsRepository animationsRepository;

        public AnimationSystem()
        {
            animationsRepository = AnimationsRepository.GetInstance();
        }


        public void Update(float deltaTime)
        {

            Animation animation;
            string animationName;
            foreach (var (entity, sprite, animationComponent) in manager.GetEntitiesWithComponents<SpriteComponent, AnimationComponent>())
            {
                animationName = animationComponent.animationName;
                animation = animationsRepository.GetAnimation(animationName);
                animationComponent.durationSinceLastFrame += deltaTime;
                if (animationComponent.durationSinceLastFrame >= animation.frameDuration)
                {
                    animationComponent.durationSinceLastFrame = 0;
                    animationComponent.currentFrame++;
                    if (animationComponent.currentFrame >= animation.numberOfFrames)
                    {
                        animationComponent.currentFrame = 0;
                        animationComponent.CycleAnimation();
                    }
                }
                UpdateSpriteFromAnimation(sprite, animation, animationComponent.currentFrame);


            }
        }

        public void Load(ContentManager content)
        {
            Animation animation;
            string animationName;
            foreach (var (entity, animationComponent) in manager.GetEntitiesWithComponent<AnimationComponent>())
            {
                animationName = animationComponent.animationName;
                animation = animationsRepository.GetAnimation(animationName);
                if (animation != null) animation.Load(content);
            }

        }


        // This implementation was for quick development.
        // There is another implementation where we keep a Container of entities to change the current/next frame and change it all during the update loop.
        // This container implemntation updated on Update may be more appropriate for a System
        public static bool ChangeEntitysCurrentAnimation(ECSEntity entity, string animationName, int startingFrame = 0)
        {
            AnimationComponent component = manager.GetComponent<AnimationComponent>(entity);
            if (component == null) return false;

            component.animationName = animationName;
            component.currentFrame = startingFrame;
            return true;
        }

        public static bool ChangeEntitysNextAnimation(ECSEntity entity, string animationName, int startingFrame = 0)
        {
            AnimationComponent component = manager.GetComponent<AnimationComponent>(entity);
            if (component == null) return false;

            component.nextAnimation = animationName;
            component.nextStartingFrame = startingFrame;
            return true;
        }
        private void UpdateSpriteFromAnimation(SpriteComponent sprite, Animation animation, int currentFrame)
        {
            Rectangle frame = animation.sourceFrames[currentFrame];
            sprite.spriteSheet = animation.spriteSheet;
            sprite.spriteSource = frame;
            sprite.spriteDimensions.X = frame.Width;
            sprite.spriteDimensions.Y = frame.Height;

        }
    }
}
