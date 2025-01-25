using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using MyGame;
using System;


// Implement a concrete class for a displaying the animated moving sprite
public class DisplayAnimatedMovingSpriteCommand : ICommand
{
    private ISprite sprite;
    private Game1 game;
    public DisplayAnimatedMovingSpriteCommand(Game1 game, ISprite sprite) 
    { 
        this.sprite = sprite; 
        this.game = game;
    }
    public void Execute()
    {
        // change sprite
        game.currentSprite = sprite;
        // Console.WriteLine("Execute: Animated Moving sprite");
    }

}