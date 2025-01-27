using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using static Sprint0.LuigiSpriteSheetMapping;

namespace Sprint0
{
    internal class CharacterLuigi
    {
        private Vector2 position,velocity;
        private LuigiSpriteSheetMapping.LuigiState state;
        public ISprite sprite;

        public CharacterLuigi(Texture2D spriteSheet) 
        {
            position.X = 400;
            position.Y = 180;
            velocity = Vector2.Zero;
            state = LuigiState.IdleRight;
            sprite = new UnanimatedStillSprite(spriteSheet,state);
        }

        public void Update()
        {
            if(velocity.X != 0)
            {
                position.X += velocity.X;
                if(position.X <= 0)
                {
                    position.X = 800;
                }
            }
            if (velocity.Y != 0)
            {
                position.Y += velocity.Y;
                if (position.Y <=0)
                {
                    position.Y = 480;
                }
            }
            sprite.Update();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void stateChange(LuigiState newState)
        {
            if (state != newState)
            {
                state = newState;
                switch (newState)
                {
                    case LuigiState.IdleRight:
                        velocity = Vector2.Zero;
                        sprite = new UnanimatedStillSprite(sprite.GetSpriteSheet(), LuigiState.IdleRight);
                        break;
                    case LuigiState.RunningRight:
                        //This one just shows the animation it isnt proper as it doesnt have velocity
                        velocity = Vector2.Zero;
                        sprite = new AnimatedStillSprite(sprite.GetSpriteSheet(), LuigiState.RunningRight);
                        break;
                    case LuigiState.JumpingRight:
                        velocity.X = 0;
                        velocity.Y = -1;
                        sprite = new UnanimatedMovingSprite(sprite.GetSpriteSheet(), LuigiState.JumpingRight);
                        break;
                    case LuigiState.RunningLeft:
                        velocity.X = -1;
                        velocity.Y = 0;
                        sprite = new AnimatedMovingSprite(sprite.GetSpriteSheet(), LuigiState.RunningLeft);
                        break;

                }

            }

        }

    }
}
