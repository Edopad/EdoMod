using System;
using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    [BaggedProperty("canSpawn", false)]
    public class PimpRCCar : RCCar
    {
        public StateBinding _signalBinding = new StateBinding("receivingSignal", -1, false, false);

        public StateBinding _idleSpeedBinding = new CompressedFloatBinding("_idleSpeed", 1f, 4, false, false);

        private SpriteMap _sprite;

        private float _tilt;

        private float _maxSpeed = 6f;

        private SinWave _wave = new SinWave(0.1f, 0f);

        private float _waveMult;

        private Sprite _wheel;

        public bool moveLeft;

        public bool moveRight;

        public bool jump;

        private bool _receivingSignal;

        private int _inc;

        public float _idleSpeed;

        private ConstantSound _idle = new ConstantSound("rcDrive", 0f, 0f, null);

        public bool receivingSignal
        {
            get
            {
                return this._receivingSignal;
            }
            set
            {
                if (this._receivingSignal != value && !this.destroyed)
                {
                    if (value)
                    {
                        SFX.Play("rcConnect", 0.5f, 0f, 0f, false);
                    }
                    else
                    {
                        SFX.Play("rcDisconnect", 0.5f, 0f, 0f, false);
                    }
                }
                this._receivingSignal = value;
            }
        }

        public PimpRCCar(float xpos, float ypos) : base(xpos, ypos)
        {
            this._sprite = new SpriteMap("rcBody", 32, 32, false);
            SpriteMap arg_9C_0 = this._sprite;
            string arg_9C_1 = "idle";
            float arg_9C_2 = 1f;
            bool arg_9C_3 = true;
            int[] frames = new int[1];
            arg_9C_0.AddAnimation(arg_9C_1, arg_9C_2, arg_9C_3, frames);
            this._sprite.AddAnimation("beep", 0.2f, true, new int[]
            {
                0,
                1
            });
            this.graphic = this._sprite;
            this.center = new Vec2(16f, 24f);
            this.collisionOffset = new Vec2(-8f, 0f);
            this.collisionSize = new Vec2(16f, 11f);
            base.depth = -0.5f;
            this._editorName = "RC Car";
            this.thickness = 2f;
            this.weight = 5f;
            this.flammable = 0.3f;
            this._wheel = new Sprite("rcWheel", 0f, 0f);
            this._wheel.center = new Vec2(4f, 4f);
            this.weight = 0.5f;
            this.physicsMaterial = PhysicsMaterial.Metal;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Terminate()
        {
            base.Terminate();
            this._idle.Kill();
            this._idle.lerpVolume = 0f;
        }

        protected override bool OnDestroy(DestroyType type = null)
        {
            if (!base.isServerForObject)
            {
                return false;
            }
            ATRCShrapnel shrap = new ATRCShrapnel();
            shrap.MakeNetEffect(this.position, false);
            List<Bullet> firedBullets = new List<Bullet>();
            for (int i = 0; i < 20; i++)
            {
                float dir = (float)i * 18f - 5f + Rando.Float(10f);
                shrap = new ATRCShrapnel();
                shrap.range = 55f + Rando.Float(14f);
                Bullet bullet = new Bullet(base.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * 6.0), base.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * 6.0), shrap, dir, null, false, -1f, false, true);
                bullet.firedFrom = this;
                firedBullets.Add(bullet);
                Level.Add(bullet);
            }
            if (Network.isActive)
            {
                NMFireGun gunEvent = new NMFireGun(null, firedBullets, 0, false, 4, false);
                Send.Message(gunEvent, NetMessagePriority.ReliableOrdered, null);
                firedBullets.Clear();
            }
            Level.Remove(this);
            FollowCam cam = Level.current.camera as FollowCam;
            if (cam != null)
            {
                cam.Remove(this);
            }
            if (Recorder.currentRecording != null)
            {
                Recorder.currentRecording.LogBonus();
            }
            return true;
        }

        public override bool Hit(Bullet bullet, Vec2 hitPos)
        {
            if (bullet.isLocal && this.owner == null)
            {
                Thing.Fondle(this, DuckNetwork.localConnection);
            }
            if (bullet.isLocal)
            {
                this.Destroy(new DTShot(bullet));
            }
            return false;
        }

        public override void Update()
        {
            base.Update();
            this._sprite.currentAnimation = (this._receivingSignal ? "beep" : "idle");
            this._idle.lerpVolume = Math.Min(this._idleSpeed * 10f, 0.7f);
            if (base._destroyed)
            {
                this._idle.lerpVolume = 0f;
                this._idle.lerpSpeed = 1f;
            }
            this._idle.pitch = 0.5f + this._idleSpeed * 0.5f;
            if (this.moveLeft)
            {
                if (this.hSpeed > -this._maxSpeed)
                {
                    this.hSpeed -= 0.4f;
                }
                else
                {
                    this.hSpeed = -this._maxSpeed;
                }
                this.offDir = -1;
                this._idleSpeed += 0.03f;
                this._inc++;
            }
            if (this.moveRight)
            {
                if (this.hSpeed < this._maxSpeed)
                {
                    this.hSpeed += 0.4f;
                }
                else
                {
                    this.hSpeed = this._maxSpeed;
                }
                this.offDir = 1;
                this._idleSpeed += 0.03f;
                this._inc++;
            }
            if (this._idleSpeed > 0.1f)
            {
                this._inc = 0;
                Level.Add(SmallSmoke.New(base.x - (float)(this.offDir * 10), base.y));
            }
            if (!this.moveLeft && !this.moveRight)
            {
                this._idleSpeed -= 0.03f;
            }
            if (this._idleSpeed > 1f)
            {
                this._idleSpeed = 1f;
            }
            if (this._idleSpeed < 0f)
            {
                this._idleSpeed = 0f;
            }
            if (this.jump && base.grounded)
            {
                this.vSpeed -= 4.8f;
            }
            this._tilt = MathHelper.Lerp(this._tilt, -this.hSpeed, 0.4f);
            this._waveMult = MathHelper.Lerp(this._waveMult, -this.hSpeed, 0.1f);
            base.angleDegrees = this._tilt * 2f + this._wave.value * (this._waveMult * (this._maxSpeed - Math.Abs(this.hSpeed)));
            if (base.isServerForObject && base.y > Level.current.lowestPoint + 100f && !this.destroyed)
            {
                this.Destroy(new DTFall());
            }
        }

        public override void Draw()
        {
            if (this.owner == null)
            {
                this._sprite.flipH = ((float)this.offDir < 0f);
            }
            base.Draw();
            Graphics.Draw(this._wheel, base.x - 7f, base.y + 9f);
            Graphics.Draw(this._wheel, base.x + 7f, base.y + 9f);
        }
    }
}