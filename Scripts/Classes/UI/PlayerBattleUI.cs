using KirbStomp.Interfaces;
using KirbStomp.Scripts.Projectiles;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;
using System.Collections.Generic;
public class PlayerBattleUI : UIElement{
    
    private Character _character;
    private SpriteFont _font;
    private Sprite _portraitBackground;
    private Sprite _portraitIcon;
    private Sprite _nameHolder;
    private SpriteString _nameString;
    private SpriteString _healthString;
    private Sprite _stockIconOne;
    private Sprite _stockIconTwo;
    private Sprite _stockIconThree;
    private Sprite _sourceIcon;
    private Dictionary<string, Vector2> _spriteOffsets;
   public PlayerBattleUI(string name, Vector2 position, Character character, SpriteFont font, Sprite portraitBackground, Sprite nameHolder)
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
        _nameString = new SpriteString(_font, _character.GetCharacterUIData().CharacterName.ToUpper(), 0.6f);
        _healthString = new SpriteString(_font, _character.GetHealth().ToString(), 2f);
        _sourceIcon = _character.GetCharacterUIData().StockIcon;
        _portraitIcon = _character.GetCharacterUIData().PortraitIcon;

        Texture2D iconTexture = _sourceIcon.GetTexture();
        Rectangle iconSourceRectangle = _sourceIcon.GetSrcRectangle();
        float iconScale = _sourceIcon.GetScale();

        _stockIconOne = new Sprite(_sourceIcon.Name + "1", iconTexture, iconSourceRectangle, iconScale);
        _stockIconTwo = new Sprite(_sourceIcon.Name + "2", iconTexture, iconSourceRectangle, iconScale);
        _stockIconThree = new Sprite(_sourceIcon.Name + "3", iconTexture, iconSourceRectangle, iconScale);
        _stockIconOne.SetAlphaPercent(2);
        _stockIconTwo.SetAlphaPercent(15);
        _stockIconThree.SetAlphaPercent(100);

        _nameString.SetPosition(Position + new Vector2(95 - _font.MeasureString(_character.GetCharacterUIData().CharacterName.ToUpper()).X / 2, 65));
        _healthString.SetPosition(Position + new Vector2(102, 20));
        _portraitBackground.SetPosition(Position);
        _nameHolder.SetPosition(Position + new Vector2(8, 67));
        _portraitIcon.SetPosition(Position + new Vector2(28, 7));
        _stockIconOne.SetPosition(Position + new Vector2(80, 8));
        _stockIconTwo.SetPosition(Position + new Vector2(96, 8));
        _stockIconThree.SetPosition(Position + new Vector2(112, 8));
        
        Sprites.Add(_portraitBackground);
        Sprites.Add(_nameHolder);
        Sprites.Add(_portraitIcon);
        Sprites.Add(_stockIconOne);
        Sprites.Add(_stockIconTwo);
        Sprites.Add(_stockIconThree);
        TextSprites.Add(_nameString);
        TextSprites.Add(_healthString);

        //Events
        _character.OnHealthChange += Character_OnHealthChange;
        _character.OnLivesChange += Character_OnLivesChange;
    }
    private void Character_OnHealthChange(object sender, Character.OnHealthChangeEventArgs e)
    {
        _healthString.SetContent(e.health.ToString());
    }
    private void Character_OnLivesChange(object sender, Character.OnLivesChangeEventArgs e)
    {
        if (e.lives == 3)
        {
            _stockIconOne.Visible = true;
            _stockIconTwo.Visible = true;
            _stockIconThree.Visible = true;
        }
        else if (e.lives == 2)
        {
            _stockIconThree.Visible = false;

        }
        else if (e.lives == 1)
        {
            _stockIconTwo.Visible = false;   
        }
        else if (e.lives == 0)
        {
            _stockIconOne.Visible = false;
        }
    }
}