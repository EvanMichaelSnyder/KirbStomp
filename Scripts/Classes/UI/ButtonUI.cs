using KirbStomp.Interfaces;
using KirbStomp.Scripts.Projectiles;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;

public class ButtonUI : IUI {
    private Sprite _buttonSprite;
    private Vector2 _position;
    public ButtonUI(Sprite buttonSprite, Vector2 position) {
        this._buttonSprite = buttonSprite;
        this._position = position;
    }
    public void Initialize() {
        throw new NotImplementedException();
    }
    public void Draw(SpriteBatch spriteBatch) {
        _buttonSprite.Draw(spriteBatch, this._position);
    }
}
