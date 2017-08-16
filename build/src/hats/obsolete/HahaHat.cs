namespace DuckGame.EdoMod
{
    class HahaHat : EdoHat
    {
        public static string hatName = "Woody";

        public static string hatPath = "hats\\woody";

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

        public HahaHat(float x, float y, Team t)
            : base(x, y, t)
        {
            setquack(Mod.GetPath<EdoMod>("SFX\\hahaha"));
        }
    }
}