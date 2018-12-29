using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace DuckGame.EdoMod
{
    [BaggedProperty("isFatal", false)]
    [BaggedProperty("canSpawn", true)]
    [EditorGroup("EdoMod")]
    public class ToBeContinued : Grenade
    {
        public StateBinding _realTimerStateBinding = new StateBinding("_realTimer");
        public StateBinding _detonationTriggerStateBinding = new StateBinding("_detonationTrigger");
        public StateBinding _frameStateBinding = new StateBinding("spriteFrame");

        public float _realTimer;
        public int _detonationTrigger;

        protected SpriteMap sprite;

        public byte spriteFrame
        {
            get
            {
                if (sprite == null)
                    return (byte)0;
                return (byte)sprite._frame;
            }
            set
            {
                if (sprite == null)
                    return;
                sprite._frame = (int)value;
            }
        }

        public ToBeContinued(float xpos, float ypos) :
            base(xpos, ypos)
        {
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("weapons\\TBCGrenade"), 8, 10);
            graphic = sprite;
            center = new Vec2(4f, 5f);
            _holdOffset = new Vec2(-2f, 2f);

            _detonationTrigger = 0;
            _realTimer = 2f;

            _editorName = "To Be Continued";
        }

        public override void Update()
        {
            _timer = 99f;
            base.Update();

            PinUpdate();

            if (_realTimer <= 0f && _detonationTrigger == 0)
            {
                //Shockwave();

                Explosion();
                _detonationTrigger++;
                _destroyed = true;

                //_detonationTrigger = -1;
                
            } else if(_detonationTrigger > 0)
            {
                _detonationTrigger++;
                if(_detonationTrigger == 9)
                {


                    Layer.Game.colorMul = 
                    Layer.Glow.colorMul = 
                    Layer.Parallax.colorMul = 
                    Layer.Background.colorMul = 
                    Layer.Blocks.colorMul = 
                    Layer.Lighting.colorMul = 
                    Layer.Lighting2.colorMul = new Vec3(255f, 249f, 201f) / 255f;

                    /*Layer.Game.darken = 
                    Layer.Glow.darken =
                    Layer.Parallax.darken =
                    Layer.Background.darken =
                    Layer.Blocks.darken =
                    Layer.Lighting.darken =
                    Layer.Lighting2.darken = 1f / 8f;*/
                }
                if(_detonationTrigger == 10)
                {
                    SFX.KillAllSounds();
                    OverrideLevelCalls();

                    Thread thread = new Thread(new ThreadStart(DelayCallback));
                    thread.Start();

                    //DelayCallback();
                    Level.Remove(this);
                }
                
            }

            sprite.frame = _pin ? 0 : 1;

            if(_triggered) Graphics.Draw(_snap, 0, 0);


        }

        protected virtual void PinUpdate()
        {
            if (!_pin)
            {
                sprite.frame = 1;
                _realTimer -= 0.01f;
            }
            else
                sprite.frame = 0;
        }

        Tex2D _snap = Content.Load<Tex2D>(EdoMod.GetPath<EdoMod>("images\\TBCArrow"));
        bool _triggered = false;

        protected virtual void Explosion()
        {
            

            IEnumerable<Thing> ducklist = Level.current.things[typeof(Duck)];

            Level.current.simulatePhysics = false;

            foreach (Duck duck in ducklist)
            {
                if (duck == null) throw new PropertyNotFoundException("WTF!");
                duck.immobilized = true;
                duck.crippleTimer = 4f;
            }

            Level cl = Level.current;
            SpriteThing tbcarrow = new SpriteThing(0f, 0f, new Sprite(EdoMod.GetPath<EdoMod>("images\\TBCArrow")));
            tbcarrow.layer = Layer.HUD;
            tbcarrow.scale = new Vec2(0.5f,0.5f);
            tbcarrow.position = new Vec2(0f, Layer.HUD.height) + 1.1f * new Vec2(tbcarrow.halfWidth, -tbcarrow.halfHeight);
            tbcarrow.z = 1000000;

            cl.AddThing(tbcarrow);

            //DelayCallback();
            //typeof(Level).GetField("_updateWaitFrames", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Level.current, 500);
            //Thread thread = new Thread(DelayCallback);
            //thread.Start();
        }

        private static bool _LevelOverride = false;

        public static void UpdateCurrentLevel()
        {
            return;
        }

        public static void DrawCurrentLevel()
        {
            return;
        }

        private static void SwapLevelCalls()
        {
            //UpdateCurrentLevel
            {
                MethodInfo orig = typeof(ToBeContinued).GetMethod("UpdateCurrentLevel", BindingFlags.Static | BindingFlags.Public);
                MethodInfo newer = typeof(Level).GetMethod("UpdateCurrentLevel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                DynamicMojo.SwapMethodBodies(newer, orig);
            }
            //DrawCurrentLevel
            {
                MethodInfo orig = typeof(ToBeContinued).GetMethod("DrawCurrentLevel", BindingFlags.Static | BindingFlags.Public);
                MethodInfo newer = typeof(Level).GetMethod("DrawCurrentLevel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                DynamicMojo.SwapMethodBodies(newer, orig);
            }
        }

        private static void OverrideLevelCalls()
        {
            if (_LevelOverride) return;
            SwapLevelCalls();
            _LevelOverride = true;
        }

        private static void RestoreLevelCalls()
        {
            if (!_LevelOverride) return;
            SwapLevelCalls();
            _LevelOverride = false;
        }

        private void DelayCallback()
        {
            //level.updatecurrentlevel
            //level.drawcurrentlevel
            Thread.Sleep(4000);

            RestoreLevelCalls();

            Layer.Game.colorMul =
            Layer.Glow.colorMul =
            Layer.Parallax.colorMul =
            Layer.Background.colorMul =
            Layer.Blocks.colorMul =
            Layer.Lighting.colorMul =
            Layer.Lighting2.colorMul = new Vec3(0f); ;
            if (Level.current is GameLevel)
                (Level.current as GameLevel).SkipMatch();
            Music.Resume();
        }

        protected virtual void Shockwave()
        {
            //if (isServerForObject)
            //    Level.Add(new GlobalPulse(x, y));
            SFX.Play("explode");
        }

        public override void OnNetworkBulletsFired(Vec2 pos)
        {
            // do nothing
        }
        public override void OnPressAction()
        {
            
            if(_pin)
            {
                //Music.Pause();
                //SFX.Play(EdoMod.GetPath<EdoMod>("music\\tobecontinued"));
            }
            
            base.OnPressAction();
        }
    }
}