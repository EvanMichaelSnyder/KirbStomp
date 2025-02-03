using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Inputs
{
	internal class TestCommand : ICommands
	{
		public void Execute()
		{
			Debug.WriteLine("Testing!!!! I WAS JUST CALLED BACk");
		}
	}
}
