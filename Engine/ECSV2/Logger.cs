using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Engine.ECSV2
{
	public static class Logger
	{
		public static void Log(string message)
		{
			Debug.WriteLine(message);
			Console.WriteLine(message);
		}
	}
}
