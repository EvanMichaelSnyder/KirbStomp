using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.Components.IComponents;
using KirbStomp.Engine.ECSV2.Components.Tags;
using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
	internal class BasicProjectileSystem : IUpdatableSystem
	{

		// Call is sent to this class to create a projectile in the direction of the entity which called the function
		// The system has a list of projectiles which it processes on each frame,
		// Projectiles has a set life span, dependant on their path or whether they should be destroyed


		// Container of projectiles to create
		// Create Projectile with animation, and set trajectory
		// Update detials on the remaining lifespan of each projectile
		private EntityManager manager;
		public BasicProjectileSystem()
		{
			manager = EntityManager.GetInstance();
		}
		public void Update(float deltaTime)
		{
			Vector2 currentVel;
			foreach(var (entity, projectileComponent, rigidBody) in manager.GetEntitiesWithComponents<ProjectileComponent, RigidBody2DComponent>())
			{
				currentVel = projectileComponent.GetCurrentProjectileVelocity();
				rigidBody.SetVelocity(currentVel);
				projectileComponent.UpdateProjectile(deltaTime);
			}
		}

	}
}
