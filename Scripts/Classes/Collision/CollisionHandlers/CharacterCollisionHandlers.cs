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
            if (character.ClockTime <= 0)
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


                    if (context.Other.GetType() == typeof(Platform) && character.StateMachine.State.fancyPlatformCollisionFlag)
                    {
                        /*
                        Platform platform = (Platform)context.Other;
                        Microsoft.Xna.Framework.Rectangle charaApprox = character._bodyCarrier.HitboxManager.GetApproximation();
                        Microsoft.Xna.Framework.Rectangle platformApprox = platform.PlatformCarrier.HitboxManager.GetApproximation();
                        if (charaApprox.Y + charaApprox.Height - character.Velocity.Y * context.GameTime.ElapsedGameTime.TotalSeconds <= platformApprox.Y + platformApprox.Height)
                        {
                            {
                                character.Position.Y = platformApprox.Y - charaApprox.Height/2;
                                character.ActionList.AddAction(GameButtons.HitGround);
                                character.Velocity.Y = 0;
                                //character.Velocity.X = 0;
                                character.StateMachine.State.IsGrounded = true;
                                character.StateMachine.State.ResetJumps();
                            }

                        }
                        */
                        Platform platform = (Platform)context.Other;
                        Microsoft.Xna.Framework.Rectangle charaApprox = character._bodyCarrier.HitboxManager.GetApproximation();
                        Microsoft.Xna.Framework.Rectangle platformApprox = platform.PlatformCarrier.HitboxManager.GetApproximation();
                        if (charaApprox.Y + charaApprox.Height - character.Velocity.Y * context.GameTime.ElapsedGameTime.TotalSeconds <= platformApprox.Y + platformApprox.Height)
                        {
                            character.Position.Y -= charaApprox.Height - (platformApprox.Y-charaApprox.Y)- 4;
                            character.ActionList.AddAction(GameButtons.HitGround);
                            character.Velocity.Y = 0;
                            //character.Velocity.X = 0;
                            character.StateMachine.State.IsGrounded = true;
                            character.StateMachine.State.ResetJumps();
                            //Kirby avert your eyes the if statement of doom will hurt you
                            StateEnum state = character.StateMachine.State.CurrentState;
                            if (state == StateEnum.SpecialNeutral || state == StateEnum.SpecialForward || state == StateEnum.SpecialBack || state == StateEnum.SpecialDown)
                            {
                                character.Velocity.X = 0;
                                //Debug.WriteLine("heyo");
                                character.snapVelocity = false;
                            }
                        }
                    }
                    else
                    {
                        character.Position.Y -= context.Intersection.Height - 4;
                        character.ActionList.AddAction(GameButtons.HitGround);
                        character.Velocity.Y = 0;
                        //character.Velocity.X = 0;
                        character.StateMachine.State.IsGrounded = true;
                        character.StateMachine.State.ResetJumps();


                        //Kirby avert your eyes the if statement of doom will hurt you
                        StateEnum state = character.StateMachine.State.CurrentState;
                        if (state == StateEnum.SpecialNeutral || state == StateEnum.SpecialForward || state == StateEnum.SpecialBack || state == StateEnum.SpecialDown)
                        {
                            character.Velocity.X = 0;
                            //Debug.WriteLine("heyo");
                            character.snapVelocity = false;
                        }
                    }
                }
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
                character.ClockTime = otherCharacter._attackCarrier.ClockTime;

                character.snapVelocity = false;
                var result = otherCharacter._attackCarrier.GetImpulseVector(character._bodyCarrier);
                character.Velocity = result.impulse * (float)(1.0 + character._health / 500.0); ;
            }
            else if (context.Other.GetType().BaseType == typeof(AProjectile))
            {
                //Debug.WriteLine("projectile collision detected");
                AProjectile projectile = (AProjectile)context.Other;
                AttackCarrier attackCarrier = projectile.GetAttackCarrier();
                character.TakeDamage(attackCarrier.getDamage());
                character.ClockTime = .25f;
                character.snapVelocity = false;

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
