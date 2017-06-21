using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    class UpsideHat : EdoHat
    {
        public static string hatName = "Upside";

        public static string hatPath = "hats\\template";

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

        public UpsideHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Update()
        {
            if (this.equippedDuck != null) equippedDuck._sprite.flipV = true;
            base.Update();
        }
    }
}