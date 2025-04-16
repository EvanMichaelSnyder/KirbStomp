using KirbStomp.Interfaces;
using KirbStomp.Scripts.Projectiles;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;

public class ButtonUI : UIElement {
    private Sprite _buttonSprite;
    public AreaUI2D Area { get; set; }
    public EventHandler OnClick;
    public EventHandler OnClickEvent {get; set;}
    public ButtonUI(Sprite buttonSprite, Vector2 position) :
        base("ButtonUI", position) 
    {
        this._buttonSprite = buttonSprite;
        Initialize();
    }
    public ButtonUI(String name, Sprite buttonSprite, Vector2 position) :
        base(name, position) 
    {
        this._buttonSprite = buttonSprite;
        Initialize();
    }
    public void Initialize() {
        Vector2 globalScale = new Vector2((float)Game1.Get().GetScreenWindow().globalScaleX, (float)Game1.Get().GetScreenWindow().globalScaleY);
        float scale = _buttonSprite.GetScale();
        OnClickEvent = (sender, args) => {
            Console.WriteLine(Name + " Button clicked!");
        };
        Area = new AreaUI2D(new Rectangle((int)(Position.X * globalScale.X), (int)(Position.Y * globalScale.Y), (int)(globalScale.X * scale * _buttonSprite.GetSrcRectangle().Width), (int)(globalScale.Y * scale * (int)_buttonSprite.GetSrcRectangle().Height)));
        Area.OnClick += OnClickEvent;
        _buttonSprite.SetPosition(this.Position);
        Sprites.Add(_buttonSprite);
    }
    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
        if (this.IsVisible) {
            Area.Draw(spriteBatch);
        }
    }

    public void SetClickEvent(EventHandler e) {
        OnClickEvent = e;
    }
}
