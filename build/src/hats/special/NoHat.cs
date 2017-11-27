namespace DuckGame.EdoMod
{
    class NoHat : EdoHat
    {
        public static string hatName = "No!";

        public static string hatPath = "hats\\nohat";

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

        public NoHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }


        private float tmr = 0f;
        private Sound cquack;

        public override void Quack(float volume, float pitch)
        {
            SFX.KillAllSounds();
            //if (EdoMod.isDeveloper && equippedDuck != null)
            //{
            //    equippedDuck.visible = !equippedDuck.visible;
            //}
            cquack = SFX.Play(Mod.GetPath<EdoMod>("SFX\\no-1"), volume, pitch);
            tmr = 1.1f;
        }

        public override void Update()
        {
            base.Update();

            if (equippedDuck == null || cquack == null) return;

            if (!equippedDuck.IsQuacking()) return;

            float pitch = equippedDuck.quackPitch;
            if (pitch < 0f) pitch = 0f;
            if (pitch > 1f) pitch = 1f;
            cquack.Pitch = pitch;


            
            if (tmr > 0)
            {
                tmr -= 0.05f;
            }
            else
            {
                SFX.KillAllSounds();
            }

        }
    }
}
