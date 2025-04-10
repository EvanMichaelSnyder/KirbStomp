using KirbStomp.Interfaces;
using KirbStomp.Scripts.Projectiles;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;
using System.Collections.Generic;
public class PlayerBattleUINew : UIElement{
    
    private Character _character;
    private SpriteFont _font;
    private Sprite _portraitBackground;
    private Sprite _nameHolder;
    private Dictionary<string, Vector2> _spriteOffsets;
   public PlayerBattleUINew(string name, Vector2 position, Character character, SpriteFont font, Sprite portraitBackground, Sprite nameHolder)
        : base(name, position) 
    {
        this._character = character;
        this._font = font;
        this._portraitBackground = portraitBackground;
        this._nameHolder = nameHolder;
        
        Initialize();
    }
    public void Initialize()
    {
        _spriteOffsets = new Dictionary<string, Vector2>
        {
            { "PortraitBackground", new Vector2(0, 0) },
            { "PortraitIcon", new Vector2(28, 7) },
            { "NameHolder", new Vector2(8, 67) },
            { "StockIcon", new Vector2(80, 8) }
        };
    }
}