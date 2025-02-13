using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2.Components.IECSComponents
{
	internal interface IActivatable
	{
		public void Activate();
		public void Deactivate();
	}
}
