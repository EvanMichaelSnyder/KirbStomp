using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Scripts.Classes.GameObjects.ItemAbillity
{
    public abstract class AItemAbillity
    {
        protected String _abillityName = "default abillity name";

        public abstract void ExectuteAbillity();

        public String GetAbillityName()
        {
            return this._abillityName;
        }
        

    }
}
