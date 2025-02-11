using KirbStomp.Engine.Animations;
using KirbStomp.Engine.Animations.Content;
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.EntityManagement;
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
        private static readonly EntityManager manager = EntityManager.GetInstance();
        private AnimationsRepository animationsRepository;

		private static Dictionary<uint, (string, int)> entitiesToChangeCurrentFrame = new();
		private static Dictionary<uint, (string, int)> entitiesToChangeNextFrame = new();

        public AnimationSystem()
        {
            animationsRepository = AnimationsRepository.GetInstance();
        }


        public void Update(float deltaTime)
        {

            Animation animation;
            string animationName;
			(string, int) changingAnimationData;
			foreach (var (entity, sprite, animationComponent) in manager.GetEntitiesWithComponents<SpriteComponent, AnimationComponent>())
            {
				// The following two if states can be put into their own helper function TODO
				if (entitiesToChangeCurrentFrame.TryGetValue(entity.GetID(), out changingAnimationData))
				{
					ChangeCurrentAnimation(animationComponent, changingAnimationData.Item1, changingAnimationData.Item2);
					entitiesToChangeCurrentFrame.Remove(entity.GetID());
				}
				if (entitiesToChangeNextFrame.TryGetValue(entity.GetID(), out changingAnimationData))
				{
					ChangeNextAnimation(animationComponent, changingAnimationData.Item1, changingAnimationData.Item2);
					entitiesToChangeNextFrame.Remove(entity.GetID());
				}

				animationName = animationComponent.animationName;
                animationComponent.durationSinceLastFrame += deltaTime;
                animation = animationsRepository.GetAnimation(animationName);
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
			foreach (var nameAnimationPair in AnimationsRepository.animationDictionary)
			{
				nameAnimationPair.Value.Load(content);
			}

        }
		private void ChangeCurrentAnimation(AnimationComponent animation, string newName, int startingFrame)
		{
			animation.currentFrame = startingFrame;
			animation.animationName = newName;
			animation.nextAnimation = newName;
			animation.durationSinceLastFrame = 0.0f;
		}
		private void ChangeNextAnimation(AnimationComponent animation, string newName, int startingFrame)
		{
			animation.nextAnimation = newName;
			animation.nextStartingFrame = startingFrame;
		}

        // This implementation was for quick development.
        // There is another implementation where we keep a Container of entities to change the current/next frame and change it all during the update loop.
        // This container implemntation updated on Update may be more appropriate for a System
        public static bool ChangeEntitysCurrentAnimation(Entity entity, string animationName, int startingFrame = 0)
        {
			return entitiesToChangeCurrentFrame.TryAdd(entity.GetID(), (animationName, startingFrame));
        }

        public static bool ChangeEntitysNextAnimation(Entity entity, string animationName, int startingFrame = 0)
        {
			return entitiesToChangeNextFrame.TryAdd(entity.GetID(), (animationName, startingFrame));
        }
        private void UpdateSpriteFromAnimation(SpriteComponent sprite, Animation animation, int currentFrame)
        {
            Rectangle frame = animation.sourceFrames[currentFrame];
			Point perFrameOffset = animation.perFrameOffset[currentFrame];
			float scale;
			AnimationsRepository.perTextureScale.TryGetValue(animation.textureName, out scale);
            sprite.spriteSheet = animation.spriteSheet;
            sprite.spriteSource = frame;
			sprite.spriteDimensions.X = frame.Width + perFrameOffset.X;
			sprite.spriteDimensions.Y = frame.Height + perFrameOffset.Y;
			sprite.scale = scale;
        }
    }
}
