using System;

namespace DuckGame.EdoMod
{
    class BombHat : EdoHat
    {
        public static string hatName = "Bombs";

        public static string hatPath = "hats\\bomb";

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

        public BombHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }



        public override void Quack(float volume, float pitch)
        {
            if(ModSettings.enableDevEggs)
                Level.Add(new ToBeContinued(x, y));
            if (pitch > 0.95f)
            {
                if (equippedDuck == null) return;
                //immobilize duck
                //equippedDuck.ragdoll = new Ragdoll(equippedDuck.x, equippedDuck.y, equippedDuck, false, 0, 0, new Vec2(0,0));
                equippedDuck.GoRagdoll();

                for (int i = 0; i < 200; i++)
                {
                    Shrapnel magnumShell = new Shrapnel(x + Rando.Float(-4, 4), y);

                    float ang = Rando.Float(0f, 2f * (float)Math.PI);
                    float speed = Rando.Float(0f, 5f);

                    magnumShell.vSpeed = -speed * (float)Math.Sin(ang);
                    magnumShell.hSpeed = speed * (float)Math.Cos(ang);

                    magnumShell.depth = depth - 1;
                    Level.Add((Thing)magnumShell);
                }
                base.Quack(volume, pitch);
                Level.Remove(this);

            } else
            {
                base.Quack(volume, pitch);
            }
            
        }

        public override void Terminate()
        {
            if (!(Level.current is Editor))
            {
                Level.Add(SmallSmoke.New(base.x, base.y));
                Level.Add(SmallSmoke.New(base.x + 4f, base.y));
                Level.Add(SmallSmoke.New(base.x - 4f, base.y));
                Level.Add(SmallSmoke.New(base.x, base.y + 4f));
                Level.Add(SmallSmoke.New(base.x, base.y - 4f));
            }
            base.Terminate();
        }

        private class Shrapnel : PhysicsParticle
        {
            //private Sprite _sprite;
            public Shrapnel(float xpos, float ypos)
                : base(xpos, ypos)
            {
                Sprite _sprite = new Sprite(Mod.GetPath<EdoMod>("hats\\particles\\shrapnel"));
                this.scale = new Vec2(1f);
                this.graphic = (Sprite)_sprite;
                this.center = new Vec2(5, 3);
            }

            public override void Update()
            {
                base.Update();
                this._angle = Maths.DegToRad(-this._spinAngle);
            }
        }
    }
}