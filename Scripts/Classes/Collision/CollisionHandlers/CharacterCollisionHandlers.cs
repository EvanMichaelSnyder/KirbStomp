using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                character.Position.Y -= context.Intersection.Height - 1;
                character.ActionList.AddAction(GameButtons.HitGround);
                character.Velocity.Y = 0;
                character.StateMachine.State.IsGrounded = true;
                character.StateMachine.State.ResetJumps();
            }

        }

        //Body, Attack
        internal static void HandleAttackCollision(Character character, CollisionContext context)
        {
            // Access via parameter
        }



    }
}
