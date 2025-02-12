using KirbStomp.Engine.ECSV2.Components.IComponents;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems;
using KirbStomp.Engine.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class CreateProjectileComponent : Component, IDisposable
	{
		private readonly EntityManager manager;
		private readonly (string, string) animationName;
		private List<(float, Vector2)> projectilePath;
		private readonly Keys activationKey;
		private List<Entity> projectileEntities;
		private GlobalEvents globalEvents;
		public CreateProjectileComponent(Entity entity, (string, string) animation, List<(float, Vector2)> projectilePath, Keys keyPressToCreate)
		{
			this.SetEntity(entity);
			this.manager = EntityManager.GetInstance();
			this.projectilePath = projectilePath;
			this.activationKey = keyPressToCreate;
			this.animationName = animation;
			this.projectileEntities = new();
			this.globalEvents = GlobalEvents.GetInstance();
			globalEvents.SubscribeToKeyEvents(entity, this.activationKey, Inputs.InputStatus.JustPressed, CreateProjectile);
			
		}

		
		private void CreateProjectile(Entity entity)
		{
			Entity projectileEntity = manager.CreateEntity();
			Vector2 currentPosition = entity.GetPosition();
			manager.AddComponent(projectileEntity, new ProjectileComponent(projectileEntity, projectilePath, EndOfProjectile));
			manager.AddComponent(projectileEntity, new AnimationComponent(animationName));
			manager.AddComponent(projectileEntity, new SpriteComponent());
			manager.AddComponent(projectileEntity, new RigidBody2DComponent(projectileEntity, currentPosition));
		}
		private void EndOfProjectile(Entity entity)
		{
			manager.RemoveEntity(entity);
			Logger.Log("Projectile entity removed");
		}
		public void Dispose()
		{
			globalEvents.UnsubscriptAllEntityEvents(entity);
		}
	}
}
