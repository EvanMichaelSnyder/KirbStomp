using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Sprint0.LuigiSpriteSheetMapping;

namespace Sprint0
{
    internal class BecomeUnanimatedStillSpriteCommand : ICommand
    {
        CharacterLuigi _character;
        public BecomeUnanimatedStillSpriteCommand(CharacterLuigi character)
        {
            _character = character;
        }
        public void Execute()
        {
            if (_character.sprite is not UnanimatedStillSprite)
            {
                _character.sprite = new UnanimatedStillSprite(_character.sprite.GetSpriteSheet(), LuigiState.IdleRight);
                _character.stateChange(LuigiState.IdleRight);
            }
        }
    }
}
