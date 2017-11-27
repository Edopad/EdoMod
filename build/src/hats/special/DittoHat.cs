using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    class DittoHat : EdoHat
    {
        public static string hatName = "Ditto";

        public static string hatPath = "hats\\ditto";

        public static short texindex;

        public static void addHat()
        {
            Team t = new Team(hatName, GetPath<EdoMod>(hatPath));
            Teams.core.teams.Add(t);
            texindex = t.hat.texture.textureIndex;
        }

        public static bool isHat(TeamHat teamHat)
        {
            return teamHat.sprite.texture.textureIndex == texindex && !(teamHat is EdoHat);
        }

        public DittoHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        bool infcopies = true;
        private int _copiesleft = 5;

        public override void Quack(float volume, float pitch)
        {
            base.Quack(volume, pitch);
            if (_copiesleft-- > 0 || infcopies) Level.Add(new DittoHat(x,y,team));
            //SFX.Play(Mod.GetPath<EdoMod>("SFX\\airhorn_long"), volume, pitch);
            //for(Duck ducks in Level.current.)

            IEnumerable<Thing> ducks = Level.current.things[typeof(Duck)];
            Duck cd = this.equippedDuck;    //closest duck
            float mindist = 100000f; //current min. distance
            foreach (Duck d in ducks)
            {
                if (d == this.equippedDuck) continue;
                if(!(d.hat is TeamHat)) continue;
                float cdist = new Vec2(cd.position - d.position).length;
                if (cdist <= 1f) continue;
                if (cdist < mindist)
                {
                    cd = d;
                    mindist = cdist;
                }
            }
            //replace current hat
            if (cd == this.equippedDuck) return;

            TeamHat h = new TeamHat(0f, 0f, this.team);
            if (cd.hat is TeamHat)
                h = new TeamHat(this.x, this.y, (cd.hat as TeamHat).team);
            EdoMain.instance.ReplaceHat(this, h);
            Level.Remove(this);
        }
    }
}