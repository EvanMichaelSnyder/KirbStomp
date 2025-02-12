using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Interfaces;
using KirbStomp;

    internal class AllPurposeSprite : ISpriteComplete
    {
        private Texture2D _spriteSheet;
        private MarioState _currentFrame;
        private MarioState _currentAnimation;
        private double elapsedTime = 0;

        public MarioState GetAnimation()
        {
        return _currentAnimation;
        }

    //sole constructor must provide a spritesheet and a state mapping for that spritesheet
    public AllPurposeSprite(Texture2D spriteSheet, MarioState newState)
        {
            _spriteSheet = spriteSheet;
            ChangeAnimation(newState);
        }

        
        //after character state change this will be called
        public void ChangeAnimation(MarioState newFrame)
        {
            _currentAnimation = newFrame;
            _currentFrame = newFrame;
        }
        public void ChangeFrame(MarioState newFrame)
        {
            _currentAnimation = newFrame;
            _currentFrame = newFrame;
        }

    public void Draw(SpriteBatch spriteBatch, Vector2 location, GameTime gameTime)
        {
            Rectangle sourceRectangle;
            Rectangle destinationRectangle;

            //grab entry from dictionary
            var entry = MarioSpriteSheetMapping.StateToSpriteMap[_currentFrame];

            sourceRectangle = new Rectangle(entry.sprite.X-1, entry.sprite.Y-1, entry.sprite.Width+1, entry.sprite.Height+1);

            //the great equation
            int xCoord = (int)(Game1.globalScaleX* (location.X + (entry.offsetAnimation.X+entry.offsetState.X) * MarioSpriteSheetMapping.MarioSpriteScale));
            int yCoord = (int)(Game1.globalScaleY * (location.Y + (entry.offsetAnimation.Y + entry.offsetState.Y) * MarioSpriteSheetMapping.MarioSpriteScale));
            int Width =(int)(Game1.globalScaleX * (entry.sprite.Width * MarioSpriteSheetMapping.MarioSpriteScale));
            int Height=(int)(Game1.globalScaleY * (entry.sprite.Height * MarioSpriteSheetMapping.MarioSpriteScale));

            destinationRectangle = new Rectangle(xCoord,yCoord,Width,Height);

            spriteBatch.Draw(_spriteSheet, destinationRectangle, sourceRectangle, Color.White);

            //animate after draw is complete
            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            // Check if enough time has passed to change the frame 
            if (elapsedTime >= entry.frameDuration)
            {
                elapsedTime = 0;
                ChangeFrame(entry.nextFrame);
            }
        }


    }