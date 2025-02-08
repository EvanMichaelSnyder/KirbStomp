using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2Test.EntityObjects
{
    internal class SwappingItems
    {
        public ECSEntity CreateSwappingItemsEntity()
        {
            ECSManager manager = ECSManager.GetInstance();
            ECSEntity entity = manager.CreateEntity();
            manager.AddComponent(entity, new SpriteComponent(default, new(), new(), Color.White));
            manager.AddComponent(entity, new AnimationComponent("Runnnning"));
            manager.AddComponent(entity, new RigidBody2DComponent(new Vector2(0, 0), new Vector2(50, 50), new Vector2(0, 0)));
			manager.AddComponent(entity, new CycleItems());
            return entity;

        }

    }
}
