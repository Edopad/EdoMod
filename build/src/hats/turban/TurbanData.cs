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
        //quack noise path
        public string QuackPath;
        //devlevel
        public FriendLevel access;

        //Texture ID
        private short _texid;

        public TurbanData(string name, string texture, string quack, FriendLevel flvl = FriendLevel.Neutral)
        {
            Name = name;
            TexPath = texture;
            QuackPath = quack;

            Team t = new Team(Name, Mod.GetPath<EdoMod>(TexPath))
            {
                locked = !FriendManager.canuse(flvl)
            };

            Teams.core.teams.Add(t);
            _texid = t.hat.texture.textureIndex;
        }

        //combination of ishat and gethat
        public Turban getHat(TeamHat th, float x, float y, Team t)
        {
            return isHat(th) ? create(x, y, t) : null;
        }

        public bool isHat(TeamHat th)
        {
            return th.sprite.texture.textureIndex == _texid && !(th is EdoHat);
        }
        
        public Turban create(float x, float y, Team t)
        {
            return new Turban(x,y,t,this);
        }

        //static portion of class

        private static List<TurbanData> turbans = new List<TurbanData>();

        public static void add(TurbanData t)
        {
            turbans.Add(t);
        }

        public static void add(string name, string texture, string quack, FriendLevel flvl = FriendLevel.Neutral)
        {
            add(new TurbanData(name, texture, quack, flvl));
        }
    }
}
