using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod.src.weapons.railguns.ammo
{
    class DynamicBullet : Bullet
    {
        public DynamicBullet(float xval, float yval, AmmoType type, float ang = -1f, Thing owner = null, bool rbound = false, float distance = -1f, bool tracer = false, bool network = true) :
            base(xval, yval, type, ang, owner, rbound, distance, tracer, network)
        {

        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void Update()
        {
            (this.ammo.sprite as SpriteMap).frame = 1;
            base.Update();
        }
    }
}
