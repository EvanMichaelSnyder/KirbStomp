using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.ECSV2.Components.IECSComponents
{
    internal interface IUpdatableECSComponent : IECSComponent
    {
        public void Update(float deltaTime);
    }
}
