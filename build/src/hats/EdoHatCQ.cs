using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    //EdoHat Custom Quack
    abstract class EdoHatCQ : EdoHat
    {

        protected abstract string getQuack();

        public EdoHatCQ(float x, float y, Team t)
            : base(x, y, t)
        {
        }

        public override void Terminate()
        {
            resetquack();
            base.Terminate();
        }

        public override void UnEquip()
        {
            resetquack();
            base.UnEquip();
        }

        //only used for quack workaround
        bool pquack = false;

        public override void Update()
        {
            base.Update();
            if (ModSettings.quackworkaround)
            {
                if (equippedDuck != null)
                {
                    bool cquack = equippedDuck.IsQuacking();
                    if (cquack && !pquack) this.Quack(1f, equippedDuck.quackPitch);
                    pquack = cquack;
                }
            }
        }

        private void resetquack()
        {
            if (equippedDuck != null)
                equippedDuck._netQuack = new NetSoundEffect("quack");
        }

        public override void Quack(float volume, float pitch)
        {
            string qstr = getQuack();
            if (qstr == null) return;
            SFX.Play(Mod.GetPath<EdoMod>(qstr), volume, pitch);
        }
    }
}
