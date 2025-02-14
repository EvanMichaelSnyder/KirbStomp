using KirbStomp.Engine.ECSV2.Components.IComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components
{
    internal interface IUpdatableECSComponent
    {
        public void Update(float deltaTime);
    }
}
