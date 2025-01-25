using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MyGame;
using System;


// Implement a concrete class for a displaying the animated static sprite
public class DisplayAnimatedStaticSpriteCommand : ICommand
{
    private ISprite sprite;
    private Game1 game;
    public DisplayAnimatedStaticSpriteCommand(Game1 game, ISprite sprite) 
    { 
        this.sprite = sprite; 
        this.game = game;
    }
    public void Execute()
    {
        // change sprite
        game.currentSprite = sprite;
        // Console.WriteLine("Execute: Animated Static sprite");
    }

}