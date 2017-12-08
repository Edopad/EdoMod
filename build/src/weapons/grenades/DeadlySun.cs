using System.Collections.Generic;
using System.Threading;

namespace DuckGame.EdoMod
{
    [EditorGroup("EdoMod")]
    public class DeadlySun : Grenade
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

        public DeadlySun(float xpos, float ypos) :
            base(xpos, ypos)
        {
            sprite = new SpriteMap(Mod.GetPath<EdoMod>("weapons\\SunGrenade"), 8, 10);
            graphic = sprite;
            center = new Vec2(4f, 5f);
            _holdOffset = new Vec2(-2f, 2f);

            _detonationTrigger = 0;
            _realTimer = 1f;

            _editorName = "Deadly Sun";
        }

        public override void Update()
        {
            _timer = 99f;
            base.Update();

            PinUpdate();

            if (_realTimer <= 0f && _detonationTrigger == 0)
            {
                Explosion();

                _detonationTrigger++;
                _destroyed = true;
                Level.Remove(this);
            }

            sprite.frame = _pin ? 0 : 1;
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

        protected virtual void Explosion()
        {
            SFX.Play(Mod.GetPath<EdoMod>("SFX\\the-sun-is-a-deadly-lazer"), looped: false);
            // IList<Duck> duckList = new List<Duck>();
            Thread thread = new Thread(DelayCallback);
            thread.Start();
        }

        private void DelayCallback()
        {
            Thread.Sleep(2100);
            IEnumerable<Thing> ducklist = Level.current.things[typeof(IAmADuck)];

            foreach (MaterialThing duck in ducklist)
            {
                //if (duck == null) throw new PropertyNotFoundException("WTF!");
                duck.Burn(duck.position, this);
            }
        }

        public override void OnNetworkBulletsFired(Vec2 pos)
        {
            // do nothing
        }
    }
}