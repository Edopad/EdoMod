using System;
using Microsoft.Xna.Framework.Graphics;

namespace DuckGame.EdoMod
{
    class PiesHat : EdoHat
    {
        public static string hatName = "Pies";

        public static string hatPath = "hats\\pies";

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

        public PiesHat(float x, float y, Team t)
            : base(x, y, t)
        {

        }

        public override void Quack(float volume, float pitch)
        {
            base.Quack(volume,pitch);
            //string qstr = getQuack();
            //if (qstr == null) return;
            //SFX.Play(Mod.GetPath<EdoMod>("SFX/no-1"), volume, pitch);
            //this.graphic = new SpriteMap(Mod.GetPath<EdoMod>("hats\\particles\\burger"), 32, 32);
            //this._sprite.texture = this.graphic.texture;
            //this._sprite = new SpriteMap(Mod.GetPath<EdoMod>("hats\\particles\\burger"), 32, 32);
            //Tex2D tmptext = (new Sprite(Mod.GetPath<EdoMod>("hats\\particles\\burger"))).texture;
            //this._sprite.texture = tmptext;
        }

        public override void PressAction()
        {
            Quack(1f, 0f);
            base.PressAction();
        }

        public override void UnEquip()
        {
            base.UnEquip();
            for (int i = 0; i < 100; i++)
            {
                Pie magnumShell = new Pie(x + Rando.Float(-4, 4), y);

                float ang = Rando.Float(0f, 2f * (float)Math.PI);
                magnumShell.vSpeed = -5f * (float)Math.Sin(ang);
                magnumShell.hSpeed = 5f * (float)Math.Cos(ang);

                magnumShell.depth = depth - 1;
                Level.Add((Thing)magnumShell);
            }
        }

        /*public override float angle
        {
            get
            {
                if (this._raised || base.duck == null)
                {
                    return base.angle;
                }
                Vec2 stick = base.duck.inputProfile.rightStick;
                if (stick.length < 0.1f)
                {
                    stick = Vec2.Zero;
                    return base.angle;
                }
                if (this.offDir > 0)
                {
                    return Maths.DegToRad(Maths.PointDirection(Vec2.Zero, stick));
                }
                return Maths.DegToRad(Maths.PointDirection(Vec2.Zero, stick) + 180f);
            }
            set
            {
                this._angle = value;
            }
        }*/

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

            if (angle != 00 || !equippedDuck.IsQuacking()) return;
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
                this._sprite = new Sprite(Mod.GetPath<EdoMod>("hats\\particles\\piepart"));
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