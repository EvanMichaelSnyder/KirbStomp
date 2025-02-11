using KirbStomp.Engine.ECSV2.Components.IECSComponents;
using KirbStomp.Engine.ECSV2.Components;
using KirbStomp.Engine.ECSV2.Systems.ISystems;
using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ECSV2.ECSEntityManagement;

namespace KirbStomp.Engine.ECSV2.Systems
{
    internal class PlayerMovementSystem : IUpdatableSystem
    {
        private EntityManager manager;
        private GlobalInputs inputs;
        public PlayerMovementSystem()
        {
            manager = EntityManager.GetInstance();
            inputs = GlobalInputs.GetInstance();
        }
        public void Update(float deltaTime)
        {
            List<Entity> entities = this.getPlayerMovementEnitity();
            foreach (Entity entity in entities)
            {
                PlayerControlsComponent playerControls = this.manager.GetComponent<PlayerControlsComponent>(entity);
                ExamplePlayerState playerState = this.manager.GetComponent<ExamplePlayerState>(entity);
                playerState.normalMovementDirection.X = Convert.ToInt32(inputs.IsInputPressed(playerControls.moveRightKey)) - Convert.ToInt32(inputs.IsInputPressed(playerControls.moveLeftKey));
                playerState.isWalking = playerState.normalMovementDirection.X != 0;
                playerState.isFalling = manager.GetComponent<RigidBody2DComponent>(entity).getVelocity().Y != 0;
                playerState.jumped = inputs.IsInputJustPressed(playerControls.jumpKey) && !playerState.isFalling;
                if (playerState.jumped) Debug.WriteLine("Jumped!");
                if (playerState.isWalking) Debug.WriteLine("WALKING, AND I SET THE STATE OF PlayerState Component");
            }
        }

        private List<Entity> getPlayerMovementEnitity()
        {
            List<Entity> list = new List<Entity>();
            List<Entity> entities = this.manager.getEntities();
            foreach(Entity entity in entities)
            {
                if(this.manager.hasComponent<ExamplePlayerState>(entity) && this.manager.hasComponent<PlayerControlsComponent>(entity))
                {
                    list.Add(entity);
                }
                
            }
            return list;
        }
    }
}
