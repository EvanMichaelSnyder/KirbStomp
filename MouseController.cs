using System;
using System.Data;
using Microsoft.Xna.Framework.Input;
// Implement a concrete class for a mouse controller

public class MouseController : IController
{
    private ICommand[] mouseCommands;
    public MouseController (ICommand[] mouseCommands) 
    { 
        this.mouseCommands = mouseCommands;
    }

    public void HandleMouseInput(int x, int y, bool isLeftClick, bool isRightClick)
    {
        // update the sprite with left mouse click, or quit with right mouse click
        int quad = 0;   // default to 0 to quit
        if(isRightClick || isLeftClick) {
            if(isLeftClick) {
                if(y < 240) {
                    if(x < 400) {
                        //quadrant 1
                        quad = 1;
                    } else {
                        //quadrant 2
                        quad = 2;
                    }
                } else {
                    if(x < 400) {
                        //quadrant 3
                        quad = 3;
                    } else {
                        //quadrant 4
                        quad = 4;
                    }
                }
                // Console.WriteLine($"Mouse: Right click,  Quadrant: {quad}");
            }
            else {
            //    Console.WriteLine($"Mouse: Left click,  Quadrant: {quad}");
            }
            // execute the command and update the sprite
            mouseCommands[quad].Execute();
        }
    }

    public void Update()
    {
        // handle the input based on the mouse state
        MouseState mouseState = Mouse.GetState();
        HandleMouseInput(mouseState.X, mouseState.Y, mouseState.LeftButton == ButtonState.Pressed, mouseState.RightButton == ButtonState.Pressed);
    }

}