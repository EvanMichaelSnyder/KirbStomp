using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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

        public static XElement GetXElementOrAssert(string name, XElement parent)
        {
            XElement output = parent.Element(name);
            if (output == null)
            {
                throw new Exception($"XElement did not contain {name} element in Parent: {parent.Name}");
            }
            return output;
        }

        public static XElement GetXMLRootElement(string rootName, string fileName)
        {
            string dataFolder = XMLData.GetDataFolder();
            string sceneDataFile = Path.Combine(dataFolder, rootName + "Data", fileName + ".XML");

            XDocument sceneDoc = XDocument.Load(sceneDataFile);
            XElement root = sceneDoc.Root;
            if (root.Name != rootName)
            {
                throw new ArgumentException($"File at {sceneDataFile} has wrong root of {root.Name} and not {rootName}");
            }

            return root;
        }


	}
}
