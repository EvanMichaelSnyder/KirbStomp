using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//concrete class for the non-moving, animated sprite
public class AnimatedStaticSprite : ISprite
{
    private Texture2D texture;
    private Vector2 location;
    private Rectangle[] spriteFrames;
    private int scaleFactor;
    private int firstFrame;
    private int currentFrame;
    private int lastFrame;
    private double elapsedTime = 0; 
    private double frameDuration = 100;
    public AnimatedStaticSprite(Texture2D texture, Vector2 location, Rectangle[] spriteFrames, int firstFrame, int lastFrame, int scaleFactor)
    {
        this.texture = texture;
        this.location = location;
        this.spriteFrames = spriteFrames;
        currentFrame = firstFrame;
        this.firstFrame = firstFrame;
        this.lastFrame = lastFrame;
        this.scaleFactor = scaleFactor;

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // rectangle with sprite location and size to draw, from the texture sprite sheet
        Rectangle sourceRectangle = spriteFrames[currentFrame];
        // rectangle that represents where the texture will be drawn
        Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width*scaleFactor, sourceRectangle.Height*scaleFactor);
    
        //draw the correct part of the texture on the screen
        spriteBatch.Begin();
        spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        spriteBatch.End();
    }
    public void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        // Check if enough time has passed to change the frame 
        if (elapsedTime >= frameDuration) 
        { 
            //reset the time and moves to the next frame
            elapsedTime = 0;
            currentFrame++; 
            if (currentFrame > lastFrame) 
            {
                //reset to the first frame
                currentFrame = firstFrame; 
            }
        }
    }
}


