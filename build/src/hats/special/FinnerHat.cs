namespace DuckGame.EdoMod
{
    class FinnerHat : EdoHat
    {
        public static string hatName = "Finner";

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

        public FinnerHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void DoDraw()
        {
            //this._angle += 1;
            this.angle += (MathHelper.Pi * 2 / 60);
            //this.sprite.angle += 1;
            //this.sprite._angle += 1;
           // base.DoDraw();
        }

        private float rot = 0;

        public override void Draw()
        {
            //this.angle += 1;
            this.angle += (MathHelper.Pi * 2 / 60);
            this.PositionOnOwner();
            if (this._graphic != null)
            {
                base.graphic.flipH = (this.offDir <= 0);
                this._graphic.position = this.position;
                this._graphic.alpha = base.alpha;
                this._graphic.angle = this.angle + (rot += (MathHelper.Pi * 2 / 60));
                this._graphic.depth = base.depth;
                this._graphic.scale = base.scale;
                this._graphic.center = this.center;
                this._graphic.Draw();
            }
            //this._angle += 1;
            
            //this.sprite.angle += 1;
            //this.sprite._angle += 1;
            //base.Draw();
        }

        public override void Quack(float volume, float pitch)
        {

            Pie magnumShell = new Pie(equippedDuck.x + Rando.Float(-4, 4), equippedDuck.y);

            magnumShell.vSpeed = (-1.5f - Rando.Float(1f));
            magnumShell.hSpeed = (Rando.Float(-2f, 2f));

            magnumShell.depth = depth - 1;
            Level.Add((Thing)magnumShell);

            
            this.graphic = new Sprite(Mod.GetPath<EdoMod>("images\\finner"));

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
                this.scale = new Vec2(0.01f);
                this.graphic = (Sprite)this._sprite;
                this.center = new Vec2(800, 800);
                //this.depth = (Depth)(0.3f + Rando.Float(0.0f, 0.1f));
            }

            public override void Update()
            {
                base.Update();
                this._angle += (float) (MathHelper.Pi / 30.0 * 1.0);
                //this._angle = Maths.DegToRad(-this._spinAngle);
            }
        }
    }
}