﻿namespace DuckGame.EdoMod
{
    class CensoredHat : EdoHat
    {
        public static string hatName = "[Censored]";

        public static string hatPath = "hats\\censored";

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

        public CensoredHat(float x, float y, Team t)
            : base(x, y, t)
        {
            playing = null;
        }

        private Sound playing = null;

        private void killquack()
        {
            if (playing != null)
            {
                playing.Kill();
                playing = null;
            }
        }

        public override void Update()
        {
            base.Update();
            
            if (equippedDuck == null)
            {
                killquack();
                return;
            } else
            {
                if (playing != null)
                {
                    playing.Pitch = equippedDuck.inputProfile.leftTrigger - equippedDuck.inputProfile.rightTrigger;
                }
            }
            /*if (playing != null) playing.Pitch = equippedDuck.quackPitch;*/

            if (!equippedDuck.IsQuacking()) killquack();
        }

        public override void Terminate()
        {
            killquack();
            base.Terminate();
        }

        public override void Quack(float volume, float pitch)
        {
            if (playing == null)
            {
                playing = SFX.Play(Mod.GetPath<EdoMod>("SFX\\bleep_ss"), volume, pitch, 0f, true);
            }
        }
    }
}