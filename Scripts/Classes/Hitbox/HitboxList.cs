using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KirbStomp.Scripts.Classes.HitboxList
{
    public class HitboxData
    {
        public Vector2 Position;
        public Vector2 Size;

        public Rectangle ToRectangle(DirectionEnum direction, float boundX)
        {
            if (direction == DirectionEnum.Left)
            {
                return new Rectangle(
                    (int)(boundX - Position.X - Size.X),
                    (int)Position.Y,
                    (int)Size.X,
                    (int)Size.Y
                );
            }
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y
            );
        }
    }
}
