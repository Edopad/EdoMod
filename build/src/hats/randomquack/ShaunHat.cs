using System;

namespace DuckGame.EdoMod
{
    class ShaunHat : EdoHat
    {
        public static string hatName = "Shaun";

        public static string hatPath = "hats\\shaun";

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

        public ShaunHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            SFX.Play(Mod.GetPath<EdoMod>("SFX\\shaun" + (new Random().Next(0, 3) + 1)), volume, pitch);
        }
    }
}