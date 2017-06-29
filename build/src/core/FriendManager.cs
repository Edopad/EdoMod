using System;
using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    public class FriendManager
    {
        private static ulong _steamid;
        private static FriendLevel _flvl;

        public static ulong SteamId
        {
            get
            {
                return _steamid;
            }
            set
            {
                
            }
            
        }
        public static FriendLevel flevel
        {
            get
            {
                return _flvl;
            }
            set
            {

            }

        }

        public static bool canuse(FriendLevel fl)
        {
            return _flvl >= fl;
        }

        private class Known
        {
            public ulong _steamid;
            public FriendLevel _level;

            public Known(ulong id, FriendLevel flvl)
            {
                _steamid = id;
                _level = flvl;
            }
        }

        private static List<Known> knowns;

        public static void init()
        {
            knowns = new List<Known>();

            FriendLevel dev = FriendLevel.Dev;
            FriendLevel super = FriendLevel.Super;
            FriendLevel tester = FriendLevel.Tester;
            FriendLevel droog = FriendLevel.Droog;
            FriendLevel moodge = FriendLevel.Moodge;
            FriendLevel neuteral = FriendLevel.Neutral;
            FriendLevel foe = FriendLevel.Foe;

            //setup friend organizations
            //"Devs"
            add(76561198075158393, dev);    //Edopad
            add(76561198045895284, dev);    //Deadalus
            //Supadroogs
            add(76561198170721967, super);  //Funkasaurusrex
            add(76561198242940134, super);  //AWOLKat
            add(76561198079196492, super);  //TintedMonacle
            add(76561198143459877, super);  //Finitecircus
            //testers
            add(76561198135463290, tester); //Dawaste
            add(76561198052068017, tester); //Javito
            add(76561198305275218, tester); //Castiel +Verified

            //determine FriendLevel of current steam user

            if (Steam.user != null) _steamid = Steam.user.id;
            _flvl = FriendLevel.Neutral;
            foreach(Known k in knowns)
            {
                if (k._steamid == _steamid)
                {
                    _flvl = k._level;
                    break;
                }
            }
        }

        private static void add(ulong steamid, FriendLevel fl)
        {
            knowns.Add(new Known(steamid, fl));
        }
    }
}