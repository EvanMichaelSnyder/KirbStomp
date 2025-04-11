// using KirbStomp.Interfaces;
// using KirbStomp.Scripts.Projectiles;
// using System;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using KirbStomp;
// public class PlayerBattleUI : IUI {

//     private SpriteFont _font;
//     private Sprite _stockIcon;
//     private Sprite _portraitIcon;
//     private Sprite _portraitBackground;
//     private Sprite _nameHolder;
//     private string _characterName;
//     private Character _character;
//     private Vector2 _position;

//     private SpriteString _nameString;
//     private SpriteString _healthString;

//     public PlayerBattleUI(SpriteFont font, Character character, Vector2 position, Sprite portraitBackground, Sprite nameHolder) {
//         this._font = font;
//         this._character = character;
//         this._position = position;
//         this._characterName = character.GetCharacterUIData().CharacterName;
//         this._portraitIcon = character.GetCharacterUIData().PortraitIcon;
//         this._portraitBackground = portraitBackground;
//         this._nameHolder = nameHolder;
//         this._stockIcon = character.GetCharacterUIData().StockIcon;
//         _nameString = new SpriteString(_font, _characterName, 0.6f);
//         _healthString = new SpriteString(_font, _character.GetHealth().ToString(), 2f);
//     }

//     public void Initialize() {
//         throw new NotImplementedException();
//     }
//     public void Draw(SpriteBatch spriteBatch) {
//         _portraitBackground.Draw(spriteBatch, this._position);
//         _portraitIcon.Draw(spriteBatch, this._position + new Vector2(28, 7));
//         Vector2 nameHolderPosition = this._position + new Vector2(8, 67);
//         _nameHolder.Draw(spriteBatch, nameHolderPosition);
//         _stockIcon.Draw(spriteBatch, this._position + new Vector2(80, 8), 2);
//         _stockIcon.Draw(spriteBatch, this._position + new Vector2(96, 8), 15);
//         _stockIcon.Draw(spriteBatch, this._position + new Vector2(112, 8));
        
      
//         _nameString.Draw(spriteBatch, nameHolderPosition + new Vector2(95 - _font.MeasureString(_characterName).X / 2, -2));
//         _healthString.SetContent(_character.GetHealth().ToString());
//         _healthString.Draw(spriteBatch, this._position + new Vector2(102, 20));
//     }
// }