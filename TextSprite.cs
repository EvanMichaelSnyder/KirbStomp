using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//concrete class for a non-moving, non-animated text sprite
public class TextSprite : ISprite
{
    private string text;
    private SpriteFont font;
    private Vector2 location;
    public TextSprite(SpriteFont font, string text, Vector2 location) 
    { 
        this.font = font;
        this.text = text; 
        this.location = location;
    }

    //draw the text sprite
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        // draw the text
        spriteBatch.DrawString(font, text, location, Color.Black);
        spriteBatch.End();
    }

    public void Update(GameTime gameTime)
    {
        //nothing to update
    }
}