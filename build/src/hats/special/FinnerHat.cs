namespace DuckGame.EdoMod
{
    class FinnerHat : EdoHat
    {
        public static string hatName = "Finner";

        public static string hatPath = "hats\\weed";

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

        public FinnerHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {

            Pie magnumShell = new Pie(equippedDuck.x + Rando.Float(-4, 4), equippedDuck.y);

            magnumShell.vSpeed = (-1.5f - Rando.Float(1f));
            magnumShell.hSpeed = (Rando.Float(-2f, 2f));

            magnumShell.depth = depth - 1;
            Level.Add((Thing)magnumShell);

            base.Quack(volume, pitch);
            //SFX.Play(Mod.GetPath<EdoMod>("SFX\\smokeweedeveryday"), volume, pitch);
        }

        public override void Update()
        {
            base.Update();

            if (equippedDuck == null) return;


            //if (equippedDuck.onFire)
            //{
            //switch to burnt hat

            //this.graphic = new SpriteMap(Mod.GetPath<EdoMod>("hats\\particles\\burger"),32,32);
            //}



            Vec2 rsaim = equippedDuck.inputProfile.rightStick;

            if (!equippedDuck.IsQuacking()) return;
            //add pies
            {
                Pie magnumShell = new Pie(equippedDuck.x + Rando.Float(-4, 4), equippedDuck.y);

                if (rsaim.length > 0.05)
                {
                    magnumShell.vSpeed = 5f * -rsaim.y;
                    magnumShell.hSpeed = 5f * rsaim.x;
                }
                else
                {
                    magnumShell.vSpeed = (-1.5f - Rando.Float(1f));
                    magnumShell.hSpeed = (Rando.Float(-2f, 2f));
                }



                magnumShell.depth = depth - 1;
                Level.Add((Thing)magnumShell);
            }
        }

        private class Pie : PhysicsParticle
        {
            private Sprite _sprite;
            public Pie(float xpos, float ypos)
          : base(xpos, ypos)
            {
                this._sprite = new Sprite(Mod.GetPath<EdoMod>("images\\finner"));
                this.scale = new Vec2(1f);
                this.graphic = (Sprite)this._sprite;
                this.center = new Vec2(5, 3);
                //this.depth = (Depth)(0.3f + Rando.Float(0.0f, 0.1f));
            }

            public override void Update()
            {
                base.Update();
                this._angle = Maths.DegToRad(-this._spinAngle);
            }
        }
    }
}