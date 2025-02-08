using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Engine.ecs;
using KirbStomp.Engine.Inputs;
using Microsoft.Xna.Framework.Input;

namespace KirbStomp.Engine.ecs.ComponentsV1
{
    public class PlayerMovement : Component
    {
        private float maxRunSpeed;
        private float maxJumpTime;
        private float jumpTime;
        private float jumpSpeed;

        private float xPosAdjust, yPosAdjust;

        private bool isJumping;

        private GlobalInputs input;
        private Keys jumpKey;
        private Keys moveLeftKey;
        private Keys moveRightKey;

        public PlayerMovement(float maxRunSpeed, float maxJumpTime, float jumpSpeed, Keys jumpKey, Keys moveLeftKey, Keys moveRightKey)
        {
            this.maxRunSpeed = maxRunSpeed;
            jumpTime = 0;
            this.maxJumpTime = maxJumpTime;
            this.jumpSpeed = jumpSpeed;

            this.jumpKey = jumpKey;
            this.moveLeftKey = moveLeftKey;
            this.moveRightKey = moveRightKey;
            input = Game1.get().GetGlobalInputs();

            yPosAdjust = 0;
            xPosAdjust = 0;

            isJumping = false;
        }
        public override void Update(float dt)
        {
            CheckJump(dt);
            CheckMove(dt);

            //update gameobject pos based on input
            Vector2 pos = gameObject.getPosition();
            pos.X += xPosAdjust;
            pos.Y += yPosAdjust;
            gameObject.setPosition(pos);
            //reset x/y pos adjust for next update
            xPosAdjust = 0;
            yPosAdjust = 0;
        }

        private void CheckMove(float dt)
        {
            if (input.IsInputPressed(moveRightKey))
            {
                xPosAdjust += maxRunSpeed * dt;
            }

            if (input.IsInputPressed(moveLeftKey))
            {
                xPosAdjust -= maxRunSpeed * dt;
            }
        }

        private void CheckJump(float dt)
        {
            //TODO tempory to reset jumptime, need to account if touching ground
            if (input.IsInputPressed(Keys.R))
            {
                //resets jump time
                jumpTime = 0;
            }


            if (input.IsInputPressed(jumpKey) && jumpTime < maxJumpTime)//jump
            {
                yPosAdjust -= jumpSpeed * dt;
                jumpTime += dt;
                isJumping = true;
            }
            else if (input.IsInputJustReleased(jumpKey) && isJumping)//check if jump ended early
            {
                isJumping = false;
                jumpTime = maxJumpTime;

            }
        }


    }
}
