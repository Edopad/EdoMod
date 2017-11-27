using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    class Turban : EdoHat
    {

        public Turban(float x, float y, Team t) : base(x, y, t)
        {
            //This code is pretty much just for (possible) cases where the hat isn't equipped.
            TurbanData td = TurbanData.find(this.sprite.texture.textureIndex);
            if (td != null)
            {
                //_tdata = td;
                //this.team = t;
                setquack(td.QuackPaths, td.RareQuackPaths);
            }
        }

        public override void Equip(Duck d)
        {
            TurbanData td = TurbanData.find(this.sprite.texture.textureIndex);
            if (td != null)
            {
                //_tdata = td;
                //this.team = t;
                setquack(td.QuackPaths, td.RareQuackPaths);
            }
            base.Equip(d);
        }
    }
}