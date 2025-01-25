using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using MyGame;


// Implement a concrete class for a displaying the moving sprite
public class DisplayMovingSpriteCommand : ICommand
{
    private ISprite sprite;
    private Game1 game;
    public DisplayMovingSpriteCommand(Game1 game, ISprite sprite) 
    { 
        this.sprite = sprite; 
        this.game = game;
    }
    public void Execute()
    {
        // change sprite
        game.currentSprite = sprite;
        // Console.WriteLine("Execute: Moving sprite");
    }

}