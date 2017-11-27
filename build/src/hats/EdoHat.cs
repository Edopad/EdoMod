using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    abstract class EdoHat : TeamHat
    {
        //protected abstract string getQuack();

        public EdoHat(float x, float y, Team t)
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

        private NetSoundEffect quackeff = new NetSoundEffect("quack");

        public virtual void DoQuack(float pitch)
        {
            SFX.Play("SFX\\airhorn_long");
            return;
        }

        private class QuackCallback
        {
            public void Invoke(EdoHat eh, float pitch)
            {
                eh.DoQuack(pitch);
            }
        }

        private static QuackCallback _QuackCallback = new QuackCallback();

        public override void Update()
        {
            if (this.equippedDuck != null) this.equippedDuck._netQuack = quackeff;

            base.Update();

            if (ModSettings.quackworkaround)
            {
                if (equippedDuck != null)
                {
                    bool cquack = equippedDuck.IsQuacking();
                    if (cquack && !pquack && Network.isActive)
                    {
                        this.xscale *= 2;
                    }
                        //_QuackCallback.Invoke(this, equippedDuck.quackPitch);
                        //DoQuack(equippedDuck.quackPitch); //Quack(1f, equippedDuck.quackPitch);
                    pquack = cquack;
                }
            }
        }

        private void resetquack()
        {
            if (equippedDuck != null)
                equippedDuck._netQuack = new NetSoundEffect("quack");
        }

        public void setquack(string path)
        {
            quackeff = new NetSoundEffect(path);
        }
        public void setquack(string[] paths)
        {
            quackeff = new NetSoundEffect(paths);
        }
        public void setquack(List<string> common, List<string> rare)
        {
            quackeff = new NetSoundEffect(common, rare);
        }

        public override void Quack(float volume, float pitch)
        {
            //if (equippedDuck.inputProfile.rightTrigger != 0)
            //    pitch = -equippedDuck.inputProfile.rightTrigger;
            quackeff.Play(volume, pitch);
        }
    }
}

