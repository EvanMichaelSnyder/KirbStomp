using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using MyGame;


// Implement a concrete class for a displaying the static sprite
public class DisplayStaticSpriteCommand : ICommand
{
    private ISprite sprite;
    private Game1 game;
    public DisplayStaticSpriteCommand(Game1 game, ISprite sprite) 
    { 
        this.sprite = sprite; 
        this.game = game;
    }
    public void Execute()
    {
        // change current sprite
        game.currentSprite = sprite;
        // Console.WriteLine("Execute: Static sprite");
    }

}