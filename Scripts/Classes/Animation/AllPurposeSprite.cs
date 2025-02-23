using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbStomp.Interfaces;
using KirbStomp;
using System.Diagnostics;
using System.Diagnostics;

internal class AllPurposeSprite : ISpriteComplete
    {
    private Texture2D _spriteSheet;

    public AllPurposeSprite(Texture2D spriteSheet)
        {
            _spriteSheet = spriteSheet;
        }
    public void DrawHitbox(SpriteBatch spriteBatch, Rectangle HitBox)
    {
        //hitboxes come already scaled
        Rectangle scaledHitBox = new Rectangle(
            (int)(HitBox.X*Game1.globalScaleX),
            (int)(HitBox.Y*Game1.globalScaleY),
            (int)(HitBox.Width*Game1.globalScaleX),
            (int)(HitBox.Height*Game1.globalScaleY));
        Color color = new Color(100,100,100,100);
        spriteBatch.Draw(_spriteSheet, scaledHitBox, color);
    }

	public void Draw(SpriteBatch spriteBatch, Vector2 location, DirectionEnum direction, StateEnum state, int frame, string name)
		{
			Rectangle sourceRectangle;
			Rectangle destinationRectangle;

            //grab entry from dictionary
            var entry = AnimationRepository.GetFrameData(name, state, frame);
            sourceRectangle = entry.frame.ToRectangle(direction,entry.boundX);
            
            //the great equation
            int xCoord = (int)(Game1.globalScaleX * (location.X + (entry.totalOffset.X * entry.scale)));
            if (direction == DirectionEnum.Left)
            {
                xCoord = (int)(Game1.globalScaleX * (location.X - (entry.frame.Size.X + entry.totalOffset.X + entry.offSetDirectional) * entry.scale));
            }
            int yCoord = (int)(Game1.globalScaleY * (location.Y + (entry.totalOffset.Y * entry.scale)));
            int Width =(int)(Game1.globalScaleX * (entry.frame.Size.X * entry.scale));
            int Height=(int)(Game1.globalScaleY * (entry.frame.Size.Y * entry.scale));



			destinationRectangle = new Rectangle(xCoord,yCoord,Width,Height);

			spriteBatch.Draw(_spriteSheet, destinationRectangle, sourceRectangle, Color.White);

		}


	}