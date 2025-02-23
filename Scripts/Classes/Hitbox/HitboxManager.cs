using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;
using static HitboxRepository;

namespace KirbStomp.Scripts.Classes.HitboxManager
{
    public class HitboxManager
    {
        private ISpriteComplete _sprite;
        private List<Rectangle> _hitboxes;

        public void debugWriteBoxes()
        {
            foreach (Rectangle box in _hitboxes)
            {
                Debug.WriteLine(box.ToString);
            }
        }

        public HitboxManager(Texture2D spriteSheet)
        {
            _sprite = new AllPurposeSprite(spriteSheet);
        }

        public void UpdateHitboxList(Vector2 location, DirectionEnum direction, string name, StateEnum state, int currentFrame)
        {
            var animEntry = AnimationRepository.GetFrameData(name, state, currentFrame);
            var hitboxEntry = HitboxRepository.GetFrameData(name, state, currentFrame);
            _hitboxes = new List<Rectangle>();

            //Finding sprite position in virtual space
            Vector2 spriteLocationVirtual;
            spriteLocationVirtual.X = (int)((location.X + (animEntry.totalOffset.X * animEntry.scale)));
            if (direction == DirectionEnum.Left)
            {
               spriteLocationVirtual.X = (int)((location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional) * animEntry.scale));
            }
            spriteLocationVirtual.Y = (int)((location.Y + (animEntry.totalOffset.Y * animEntry.scale)));


            Rectangle animBoxSource =animEntry.frame.ToRectangle(direction, animEntry.boundX);


            foreach (HitboxData box in hitboxEntry.frame.Hitboxes)
            {
                Rectangle hitBoxSource = box.ToRectangle(direction, animEntry.boundX);
                Vector2 offsetToHitbox = new Vector2(hitBoxSource.X - animBoxSource.X, hitBoxSource.Y - animBoxSource.Y);

                int xCoord = (int)(location.X + (animEntry.totalOffset.X + offsetToHitbox.X) * animEntry.scale);
                if (direction == DirectionEnum.Left)
                {
                    xCoord = (int)(location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional - offsetToHitbox.X) * animEntry.scale);
                }
                int yCoord = (int)(location.Y + (animEntry.totalOffset.Y + offsetToHitbox.Y)* animEntry.scale);

                int Width = (int)(box.Size.X * animEntry.scale);
                int Height = (int)(box.Size.Y * animEntry.scale);

                _hitboxes.Add(new Rectangle(xCoord, yCoord, Width, Height));

            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
           foreach (Rectangle hitBox in _hitboxes)
           {
             _sprite.DrawHitbox(spriteBatch, hitBox);
           }
        }

    }
}
