using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    [BaggedProperty("canSpawn", false), EditorGroup("guns|misc")]
    public class PimpRCController : RCController
    {
        PimpRCController(float xval, float yval, PimpRCCar car) : base(xval, yval, car)
        {

        }
    }
}
