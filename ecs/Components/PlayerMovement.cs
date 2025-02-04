using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Inputs;
using Microsoft.Xna.Framework.Input;

namespace KirbStomp.ecs.Components
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

        public PlayerMovement(float maxRunSpeed,  float maxJumpTime, float jumpSpeed, Keys jumpKey, Keys moveLeftKey, Keys moveRightKey) 
        {
            this.maxRunSpeed = maxRunSpeed;
            this.jumpTime = 0;
            this.maxJumpTime = maxJumpTime;
            this.jumpSpeed = jumpSpeed;

            this.jumpKey = jumpKey;
            this.moveLeftKey = moveLeftKey;
            this.moveRightKey = moveRightKey;
            this.input = Game1.get().GetGlobalInputs();

            this.yPosAdjust = 0;
            this.xPosAdjust = 0;

            this.isJumping = false;
        } 
        public override void Update(float dt)
        {
            CheckJump(dt);
            CheckMove(dt);

            //update gameobject pos based on input
            Vector2 pos = this.gameObject.getPosition();
            pos.X += this.xPosAdjust;
            pos.Y += this.yPosAdjust;
            this.gameObject.setPosition(pos);
            //reset x/y pos adjust for next update
            this.xPosAdjust = 0;
            this.yPosAdjust = 0;
        }

        private void CheckMove(float dt)
        {
            if (this.input.IsInputPressed(this.moveRightKey))
            {
                this.xPosAdjust += this.maxRunSpeed * dt;
            }

            if (this.input.IsInputPressed(this.moveLeftKey))
            {
                this.xPosAdjust -= this.maxRunSpeed * dt;
            }
        }

        private void CheckJump(float dt)
        {
            //TODO tempory to reset jumptime, need to account if touching ground
            if(input.IsInputPressed(Keys.R))
            {
                //resets jump time
                this.jumpTime = 0;
            }
            

            if(input.IsInputPressed(this.jumpKey) && this.jumpTime < this.maxJumpTime)//jump
            {
                this.yPosAdjust -= this.jumpSpeed * dt;
                this.jumpTime += dt;
                this.isJumping = true;
            }
            else if (input.IsInputJustReleased(this.jumpKey) && this.isJumping)//check if jump ended early
            {
                this.isJumping = false;
                this.jumpTime = this.maxJumpTime;

            }
        }


    }
}
