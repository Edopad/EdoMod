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

        public override void Update()
        {
            if (this.equippedDuck != null) this.equippedDuck._netQuack = quackeff;

            base.Update();

            if (ModSettings.quackworkaround)
            {
                if (equippedDuck != null)
                {
                    bool cquack = equippedDuck.IsQuacking();
                    if (cquack && !pquack && Network.isActive) Quack(1f, equippedDuck.quackPitch);
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
            quackeff.Play(volume, -pitch);
        }
    }
}

