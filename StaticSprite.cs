using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//concrete class for the non-moving, non-animated sprite
public class StaticSprite : ISprite
{
    private Texture2D texture;  
    private Vector2 location;
    private Rectangle sourceRectangle;
    private int scaleFactor;
    public StaticSprite(Texture2D texture, Vector2 location, Rectangle sourceRectangle, int scaleFactor) 
    { 
        this.texture = texture; 
        this.location = location;
        this.sourceRectangle = sourceRectangle;
        this.scaleFactor = scaleFactor;
    } 
    public void Draw(SpriteBatch spriteBatch) 
    {  
        
        //rectangle that represents where the texture will be drawn
        Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width*scaleFactor, sourceRectangle.Height*scaleFactor);

        spriteBatch.Begin();
        //draw the sprite
        spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        spriteBatch.End();
    } 
    public void Update(GameTime gameTime) 
    { 
        // No update required for static sprite 
    }
}