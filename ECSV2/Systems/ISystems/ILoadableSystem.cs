using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSV2.Systems.ISystems
{
	internal interface ILoadableSystem
	{

		public void Load(ContentManager content);
	}
}
