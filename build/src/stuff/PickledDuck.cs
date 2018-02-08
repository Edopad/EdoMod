using System;

namespace DuckGame
{
    public class PickledDuck : TrappedDuck
    {
        public StateBinding _duckOwnerBinding = new StateBinding("_duckOwner", -1, false, false);

        public Duck _duckOwner;

        public float _trapTime = 1f;

        public float _shakeMult;

        private float _shakeInc;

        public byte funNum;

        public bool infinite;

        private float jumpCountdown;

        private bool _prevVisible;

        public Duck captureDuck
        {
            get
            {
                return this._duckOwner;
            }
        }

        public override bool visible
        {
            get
            {
                return base.visible;
            }
            set
            {
                if (NetworkDebugger.networkDrawingIndex == 0 && this._duckOwner.netProfileIndex == 1 && (value || !base.visible) && value)
                {
                    bool arg_29_0 = base.visible;
                }
                if (this._duckOwner.netProfileIndex == 1 && this._duckOwner.isServerForObject && (value || !base.visible) && value)
                {
                    bool arg_59_0 = base.visible;
                }
                if (value && this._trapTime < 0f)
                {
                    this._trapTime = 1f;
                    this.owner = null;
                }
                base.visible = value;
            }
        }

        public PickledDuck(float xpos, float ypos, Duck duckowner) : base(xpos, ypos, duckowner)
        {
            this.center = new Vec2(16f, 16f);
            this.collisionOffset = new Vec2(-8f, -8f);
            this.collisionSize = new Vec2(16f, 16f);
            base.depth = -0.5f;
            this.thickness = 0.5f;
            this.weight = 5f;
            this.flammable = 1f;
            this.burnSpeed = 0f;
            this._duckOwner = duckowner;
            this.InitializeStuff();
        }

        public void InitializeStuff()
        {
            this._trapTime = 1f;
        }

        protected override bool OnBurn(Vec2 firePosition, Thing litBy)
        {
            if (this._duckOwner != null)
            {
                this._duckOwner.Burn(firePosition, litBy);
            }
            return base.OnBurn(firePosition, litBy);
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        protected override bool OnDestroy(DestroyType type = null)
        {
            if (this._duckOwner == null)
            {
                return false;
            }
            if (!this.destroyed)
            {
                this._duckOwner.hSpeed = this.hSpeed;
                bool wasKilled = type != null;
                if (!wasKilled && this.jumpCountdown > 0.01f)
                {
                    this._duckOwner.vSpeed = Duck.JumpSpeed;
                }
                else
                {
                    this._duckOwner.vSpeed = (wasKilled ? (this.vSpeed - 1f) : -3f);
                }
                this._duckOwner.x = base.x;
                this._duckOwner.y = base.y - 10f;
                for (int i = 0; i < 4; i++)
                {
                    SmallSmoke s = SmallSmoke.New(base.x + Rando.Float(-4f, 4f), base.y + Rando.Float(-4f, 4f));
                    s.hSpeed += this.hSpeed * Rando.Float(0.3f, 0.5f);
                    s.vSpeed -= Rando.Float(0.1f, 0.2f);
                    Level.Add(s);
                }
                if (this.owner != null)
                {
                    Duck d = this.owner as Duck;
                    if (d != null)
                    {
                        d.holdObject = null;
                    }
                }
                if (Network.isActive)
                {
                    if (!wasKilled)
                    {
                        this._duckOwner.Fondle(this);
                        this.authority += 45;
                    }
                    this.active = false;
                    this.visible = false;
                    this.owner = null;
                }
                else
                {
                    Level.Remove(this);
                }
                if (wasKilled && !this._duckOwner.killingNet)
                {
                    this._duckOwner.killingNet = true;
                    this._duckOwner.Destroy(type);
                }
                this._duckOwner._trapped = null;
            }
            return true;
        }

        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            if (bullet.isLocal)
            {
                this.OnDestroy(new DTShot(bullet));
            }
            return base.Hit(bullet, hitPos);
        }

        public override void ExitHit(Bullet bullet, Vec2 exitPos)
        {
        }

        public override void Update()
        {
            if (Network.isActive && this._prevVisible && !this.visible)
            {
                for (int i = 0; i < 4; i++)
                {
                    SmallSmoke s = SmallSmoke.New(base.x + Rando.Float(-4f, 4f), base.y + Rando.Float(-4f, 4f));
                    s.hSpeed += this.hSpeed * Rando.Float(0.3f, 0.5f);
                    s.vSpeed -= Rando.Float(0.1f, 0.2f);
                    Level.Add(s);
                }
            }
            if (this._duckOwner == null)
            {
                return;
            }
            this._framesSinceTransfer++;
            base.Update();
            if (base.y > Level.current.lowestPoint + 100f)
            {
                this.OnDestroy(new DTFall());
            }
            this.jumpCountdown -= Maths.IncFrameTimer();
            this._prevVisible = this.visible;
            this._shakeInc += 0.8f;
            this._shakeMult = Lerp.Float(this._shakeMult, 0f, 0.05f);
            if (Network.isActive && this._duckOwner._trapped == this && !this._duckOwner.isServerForObject && this._duckOwner.inputProfile.Pressed("JUMP", false))
            {
                this._shakeMult = 1f;
            }
            if (this._duckOwner.isServerForObject && this._duckOwner._trapped == this)
            {
                if (!this.visible)
                {
                    base.y = -9999f;
                }
                if (!this.infinite)
                {
                    this._duckOwner.profile.stats.timeInNet += Maths.IncFrameTimer();
                    if (this._duckOwner.inputProfile.Pressed("JUMP", false))
                    {
                        this._shakeMult = 1f;
                        this._trapTime -= 0.007f;
                        this.jumpCountdown = 0.25f;
                    }
                    if (base.grounded && this._duckOwner.inputProfile.Pressed("JUMP", false))
                    {
                        this._shakeMult = 1f;
                        this._trapTime -= 0.028f;
                        if (this.owner == null)
                        {
                            if (Math.Abs(this.hSpeed) < 1f && this._framesSinceTransfer > 30)
                            {
                                this._duckOwner.Fondle(this);
                            }
                            this.vSpeed -= Rando.Float(0.8f, 1.1f);
                            if (this._duckOwner.inputProfile.Down("LEFT") && this.hSpeed > -1f)
                            {
                                this.hSpeed -= Rando.Float(0.6f, 0.8f);
                            }
                            if (this._duckOwner.inputProfile.Down("RIGHT") && this.hSpeed < 1f)
                            {
                                this.hSpeed += Rando.Float(0.6f, 0.8f);
                            }
                        }
                    }
                    if (this._duckOwner.inputProfile.Pressed("JUMP", false) && this._duckOwner.HasEquipment(typeof(Jetpack)))
                    {
                        Equipment e = this._duckOwner.GetEquipment(typeof(Jetpack));
                        e.PressAction();
                    }
                    if (this._duckOwner.inputProfile.Released("JUMP") && this._duckOwner.HasEquipment(typeof(Jetpack)))
                    {
                        Equipment e2 = this._duckOwner.GetEquipment(typeof(Jetpack));
                        e2.ReleaseAction();
                    }
                    this._trapTime -= 0.0028f;
                    if (this._trapTime <= 0f || this._duckOwner.dead)
                    {
                        this.OnDestroy(null);
                    }
                }
                if (this.owner == null)
                {
                    base.depth = this._duckOwner.depth;
                }
                this._duckOwner.position = this.position;
                this._duckOwner.UpdateSkeleton();
            }
        }

        public override void Draw()
        {
            if (this._duckOwner == null)
            {
                return;
            }
            this._duckOwner._sprite.SetAnimation("netted");
            this._duckOwner._sprite.imageIndex = 14;
            this._duckOwner._spriteQuack.frame = this._duckOwner._sprite.frame;
            this._duckOwner._sprite.depth = base.depth;
            this._duckOwner._spriteQuack.depth = base.depth;
            float shakeOffset = 0f;
            if (this.owner != null)
            {
                shakeOffset = (float)Math.Sin((double)this._shakeInc) * this._shakeMult * 1f;
            }
            if (this._duckOwner.quack > 0)
            {
                Graphics.Draw(this._duckOwner._spriteQuack, this._duckOwner._sprite.imageIndex, base.x + shakeOffset, base.y - 8f, 1f, 1f, false);
            }
            else
            {
                Graphics.Draw(this._duckOwner._sprite, base.x + shakeOffset, base.y - 8f);
            }
            base.Draw();
        }
    }
}
