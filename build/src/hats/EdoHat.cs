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
            //resetquack();
            base.Terminate();
        }

        public override void UnEquip()
        {
            //resetquack();
            base.UnEquip();
        }

        //only used for quack workaround
        bool pquack = false;

        public override void Update()
        {
            base.Update();
            if(ModSettings.quackworkaround)
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
    }
}

