using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.CompilerServices;

namespace KirbStomp.Data
{
	internal class XMLData
	{
		private static string DataDir = "";

		public static string GetDataFolder()
		{
			return CallerDirPath();
		}

		private static string CallerDirPath([CallerFilePath] string caller ="")
		{
			return Path.GetDirectoryName(caller);
		}
	}
}
