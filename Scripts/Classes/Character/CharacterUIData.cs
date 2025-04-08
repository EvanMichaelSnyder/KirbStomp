using KirbStomp;
using System;
using Microsoft.Xna.Framework;
using KirbStomp.Scripts.Projectiles;
public class CharacterUIData
{
    public Sprite StockIcon {get; private set;}
    public Sprite PortraitIcon {get; private set;}
    public string CharacterName {get; private set;}

    public CharacterUIData(Sprite stockIcon, Sprite portraitIcon, string characterName)
    {
        this.StockIcon = stockIcon;
        this.PortraitIcon = portraitIcon;
        this.CharacterName = characterName;
    }
}