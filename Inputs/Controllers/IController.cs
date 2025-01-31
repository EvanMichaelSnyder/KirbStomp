using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Inputs.Controllers
{
	// IControllers purely update and gives data on the pressed, just pressed, released, and just released keys
	internal interface IController
	{
		public void Update();
	}
}
