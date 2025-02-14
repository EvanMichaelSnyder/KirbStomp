using KirbStomp.Engine.ECSV2.EntityManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2Test.EntityObjects
{
	internal interface PreBuiltEntity
	{
		public Entity CreateEntity();
	}
}
