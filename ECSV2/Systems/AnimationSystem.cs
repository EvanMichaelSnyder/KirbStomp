using ECSV2.Systems.ISystems;
using KirbStomp.ECSV2.Animations;
using KirbStomp.ECSV2.Animations.Content;
using KirbStomp.ECSV2.Components;
using KirbStomp.ECSV2.ECSEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECSV2.Systems{
	internal class AnimationSystem : IUpdatableSystem, ILoadableSystem
	{
		private readonly ECSManager manager;
		private AnimationsRepository animationsRepository;

		public AnimationSystem()
		{
			this.manager = ECSManager.GetInstance();
			this.animationsRepository = AnimationsRepository.GetInstance();
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
				if(animationComponent.durationSinceLastFrame >= animation.frameDuration)
				{
					animationComponent.durationSinceLastFrame = 0;
					animationComponent.currentFrame++;
					if (animationComponent.currentFrame >= animation.numberOfFrames)
						animationComponent.currentFrame = 0;
				}
				UpdateSpriteFromAnimation(sprite, animation.spriteSheet, animation.sourceFrames[animationComponent.currentFrame]);
				
				
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
				animation.Load(content);
			}

		}
		private void UpdateSpriteFromAnimation(SpriteComponent sprite, Texture2D spriteSheet, Rectangle source)
		{
			sprite.spriteSheet = spriteSheet;
			sprite.spriteSource = source;
		}
	}
}
