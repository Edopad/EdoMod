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

        private int _copiesleft = 1;

        public override void Quack(float volume, float pitch)
        {
            base.Quack(volume, pitch);
            if (_copiesleft-- > 0) Level.Add(new DittoHat(x,y,team));
            //SFX.Play(Mod.GetPath<EdoMod>("SFX\\airhorn_long"), volume, pitch);
        }
    }
}