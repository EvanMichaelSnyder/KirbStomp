using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Systems.ISystems
{
    internal interface IUpdatableSystem : ISystem
    {
        public void Update(float deltaTime);
    }
}
