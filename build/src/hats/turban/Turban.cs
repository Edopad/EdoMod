using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    class Turban : EdoHat
    {
        public Turban(float x, float y, Team t, TurbanData td) : base(x, y, t)
        {
            setquack(td.QuackPath);
        }
    }
}
