using System;

namespace DuckGame.EdoMod
{
    class ChickenHat : EdoHat
    {
        public static string hatName = "CHKN";

        public static string hatPath = "hats\\chicken";

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

        public ChickenHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            {
                HatEgg magnumShell = new HatEgg(this.x, this.y + Rando.Float(-2, 2));

                float ang = Rando.Float(0f, 2f * (float)Math.PI);
                float speed = 5f * pitch;

                magnumShell.vSpeed = -speed * (float)Math.Sin(ang);
                magnumShell.hSpeed = speed * (float)Math.Cos(ang);

                magnumShell.depth = depth - 1;
                Level.Add((Thing)magnumShell);
            }
            SFX.Play(Mod.GetPath<EdoMod>("SFX\\cluck"), volume, pitch);
        }

        public override void Update()
        {
            base.Update();
            if (Rando.Int(0, 600) == 1) Quack(1f, 0f);
        }
    }

    class HatEgg : PhysicsParticle
    {
        private Sprite _sprite;
        public HatEgg(float xpos, float ypos)
            : base(xpos, ypos)
        {
            this._sprite = new Sprite(Mod.GetPath<EdoMod>("hats\\particles\\egg"));
            this.scale = new Vec2(0.5f);
            this.graphic = (Sprite) this._sprite;
            this.center = new Vec2(5, 3);
        }

        public override void Update()
        {
            base.Update();
            this._angle = Maths.DegToRad(-this._spinAngle);
        }
    }
}