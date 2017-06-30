using System;

namespace DuckGame.EdoMod
{
    public class StarHandler : Thing
    {
        public StateBinding _stunTimeStateBinding = new StateBinding("_stunTime");
        public int _stunTime;

        public StarHandler(int stunTime = 10)
        {
            _stunTime = stunTime;
        }

        public override void Update()
        {
            if (_stunTime <= 0)
            {
                Level.Remove(this);
                return;
            }

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