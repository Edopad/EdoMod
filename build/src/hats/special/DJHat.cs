using System;

namespace DuckGame.EdoMod
{
    class DJHat : EdoHat
    {
        public static string hatName = "DJ";

        public static string hatPath = "hats\\dj";

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

        public DJHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            SFX.Play(Mod.GetPath<EdoMod>("SFX\\djscratch"), volume, pitch);
            Music.SwitchSongs();
        }
    }
}