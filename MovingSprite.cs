using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//concrete class for the moving, non-animated sprite
public class MovingSprite : ISprite
{
    private Texture2D texture;
    private Vector2 location;
    private Rectangle sourceRectangle;
    private int scaleFactor;
    private Vector2 speed;
    private Vector2 minLocation;
    private Vector2 maxLocation;
    private bool moveToRight;
    private bool moveUp;

    public MovingSprite(Texture2D texture, Vector2 location, Rectangle sourceRectangle, int scaleFactor, Vector2 speed, Vector2 minLocation, Vector2 maxLocation) {
        this.texture = texture; 
        this.location = location;
        this.sourceRectangle = sourceRectangle;
        this.scaleFactor = scaleFactor;
        this.speed = speed;
        this.minLocation = minLocation;
        this.maxLocation = maxLocation;
        moveToRight = true;
        moveUp = true;
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
        //use elapsed time to calculate the position of the sprite
        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Determine move distance based on speed and time 
        float moveDistanceX = speed.X * elapsedTime;
        float moveDistanceY = speed.Y * elapsedTime;


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