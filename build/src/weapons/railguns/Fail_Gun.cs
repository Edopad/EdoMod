namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod|Weapons|guns")]
    public class FailGun : Gun
    {
        private SpriteMap sprite;

        public FailGun(float xpos, float ypos)
          : base(xpos, ypos)
        {
            _editorName = "Fail Gun";

            // collision & sprite settings
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("weapons\\fail_gun"), 19, 14);
            graphic = sprite;
            _center = new Vec2(7.5f, 6f);
            _collisionSize = new Vec2(16f, 9f);
            _collisionOffset = new Vec2(-7.5f, -6f);
            _holdOffset = new Vec2(-4f, -1f);
            _barrelOffsetTL = new Vec2(21f, 5f);
            _laserOffsetTL = new Vec2(22f, 5f);

            // weapon settings
            ammo = 1;
            _ammoType = new ATFail();
            //_fireSound = Mod.GetPath<EdoMod>("SFX\\pewpewpew");
            _fireWait = 3f;
            _kickForce = 2f;
            _weight = 3f;
            laserSight = false;
        }

        public override void OnPressAction()
        {
            if (ammo > 0 && duck != null)
            {
                duck.immobilized = true;
                duck.remoteControl = true;
                sprite._frame = 1;


                float cx = base.x;
                float cy = base.y;
                Graphics.flashAdd = 1.3f;
                Layer.Game.darken = 1.3f;
                if (base.isServerForObject)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        float dir = (float)i * 18f - 5f + Rando.Float(10f);
                        ATShrapnel shrap = new ATShrapnel();
                        shrap.range = 60f + Rando.Float(18f);
                        Bullet bullet = new Bullet(cx + (float)(System.Math.Cos((double)Maths.DegToRad(dir)) * 6.0), cy - (float)(System.Math.Sin((double)Maths.DegToRad(dir)) * 6.0), shrap, dir, null, false, -1f, false, true);
                        bullet.firedFrom = this;
                        this.firedBullets.Add(bullet);
                        Level.Add(bullet);
                    }
                    System.Collections.Generic.IEnumerable<Window> windows = Level.CheckCircleAll<Window>(this.position, 40f);
                    foreach (Window w in windows)
                    {
                        if (Level.CheckLine<Block>(this.position, w.position, w) == null)
                        {
                            w.Destroy(new DTImpact(this));
                        }
                    }
                    this.bulletFireIndex += 20;
                    if (Network.isActive)
                    {
                        NMFireGun gunEvent = new NMFireGun(this, this.firedBullets, this.bulletFireIndex, false, 4, false);
                        Send.Message(gunEvent, NetMessagePriority.ReliableOrdered, null);
                        this.firedBullets.Clear();
                    }
                }
            }
            else
                SFX.Play("click");
        }

        public override void OnReleaseAction()
        {

            Duck d = duck ?? prevOwner as Duck;
            if (d != null)
            {
                d.immobilized = false;
                d.remoteControl = false;
            }
            //if (_wait == 0f && aiming)
            //    Fire();
            //aiming = false;
        }
    }
}
