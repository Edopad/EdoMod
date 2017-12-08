using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    public class ModSettings
    {
        /*Mod Settings*/
        public static bool enableDevHats = false;
        public static bool enableDevEggs = false;
        public static bool quackworkaround = false;
        public static bool enableCustomIntro = true;
        public static bool enableDangerousInjections = true;

        
        public static ulong[] devs = new ulong[] {
            76561198075158393,  //Edopad
            76561198045895284   //Deadalus
        };

        public static ulong[] friends = new ulong[] {
            76561198170721967,  //Funkasaurusrex
            76561198242940134,  //AWOLKat
            76561198079196492   //TintedMonacle
        };


        /*Working Variables*/
        //Deprecated
        public static bool isDeveloper = false;
        //Deprecated
        public static bool isFriend = false;

        public static void init()
        {
            if (Steam.user != null)
            {
                isDeveloper = Array.Exists(devs, e => e == Steam.user.id);
                isFriend = Array.Exists(friends, e => e == Steam.user.id);
            }
        }
    }
}
