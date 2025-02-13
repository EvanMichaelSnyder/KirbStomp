using KirbStomp.Engine.Animations;
using KirbStomp.Engine.Animations.Content;
using KirbStomp.Engine.ECSV2.Components;

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
		private readonly EntityManager manager;
		private AnimationsRepository animationsRepository;

		private static Dictionary<uint, (string, string, int)> entitiesToChangeCurrentFrame = new();
		private static Dictionary<uint, (string, string, int)> entitiesToChangeNextFrame = new();

        public AnimationSystem()
		{
			manager = EntityManager.GetInstance();
			animationsRepository = new AnimationsRepository(new() { "mario", "Items", "Link", "NewMario", "PlatformBlocks", "MegaMan"});
			animationsRepository.InitializeAnimations();
        }


        public void Update(float deltaTime)
        {

            Animation animation;
            (string, string) fullAnimationName;
			(string, string, int) changingAnimationData;
			foreach (var (entity, sprite, animationComponent) in manager.GetEntitiesWithComponents<SpriteComponent, AnimationComponent>())
            {
				// The following two if states can be put into their own helper function TODO
				if (entitiesToChangeCurrentFrame.TryGetValue(entity.GetID(), out changingAnimationData))
				{
					ChangeCurrentAnimation(animationComponent, changingAnimationData.Item1, changingAnimationData.Item2, changingAnimationData.Item3);
					entitiesToChangeCurrentFrame.Remove(entity.GetID());
				}
				if (entitiesToChangeNextFrame.TryGetValue(entity.GetID(), out changingAnimationData))
				{
					ChangeNextAnimation(animationComponent, changingAnimationData.Item1, changingAnimationData.Item2, changingAnimationData.Item3);
					entitiesToChangeNextFrame.Remove(entity.GetID());
				}

				fullAnimationName = animationComponent.GetFullAnimationName();
				animationComponent.AddDurationSinceLastFrame(deltaTime);
                animation = animationsRepository.GetAnimation(fullAnimationName.Item1, fullAnimationName.Item2);
                if (animationComponent.GetDurationSinceLastFrame()>= animation.frameDuration)
                {
					animationComponent.SetDurationSinceLastFrame(0.0f);
					animationComponent.IncrementCurrentFrame();
                    if (animationComponent.GetCurrentFrame() >= animation.numberOfFrames)
                    {
						animationComponent.SetCurrentFrame(0);
                        animationComponent.CycleAnimation();
                    }
                }
				UpdateSpriteFromAnimation(sprite, animation, animationComponent.GetCurrentFrame());


            }
        }

        public void Load(ContentManager content)
        {
			foreach (Animation animation in animationsRepository.GetAllAnimations())
			{
				animation.Load(content);
			}
        }
		private void ChangeCurrentAnimation(AnimationComponent animation, string character, string name, int startingFrame)
		{
			Logger.Log(animation.GetFullAnimationName().ToString());
			animation.ChangeCurrentAnimationData(character, name, startingFrame);
		}
		private void ChangeNextAnimation(AnimationComponent animation, string character, string name, int startingFrame)
		{
			animation.ChangeNextAnimationData(character, name, startingFrame);
		}



        // This implementation was for quick development.
        // There is another implementation where we keep a Container of entities to change the current/next frame and change it all during the update loop.
        // This container implemntation updated on Update may be more appropriate for a System

		public static bool ChangeEntitysCurrentAnimation(Entity entity, (string, string) textureAndAnimation, int startingFrame = 0)
		{
			return entitiesToChangeCurrentFrame.TryAdd(entity.GetID(), (textureAndAnimation.Item1, textureAndAnimation.Item2, startingFrame));
		}
		public static bool ChangeEntitysNextAnimation(Entity entity, (string, string) textureAndAnimation, int startingFrame = 0)
		{
			return entitiesToChangeNextFrame.TryAdd(entity.GetID(), (textureAndAnimation.Item1, textureAndAnimation.Item2, startingFrame));
		}

        private void UpdateSpriteFromAnimation(SpriteComponent sprite, Animation animation, int currentFrame)
        {
            Rectangle frame = animation.sourceFrames[currentFrame];
			Point perFrameOffset = animation.perFrameOffset[currentFrame];
			float scale = animationsRepository.GetTexturesScale(animation.textureName);
			sprite.SetTexture(animation.spriteSheet);
			sprite.SetSpriteSource(frame);
			sprite.SetSpriteWidth(frame.Width + perFrameOffset.X);
			sprite.SetSpriteHeight(frame.Height + perFrameOffset.Y);
			sprite.SetScale(scale);

        }
    }
}
