using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Sprint0.LuigiSpriteSheetMapping;

namespace Sprint0
{
    internal class BecomeAnimatedStillSpriteCommand : ICommand
    {
        CharacterLuigi _character;
        public BecomeAnimatedStillSpriteCommand(CharacterLuigi character)
        {
            _character = character;
        }
        public void Execute()
        {
            if (_character.sprite is not AnimatedStillSprite)
            {
                _character.stateChange(LuigiState.RunningRight);
            }
        }
    }
}
