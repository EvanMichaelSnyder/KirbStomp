using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// concrete class for the moving, animated sprite
public class AnimatedMovingSprite : ISprite
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
    private Vector2 speed;
    private Vector2 minLocation;
    private Vector2 maxLocation;
    private bool moveToRight;
    private bool moveUp;
    public AnimatedMovingSprite(Texture2D texture, Vector2 location, Rectangle[] spriteFrames, int firstFrame, int lastFrame, int scaleFactor, Vector2 speed, Vector2 minLocation, Vector2 maxLocation)
    {
        this.texture = texture;
        this.location = location;
        this.spriteFrames = spriteFrames;
        currentFrame = firstFrame;
        this.firstFrame = firstFrame;
        this.lastFrame = lastFrame;
        this.scaleFactor = scaleFactor;
        this.speed = speed;
        this.minLocation = minLocation;
        this.maxLocation = maxLocation;
        moveToRight = true;
        moveUp = true;
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
            // reset the time and moves to the next frame
            elapsedTime = 0;
            currentFrame++; 
            if (currentFrame > lastFrame) 
            {
                // reset to the first frame
                currentFrame = firstFrame; 
            }
        }

        // update moving
        // use the time elapsed to calculate the position of the sprite
        float timeChange = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Determine move distance based on speed and time 
        float moveDistanceX = speed.X * timeChange;
        float moveDistanceY = speed.Y * timeChange;


        // move the sprite to the left and right with the set speed
        if(moveToRight) {
            location = new Vector2(location.X + moveDistanceX, location.Y);
        } else {
            location = new Vector2(location.X - moveDistanceX, location.Y);
        }

        // move the sprite up and down with the set speed
        if(moveUp) {
            location = new Vector2(location.X, location.Y - moveDistanceY);
        } else {
            location = new Vector2(location.X, location.Y + moveDistanceY);
        }
        
        // detect x boundary, reverse the direction to move back and fourth
        if(minLocation.X != maxLocation.X) {
            if(location.X <= minLocation.X) {
                // reset the location to min
                location = new Vector2(minLocation.X, location.Y);
                // move to the right
                moveToRight = true;
            } else if (location.X >= maxLocation.X) {
                // reset the location to max
                location = new Vector2(maxLocation.X, location.Y);
                // move to the left
                moveToRight = false;
            }
        }
        
        //detect Y boundary, reverse the direction to bounce up and down
        if(minLocation.Y != maxLocation.Y) {
            if(location.Y <= minLocation.Y) {
                // reset the location to min and change to moving down
                location = new Vector2(location.X, minLocation.Y);
                moveUp = false;
            } else if (location.Y >= maxLocation.Y) {
                // reset the location to max and change to moving up
                location = new Vector2(location.X, maxLocation.Y);
                moveUp = true;
            }
        }
    }
}