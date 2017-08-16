namespace DuckGame.EdoMod
{
    class PewDiePieHat : EdoHat
    {
        public static string hatName = "PewDiePie";

        public static string hatPath = "hats\\pewdiepie";

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

        public PewDiePieHat(float x, float y, Team t)
            : base(x, y, t)
        {
            setquack(Mod.GetPath<EdoMod>("SFX\\PewDiePie\\intro_1"));
        }
    }
}