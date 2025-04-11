using KirbStomp.Interfaces;
using KirbStomp.Scripts.Projectiles;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;

public class ButtonUI : UIElement {
    private Sprite _buttonSprite;
    public ButtonUI(Sprite buttonSprite, Vector2 position) :
        base("ButtonUI", position) 
    {
        this._buttonSprite = buttonSprite;
        Initialize();
    }
    public void Initialize() {
        _buttonSprite.SetPosition(this.Position);
        Sprites.Add(_buttonSprite);
    }
}
