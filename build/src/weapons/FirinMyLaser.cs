using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod")]
    [BaggedProperty("isSuperWeapon", true)]
    class FirinMyLaser : Hat
    {

        private static Vec2 _barrelOffset = new Vec2(0f, 4f);

        private float elapsed = 0f;

        private Boolean firing = false;

        public FirinMyLaser(float x, float y)
            : base(x, y)
        {
            playing = null;

            this._pickupSprite = new Sprite("helmetPickup", 0f, 0f);
            this._sprite = new SpriteMap(EdoMod.GetPath<EdoMod>("weapons\\firinMyLaser"), 32, 32, false);
            base.graphic = this._pickupSprite;
            this.center = new Vec2(8f, 8f);
            this.collisionOffset = new Vec2(-5f, -2f);
            this.collisionSize = new Vec2(12f, 8f);
            this._sprite.CenterOrigin();
            this._isArmor = false;
            this._equippedThickness = 3f;
        }

        private Sound playing = null;

        private void killquack()
        {
            if (playing != null)
            {
                playing.Kill();
                playing = null;
            }
        }

        public override void Update()
        {
            base.Update();
            if (equippedDuck == null)
            {
                killquack();
                return;
            }



            if (playing != null)
            {
                playing.Pitch = equippedDuck.inputProfile.leftTrigger - equippedDuck.inputProfile.rightTrigger;
                elapsed += 1f / 60f;
                if(elapsed > 2.67f)
                {
                    if (elapsed > 6.3f) firing = false;
                    else firing = true;
                }
            } else
            {
                elapsed = 0f;
                firing = false;
            }

            if(firing)
            {
                Vec2 barrel = this.Offset(_barrelOffset + new Vec2(10f, 0f));
                Vec2 target = this.Offset(_barrelOffset + new Vec2(1200f, 0f)) - barrel;
                Level.Add(new DeathBeam(barrel, target)
                {
                isLocal = base.isServerForObject
                });
            }
            /*if (playing != null) playing.Pitch = equippedDuck.quackPitch;*/

            if (!equippedDuck.IsQuacking()) killquack();
        }

        public override void Terminate()
        {
            killquack();
            base.Terminate();
        }

        public override void Draw()
        {
            //base.Draw();
            //Graphics.Draw(this._chargeAnim, base.x, base.y);
            this._sprite.frame = (firing ? 1 : 0);
            base.Draw();
            if (firing)
			{
				Vec2 barrel = this.Offset(_barrelOffset);
				Vec2 target = this.Offset(_barrelOffset + new Vec2(1200f, 0f));
				Graphics.DrawLine(barrel, target, new Color(1f, 1f, 1f) * 1f, 16f, default(Depth) + 3);
				Graphics.DrawLine(barrel, target, new Color(1f, 1f, 1f) * 0.7f, 22f, default(Depth) + 2);
				Graphics.DrawLine(barrel, target, new Color(1f, 1f, 1f) * 0.4f, 28f, default(Depth) + 1);
			}
        }

        public override void Quack(float volume, float pitch)
        {
            if (playing == null)
            {
                playing = SFX.Play(GetPath<EdoMod>("SFX\\hats\\ImFirinMyLaser"), volume, pitch, 0f, false);
            }
        }
    }
}
