using KirbStomp.Engine.ECSV2.EntityManagement;
using KirbStomp.Engine.ECSV2.Systems;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
	internal class CreateProjectileComponent : Entity
	{
		private Entity entity;
		private (string, string) animationName;
		private Keys activateKey;
		private EntityManager manager;
		public CreateProjectileComponent(Entity entity, (string, string) animation, Keys activateKey)
		{
			this.entity = entity;
			this.activateKey = activateKey;
			this.animationName = animation;
		}

	}
}
