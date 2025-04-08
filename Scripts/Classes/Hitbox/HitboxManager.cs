using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
using KirbStomp.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbStomp {
    public enum HitboxTypeEnum
    {
        None = 0,
        Body,
        Attack,
        Platform,
        Item,
        Boundary
    }
    public class HitboxManager
    {
        private int _parentID;
        private HitboxTypeEnum _hitboxType;
        private ISpriteComplete _sprite;
        private List<Rectangle> _hitboxes;

        public void debugWriteBoxes()
        {
            foreach (Rectangle box in _hitboxes)
            {
                Debug.WriteLine(box.ToString);
            }
        }

        public List<Rectangle> getRectangles()
        {
            return _hitboxes;
        }

        public int getID()
        {
            return _parentID;
        }
        public HitboxTypeEnum GetHitboxTypeEnum()
        {
            return _hitboxType;
        }

        public HitboxManager(Texture2D spriteSheet, HitboxTypeEnum type, int parentID)
        {
            _parentID = parentID;
            _hitboxType = type;
            _sprite = new AllPurposeSprite(spriteSheet);
            _hitboxes = new List<Rectangle>();
        }

        //incase no parent ID is given
        public HitboxManager(Texture2D spriteSheet, HitboxTypeEnum type)
        {
            _parentID = -1;
            _hitboxType = type;
            _sprite = new AllPurposeSprite(spriteSheet);
            _hitboxes = new List<Rectangle>();
        }


        public void basicUpdateHitbox(Rectangle rectangle)
        {
            _hitboxes.Clear();
            _hitboxes.Add(rectangle);
        }

        public void UpdateHitboxList(Vector2 location, DirectionEnum direction, string name, StateEnum state, int currentFrame)
        {
            var animEntry = AnimationRepository.GetFrameData(name, state, currentFrame);
            var hitboxEntry = HitboxRepository.GetFrameData(name, state, currentFrame);
            _hitboxes.Clear();

            //Finding sprite position in virtual space
            Vector2 spriteLocationVirtual;
            spriteLocationVirtual.X = (int)((location.X + (animEntry.totalOffset.X * animEntry.scale)));
            if (direction == DirectionEnum.Left)
            {
                spriteLocationVirtual.X = (int)((location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional) * animEntry.scale));
            }
            spriteLocationVirtual.Y = (int)((location.Y + (animEntry.totalOffset.Y * animEntry.scale)));


            Rectangle animBoxSource = animEntry.frame.ToRectangle(direction, animEntry.boundX);


            foreach (HitboxRepository.HitboxData box in hitboxEntry.frame.Hitboxes)
            {
                Rectangle hitBoxSource = box.ToRectangle(direction, animEntry.boundX);
                Vector2 offsetToHitbox = new Vector2(hitBoxSource.X - animBoxSource.X, hitBoxSource.Y - animBoxSource.Y);

                int xCoord = (int)(location.X + (animEntry.totalOffset.X + offsetToHitbox.X) * animEntry.scale);
                if (direction == DirectionEnum.Left)
                {
                    spriteLocationVirtual.X = (int)((location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional) * animEntry.scale));
                    xCoord = (int)(location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional - offsetToHitbox.X) * animEntry.scale);
                }
                int yCoord = (int)(location.Y + (animEntry.totalOffset.Y + offsetToHitbox.Y) * animEntry.scale);

                int Width = (int)(box.Size.X * animEntry.scale);
                int Height = (int)(box.Size.Y * animEntry.scale);

                _hitboxes.Add(new Rectangle(xCoord, yCoord, Width, Height));

            }


        }

        //this is duplicated code completely I have no excuse but my own mortality
        public void UpdateAttackHitboxList(Vector2 location, DirectionEnum direction, string name, StateEnum state, int currentFrame)
        {
            var animEntry = AnimationRepository.GetFrameData(name, state, currentFrame);
            var hitboxEntry = AttackHitboxRepository.GetFrameData(name, state, currentFrame);
            _hitboxes.Clear();

            //Finding sprite position in virtual space
            Vector2 spriteLocationVirtual;
            spriteLocationVirtual.X = (int)((location.X + (animEntry.totalOffset.X * animEntry.scale)));
            if (direction == DirectionEnum.Left)
            {
                spriteLocationVirtual.X = (int)((location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional) * animEntry.scale));
            }
            spriteLocationVirtual.Y = (int)((location.Y + (animEntry.totalOffset.Y * animEntry.scale)));


            Rectangle animBoxSource = animEntry.frame.ToRectangle(direction, animEntry.boundX);


            foreach (AttackHitboxRepository.HitboxData box in hitboxEntry.frame.Hitboxes)
            {
                if (box.Size.X != 0 && box.Size.Y!=0)
                {
                    Rectangle hitBoxSource = box.ToRectangle(direction, animEntry.boundX);
                    Vector2 offsetToHitbox = new Vector2(hitBoxSource.X - animBoxSource.X, hitBoxSource.Y - animBoxSource.Y);

                    int xCoord = (int)(location.X + (animEntry.totalOffset.X + offsetToHitbox.X) * animEntry.scale);
                    if (direction == DirectionEnum.Left)
                    {
                        spriteLocationVirtual.X = (int)((location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional) * animEntry.scale));
                        xCoord = (int)(location.X - (animEntry.frame.Size.X + animEntry.totalOffset.X + animEntry.offSetDirectional - offsetToHitbox.X) * animEntry.scale);
                    }
                    int yCoord = (int)(location.Y + (animEntry.totalOffset.Y + offsetToHitbox.Y) * animEntry.scale);

                    int Width = (int)(box.Size.X * animEntry.scale);
                    int Height = (int)(box.Size.Y * animEntry.scale);

                    _hitboxes.Add(new Rectangle(xCoord, yCoord, Width, Height));
                }

            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Rectangle hitBox in _hitboxes)
            {
                _sprite.DrawHitbox(spriteBatch, hitBox);
            }
        }
        public void DrawExtended(SpriteBatch spriteBatch)
        {
            foreach (Rectangle hitBox in _hitboxes)
            {
                Rectangle extended = hitBox;
                extended.Inflate(2, 2);
                _sprite.DrawHitbox(spriteBatch, extended);
            }
        }

        public Vector2 getCentralizedPosition()
        {
            float totalArea = 0;
            float weightedX = 0;
            float weightedY = 0;

            foreach (var rect in _hitboxes)
            {
                // Calculate area of the current rectangle
                float area = rect.Width * rect.Height;

                // Calculate the center of the current rectangle
                float centerX = rect.X + rect.Width / 2f;
                float centerY = rect.Y + rect.Height / 2f;

                // Weight the center by the area
                weightedX += centerX * area;
                weightedY += centerY * area;

                // Add the area to the total area
                totalArea += area;
            }

            // Return the weighted average center position (center of area)
            if (totalArea == 0)
            {
                return Vector2.Zero; // Return (0,0) if there are no rectangles (empty hitbox manager)
            }

            return new Vector2(weightedX / totalArea, weightedY / totalArea);
        }

        public Rectangle GetApproximation()
        {
            if(_hitboxes.Count == 0)
            {
                return Rectangle.Empty;
            }
            Rectangle approx = _hitboxes.First();
            foreach (var rect in _hitboxes.Skip(1))
            {
                approx = Rectangle.Union(approx, rect);
            }
            return approx;
        }

        public Rectangle CheckAccurateCollision(HitboxManager manager)
        {
            foreach (Rectangle rectA in _hitboxes)
            {
                foreach (Rectangle rectB in manager.getRectangles())
                {
                    Rectangle intersection = Rectangle.Intersect(rectA, rectB);
                    if (!intersection.IsEmpty)
                    {
                        return intersection;
                    }
                }
            }
            return Rectangle.Empty;
        }
    }
}
