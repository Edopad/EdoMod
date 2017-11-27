using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    class TurbanData
    {
        //hat name
        public string Name;
        //texture path
        public string TexPath;
        //quack noise paths
        public List<string> QuackPaths = new List<string>();
        //rare quack noise paths
        public List<string> RareQuackPaths = new List<string>();
        //devlevel
        public FriendLevel access;

        //Texture ID
        private short _texid;

        public TurbanData(string name, string texture, string quack, FriendLevel flvl = FriendLevel.Neutral)
        {
            Name = name;
            TexPath = texture;
            QuackPaths.Add(quack);
            access = flvl;

            Team t = new Team(Name, Mod.GetPath<EdoMod>(TexPath))
            {
                locked = !FriendManager.canuse(flvl)
            };

            Teams.core.teams.Add(t);
            _texid = t.hat.texture.textureIndex;
            //Add turban data to master list
            turbans.Add(this);
        }

        public TurbanData(string name, string texture, string[] quacks, FriendLevel flvl = FriendLevel.Neutral)
        {
            Name = name;
            TexPath = texture;
            QuackPaths.AddRange(quacks);
            access = flvl;

            Team t = new Team(Name, Mod.GetPath<EdoMod>(TexPath))
            {
                locked = !FriendManager.canuse(flvl)
            };

            Teams.core.teams.Add(t);
            _texid = t.hat.texture.textureIndex;
            //Add turban data to master list
            turbans.Add(this);
        }

        //combination of ishat and gethat
        public Turban getHat(TeamHat th, float x, float y, Team t)
        {
            return isHat(th) ? new Turban(x, y, t) : null;
        }

        public bool isHat(TeamHat th)
        {
            return th.sprite.texture.textureIndex == _texid && !(th is EdoHat);
        }

        
        /*public Turban create(float x, float y, Team t)
        {
            return new Turban(x,y,t);
        }*/

        //static portion of class

        private static List<TurbanData> turbans = new List<TurbanData>();

        public static Turban findHat(TeamHat th)
        {
            TurbanData td = find(th.sprite.texture.textureIndex);
            if (td != null) return new Turban(th.x, th.y, th.team);
            return null;
            /*
            foreach (TurbanData td in turbans)
            {
                if (td.isHat(th)) return new Turban(th.x, th.y, th.team);
                    //return new Turban(th.x, th.y, th.team, td);
            }
            return null;*/
        }

        public static TurbanData find(short texid)
        {
            foreach (TurbanData td in turbans)
            {
                if (td._texid == texid) return td;
            }
            return null;
        }



        /*public static void add(TurbanData t)
        {
            turbans.Add(t);
        }

        public static void add(string name, string texture, string quack, FriendLevel flvl = FriendLevel.Neutral)
        {
            add(new TurbanData(name, texture, quack, flvl));
        }*/
    }
}
