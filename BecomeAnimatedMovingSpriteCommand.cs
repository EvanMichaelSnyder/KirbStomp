using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sprint0.LuigiSpriteSheetMapping;

namespace Sprint0
{
    internal class BecomeAnimatedMovingSpriteCommand : ICommand
    {
        CharacterLuigi _character;
        public BecomeAnimatedMovingSpriteCommand(CharacterLuigi character)
        {
            _character = character;
        }
        public void Execute()
        {
            if (_character.sprite is not AnimatedMovingSprite)
            {
                _character.stateChange(LuigiState.RunningLeft);
            }
        }
    }
}