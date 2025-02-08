using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.ECSEntityManagement
{
    public class ECSEntity
    {
        private static uint IDTracker;

        private uint id;

        public ECSEntity()
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
    }
}
