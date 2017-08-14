namespace DuckGame.EdoMod
{
    class DenHat : TeamHat
    {
        public static string hatName = "DENIED";

        public static string hatPath = "hats\\denied";

        public static short texindex;

        public static void addHat()
        {
            Team t = new Team(hatName, GetPath<EdoMod>(hatPath));
            Teams.core.teams.Add(t);
            texindex = t.hat.texture.textureIndex;
        }

        public static bool isHat(TeamHat teamHat)
        {
            return teamHat.sprite.texture.textureIndex == texindex && !(teamHat is DenHat);
        }

        public DenHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            if ((ModSettings.isDeveloper || ModSettings.isFriend) && pitch > 0.9) SFX.Play(Mod.GetPath<EdoMod>("SFX\\DENIED"), volume, -1f);
            else base.Quack(volume, pitch);
        }
    }
}