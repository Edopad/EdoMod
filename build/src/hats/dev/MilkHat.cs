namespace DuckGame.EdoMod
{
    class MilkHat : EdoHat
    {
        public static string hatName = "Milk";

        public static string hatPath = "hats\\burger";

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

        public MilkHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        //public override void Quack(float volume, float pitch)
        //{
        //    SFX.Play(Mod.GetPath<EdoMod>("SFX\\airhorn_long"), volume, pitch);
        //}

        private int milkheight = 0;

        public override void Draw()
        {
            base.Draw();
            if (milkheight > 0)
                Graphics.DrawLine(new Vec2(0, 320f * Graphics.aspect), new Vec2(320f, 320f * Graphics.aspect), new Color(1f, 1f, 1f, 0.1f) * 0.5f, milkheight, 1f);
            if (equippedDuck != null)
            {
                if (equippedDuck.IsQuacking() && milkheight < 320) milkheight++;
                else if(milkheight > 0) milkheight -= 3;
            }
        }
    }
}