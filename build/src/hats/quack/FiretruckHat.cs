namespace DuckGame.EdoMod
{
    class FiretruckHat : EdoHat
    {
        public static string hatName = "Fire";

        public static string hatPath = "hats\\firetruck";

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

        public FiretruckHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            SFX.Play(Mod.GetPath<EdoMod>("SFX\\firehorn_short"), volume, pitch);
        }
    }
}