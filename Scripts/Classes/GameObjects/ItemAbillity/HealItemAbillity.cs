using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbStomp.Scripts.Classes.GameObjects.ItemAbillity
{
    public class HealItemAbillity : AItemAbillity
    {
        private Character _charecterToHeal;
        private float _healAmount;

        public HealItemAbillity(Character charecterToHeal, float healAmount)
        {
            this._charecterToHeal = charecterToHeal;
            this._healAmount = healAmount;
        }

        
        public override void ExectuteAbillity()
        {
            this._charecterToHeal.AddHealth(this._healAmount);
        }
    }
}
