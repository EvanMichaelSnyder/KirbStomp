using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using KirbStomp.Scripts.Classes.Collision;

namespace KirbStomp
{
    class CharacterMovement : PhysicsComponent
    {
        private MovementStats _movementStats;
        private CharacterStateMachine _stateMachine;
        private Dictionary<StateEnum, Action> _movementBehaviors;

        public CharacterMovement(MovementStats movementStats, CharacterStateMachine stateMachine)
        {
            _movementStats = new MovementStats();
            _stateMachine = stateMachine;
            // Matches all states with their associated velocity changes.
            _movementBehaviors = new Dictionary<StateEnum, Action>
            {
                { StateEnum.AirMove, AirMove },
                { StateEnum.Walk, Walk },
                { StateEnum.Run, Run },
                { StateEnum.Sprint, Sprint },
                { StateEnum.SlideTurn, SlideTurn },
                { StateEnum.Idle, Idle },
                { StateEnum.Landing, Landing },
                { StateEnum.SpecialUp, SpecialUp },
                { StateEnum.Jump, Jump }
            };
        }

        public void ApplyMovementBehavior()
        {
            // Finds the correct behavior to match the current state of the character
            // and invokes that action.
            if(_movementBehaviors.TryGetValue(_stateMachine.State.CurrentState, out Action behavior))
            {
                behavior.Invoke();
            }
        }

        /*
         * List of all actions that are called given the
         * current state of the character.
         * 
         * 
         */
        private void AirMove()
        {
            Velocity.X = 300;
            SetDirection();
        }
        private void Walk()
        {
            Velocity.X = 40;
            SetDirection();
        }

        private void Run()
        {
            Velocity.X = 80;
            SetDirection();
        }
        private void Sprint()
        {
            Velocity.X = 300;
            SetDirection();
        }
        private void SlideTurn()
        {
            Velocity.X = 10;
            SetDirection();
        }
        private void Idle()
        {
            Velocity.X = 0;
        }
        private void Landing()
        {
            Velocity.X = 0;
        }
        private void SpecialUp()
        {
            Velocity.Y = 150;
        }
        private void Jump()
        {
            if(_stateMachine.State.GetFrameIndex() == 0) { Velocity.Y = -510; }
        }
        private void SetDirection()
        {
            if (_stateMachine.State.DesiredMovementDirection == DirectionEnum.Left) { Velocity.X *= -1; }
        }
    }
}