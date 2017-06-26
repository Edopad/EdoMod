using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    class EdoLogo : CorptronLogo
    {
        private BitmapFont _font;

        private Sprite _logo;

        private float _wait = 1f;

        private bool _fading;

        public EdoLogo()
        {
            this._centeredView = true;
        }

        public override void Initialize()
        {
            this._font = new BitmapFont("biosFont", 8, -1);
            this._logo = new Sprite(EdoMod.GetPath<EdoMod>("images\\logo"), 0f, 0f);
            Graphics.fade = 0f;
        }

        public override void Update()
        {
            if (!this._fading)
            {
                if (Graphics.fade < 1f)
                {
                    Graphics.fade += 0.013f;
                }
                else
                {
                    Graphics.fade = 1f;
                }
            }
            else if (Graphics.fade > 0f)
            {
                Graphics.fade -= 0.013f;
            }
            else
            {
                Graphics.fade = 0f;
                Level.current = new AdultSwimLogo();
            }
            this._wait -= 0.006f;
            if (this._wait < 0f || Input.Pressed("START", "Any") || Input.Pressed("SELECT", "Any"))
            {
                this._fading = true;
            }
        }

        public override void PostDrawLayer(Layer layer)
        {
            if (layer == Layer.Game)
            {
                Graphics.Draw(this._logo, 32f, 70f);
                this._font.Draw("PRESENTED BY", 50f, 60f, Color.White, default(Depth), null, false);
            }
        }
    }
}
