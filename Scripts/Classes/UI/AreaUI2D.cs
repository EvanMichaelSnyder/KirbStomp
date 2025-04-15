using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp;
public class AreaUI2D {
    public string Name { get; set; }
    public Rectangle Area { get; set; }
    public AreaUI2D(string name, Rectangle area) {
        this.Name = name;
        this.Area = area;
    }
    public AreaUI2D(Rectangle area) {
        this.Name = "AreaUI2D";
        this.Area = area;
    }

    public void Draw(SpriteBatch spriteBatch) {
        // Draw the area if needed
        // For example, you can draw a rectangle around the area
        Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        texture.SetData(new[] { Color.White });
        spriteBatch.Draw(texture, Area, Color.Red * 0.5f); // Semi-transparent red rectangle
    }

    public bool Contains(Vector2 point) {
        return Area.Contains(point);
    }

}