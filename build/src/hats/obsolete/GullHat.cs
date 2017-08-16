namespace DuckGame.EdoMod
{
    class GullHat : EdoHat
    {
        public static string hatName = "Gulls";

        public static string hatPath = "hats\\gull";

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

        public GullHat(float x, float y, Team t)
            : base(x, y, t)
        {
            setquack(Mod.GetPath<EdoMod>("SFX\\mine"));
        }
    }
}