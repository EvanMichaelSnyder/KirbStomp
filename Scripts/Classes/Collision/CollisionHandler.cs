using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KirbStomp;
namespace KirbStomp.Scripts.Classes.Collision
{
    internal class CollisionHandler
    {
        private static ArrayList _managers;

        public static void addNewManager(HitboxManager manager)
        {
            _managers.Add(manager);
        }

        public static void removeManager(HitboxManager manager)
        {
            _managers.Remove(manager);
        }

        public static void removeAll()
        {
            _managers.Clear();
        }


        //this will change a lot it is not performant
        public static void checkGroundCollisions()
        {
            ArrayList managersCopy = new ArrayList(_managers);
            foreach (HitboxManager manager in _managers)
            {
                managersCopy.Remove(manager);
                foreach (HitboxManager manager2 in managersCopy)
                {
                    //part that needs to change
                    if (manager.GetHitboxTypeEnum() == HitboxTypeEnum.Character && manager2.GetHitboxTypeEnum() == HitboxTypeEnum.Platform)
                    {

                    }
                }


            }
        }
    }
}
