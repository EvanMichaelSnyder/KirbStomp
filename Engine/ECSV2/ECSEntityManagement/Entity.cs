using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace KirbStomp.Engine.ECSV2.EntityManagement
{
    public class Entity
    {
        private static uint IDTracker;

        private uint id;

        private Vector2 pos;

        public Entity()
        {
            id = IDTracker++;
        }

        public uint GetID()
        {
            return id;
        }
        public void SetID(uint id)
        {
            this.id = id;
        }

        public void SetPosition(float xPos, float yPos)
        {
            this.pos.X = xPos;
            this.pos.Y = yPos;
        }

        public void SetPosition(Vector2 pos)
        {
            //dont want pos var to change, just values
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;
        }

        public Vector2 GetPosition()
        {
            //return copy of pos,forces classes to use function to adjust pos
            return new Vector2(this.pos.X, this.pos.Y);
        }
    }
}
