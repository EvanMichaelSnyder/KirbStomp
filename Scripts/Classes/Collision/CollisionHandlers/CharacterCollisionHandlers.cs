using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Classes.Carriers;
using KirbStomp.Scripts.Classes.Platforms;
using KirbStomp.Scripts.Projectiles;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.Collision.CollisionHandlers
{
    public static class CharacterCollisionHandlers
    {

        //Body Boundary
        internal static void HandleBoundaryCollision(Character character, CollisionContext context)
        {
            character.Respawn();
        }

        //Body Platform
        internal static void HandlePlatformCollision(Character character, CollisionContext context)
        {
            // Access via parameter
            /*
            if (context.Self.Velocity.Y >= 0)
            {
                    character.Position.Y -= context.Intersection.Height - 4;
                    character.ActionList.AddAction(GameButtons.HitGround);
                    character.Velocity.Y = 0;
                    //character.Velocity.X = 0;
                    character.StateMachine.State.IsGrounded = true;
                    character.StateMachine.State.ResetJumps();
                }
            */
            if (context.Self.Velocity.Y >= 0)
            {

                /*
                if (context.Other.GetType() == typeof(Platform))
                {
                   
                    Platform platform = (Platform)context.Other;
                    if (character._bodyCarrier.HitboxManager.GetApproximation().Bottom - context.GameTime.ElapsedGameTime.TotalSeconds * character.Velocity.Y <= platform.PlatformCarrier.HitboxManager.GetApproximation().Top + 10)
                    {
                        character.Position.Y = platform.PlatformCarrier.HitboxManager.GetApproximation().Top - character._bodyCarrier.HitboxManager.GetApproximation().Height + 2;
                        character.ActionList.AddAction(GameButtons.HitGround);
                        character.Velocity.Y = 0;
                        //character.Velocity.X = 0;
                        character.StateMachine.State.IsGrounded = true;
                        character.StateMachine.State.ResetJumps();
                    }
                    */

                    character.Position.Y -= context.Intersection.Height - 4;
                    character.ActionList.AddAction(GameButtons.HitGround);
                    character.Velocity.Y = 0;
                    //character.Velocity.X = 0;
                    character.StateMachine.State.IsGrounded = true;
                    character.StateMachine.State.ResetJumps();
            }
        }

        //Body, Attack
        internal static void HandleAttackCollision(Character character, CollisionContext context)
        {
            // Access via parameter
            character.ActionList.AddAction(GameButtons.GotHit);

            //other character relevant info
            if (context.Other.GetType() == typeof(Character))
            {

                Character otherCharacter = (Character)context.Other;

                character.TakeDamage(otherCharacter._attackCarrier.getDamage());

                character.Velocity.X = 200 * character._health / 100;
                if (otherCharacter.StateMachine.State.FacingDirection == DirectionEnum.Left)
                {
                    character.Velocity.X *= -1;
                }
                character.Velocity.Y = -400;
                //Debug.WriteLine(context.Self + " " + context.SelfType + " " + context.Other + " " + context.OtherType + " " + context.Intersection.ToString());
            }
            else if (context.Other.GetType().BaseType == typeof(AProjectile))
            {
                //Debug.WriteLine("projectile collision detected");
                AProjectile projectile = (AProjectile)context.Other;
                AttackCarrier attackCarrier = projectile.GetAttackCarrier();
                character.TakeDamage(attackCarrier.getDamage());

                character.Velocity.X = (float)(200 * (100+character._health) / 100.0);
                if (projectile.GetVelocity().X < 0)
                {
                    character.Velocity.X *= -1;
                }
                character.Velocity.Y = -400;
                //Debug.WriteLine(context.Self + " " + context.SelfType + " " + context.Other + " " + context.OtherType + " " + context.Intersection.ToString());
            }
        }



    }
}
