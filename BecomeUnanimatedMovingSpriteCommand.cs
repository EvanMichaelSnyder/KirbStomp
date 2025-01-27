using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Sprint0.LuigiSpriteSheetMapping;
namespace Sprint0
{
    internal class BecomeUnanimatedMovingSpriteCommand : ICommand
    {
        CharacterLuigi _character;
        public BecomeUnanimatedMovingSpriteCommand(CharacterLuigi character)
        {
            _character = character;
        }
        public void Execute()
        {
            if (_character.sprite is not UnanimatedMovingSprite)
            {
                _character.stateChange(LuigiState.JumpingRight);
            }
        }
    }
}

