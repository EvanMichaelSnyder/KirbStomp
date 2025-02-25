using KirbStomp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
public class ScreenWindow {
    public double globalXBoundMax;
    public double globalYBoundMax;
    public double globalScaleX;
    public double globalScaleY;
    public double globalAspectRatio;

    private GraphicsDeviceManager _graphics;
    public ScreenWindow(GraphicsDeviceManager graphics) {
        globalXBoundMax = 800;
        globalYBoundMax = 480;
        globalScaleX = 1.0;
        globalScaleY = 1.0;
        globalAspectRatio = 5 / 3.0;
        this._graphics = graphics;
    }
    public (int width, int height) GetAdjustedWindowSize()
    {
        // Get screen dimensions
        int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        // Calculate 5:3 window size
        double maxWidth = screenWidth * 0.80; // 80% of screen width
        double maxHeight = screenHeight * 0.80; // 80% of screen height

        double windowWidth = maxWidth;
        double windowHeight = maxWidth / globalAspectRatio;

        // Return the calculated width and height as integers
        return ((int)windowWidth, (int)windowHeight);
    }
    public void UpdateWindowSize()
    {
        (int width, int height) = GetAdjustedWindowSize();
        globalScaleX = width / globalXBoundMax;
        globalScaleY = height / globalYBoundMax;
        Console.WriteLine("globalXBoundMax: " + globalXBoundMax + " globalYBoundMax: " + globalYBoundMax);
        Console.WriteLine("width: " + width + " height: " + height);
        // Console.WriteLine("Scale X: " + globalScaleX + " Scale Y: " + globalScaleY);
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.ApplyChanges();
    }
}