using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Scripts.Projectiles;

namespace KirbStomp.Scripts.Classes.Collision.CollisionHandlers
{
    public static class CharacterCollisionHandlers
    {

        //Body Platform
        internal static void HandlePlatformCollision(Character character, CollisionContext context)
        {
            // Access via parameter
            if (context.Self.Velocity.Y >= 0)
            {
                character.Position.Y -= context.Intersection.Height - 2f;
                character.ActionList.AddAction(GameButtons.HitGround);
                character.Velocity.Y = 0;
                character.StateMachine.State.IsGrounded = true;
                character.StateMachine.State.ResetJumps();
            }

        }

        //Body, Attack
        internal static void HandleAttackCollision(Character character, CollisionContext context)
        {
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
            }
        }



    }
}
