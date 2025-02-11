using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class PlayerStateMachineSystem : IUpdatableSystem
    {
        private EntityManager manager;
        public PlayerStateMachineSystem()
        {
            manager = EntityManager.GetInstance();
        }
        public void Update(float deltaTime)
        {
            List<Entity> entities = this.getPlayerStateEntity();
            foreach (Entity entity in entities)
            {
                RigidBody2DComponent rigidBody = this.manager.GetComponent<RigidBody2DComponent>(entity);
                ExamplePlayerState playerState = this.manager.GetComponent<ExamplePlayerState>(entity);
                rigidBody.setXVelocity(playerState.normalMovementDirection.X * playerState.movementVel);
                rigidBody.setYVelocity(rigidBody.getVelocity().Y - Convert.ToInt32(playerState.jumped) * playerState.jumpVelocity);
            }
        }

        private List<Entity> getPlayerStateEntity()
        {
            List<Entity> list = new List<Entity>();
            List<Entity> entities = this.manager.getEntities();
            foreach (Entity entity in entities)
            {
                if (this.manager.hasComponent<ExamplePlayerState>(entity))
                {
                    list.Add(entity);
                }
            }
            return list;
        }
    }
}
