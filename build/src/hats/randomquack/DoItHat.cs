namespace DuckGame.EdoMod
{
    class DoItHat : EdoHat
    {
        public static string hatName = "Shia";

        //Made by Dord#0010
        public static string hatPath = "hats\\LaBeouf";

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

        public DoItHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            SFX.Play(Mod.GetPath<EdoMod>("SFX\\justdoit\\" + getquack()), volume, -pitch);
            
        }

        private string getquack()
        {
            switch (Rando.Int(0, 14))
            {
                case 0: return "yesyoucan";
                case 1: return "dontletyourdreamsbedreams";
                case 2: return "justdoit";
                case 3: return "justdoit_2";
                case 4: return "no";
                case 5: return "nothingisimpossible";
                case 6: return "stopgivingup";
                default: return "doit";
            }
        }
    }
}