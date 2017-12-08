using System;

namespace DuckGame.EdoMod
{
    public class StarDuckHandler : Thing
    {
        public StateBinding _positionStateBinding = new CompressedVec2Binding("position");
        public StateBinding _stunTargetStateBinding = new StateBinding("_stunTarget");
        public StateBinding _stunTimeStateBinding = new StateBinding("_stunTime");
        public StateBinding _showDazeStateBinding = new StateBinding("_showDaze");
        public StateBinding _overriderStateBinding = new StateBinding("_overrider");

        public Duck _stunTarget;
        public int _stunTime;
        public bool _showDaze;
        public bool _overrider;

        private bool _fixH;
        private bool _fixV;

        public StarDuckHandler(Duck stunTarget, int stunTime = 10, bool fixH = false, bool fixV = false, bool showDaze = false)
        {
            _stunTarget = stunTarget;
            _stunTime = stunTime;
            _fixH = fixH;
            _fixV = fixV;
            _showDaze = showDaze;
            foreach (Thing t in Level.current.things)
                if (t is StarDuckHandler && t != this && ((StarDuckHandler)t)._stunTarget == stunTarget && !((StarDuckHandler)t)._overrider)
                {
                    if (showDaze)
                        ((StarDuckHandler)t)._showDaze = true;
                    if (stunTime > ((StarDuckHandler)t)._stunTime)
                        ((StarDuckHandler)t)._stunTime = stunTime;
                    _overrider = true;
                }

            mover = new SinWave(1f / 15f);
            ipos = _stunTarget.position;
            _stunTarget.GoRagdoll();
        }

        private SinWave mover;
        private Vec2 ipos;
        private Depth idpth;

        public override void Update()
        {
            if (_overrider || _stunTime <= 0 || _stunTarget == null)
            {
                if (!_overrider && _stunTarget != null)
                    _stunTarget.immobilized = false;
                Level.Remove(this);
                return;
            }

            _stunTarget.immobilized = true;

            Ragdoll rag = _stunTarget.ragdoll;

            rag._part1.vSpeed += NetRand.Float(-2f, 2f);
            rag._part3.vSpeed += NetRand.Float(-2f, 2f);
            rag._part2.vSpeed -= NetRand.Float(1.5f, 2f);
            rag._part2.position = ipos + new Vec2(10f * mover.value);
            rag._timeSinceNudge = 0f;

            _stunTime--;

            base.Update();
        }

        public override void Draw()
        {
            //if (!_overrider && _showDaze)
            //    Graphics.Draw(swirl, position.x, position.y);
            base.Draw();
        }

        public override void Terminate()
        {
            //_stunTarget.depth = idpth;
            base.Terminate();
        }
    }
}