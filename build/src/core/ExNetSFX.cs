using System;
using System.Collections.Generic;

namespace DuckGame.EdoMod
{
    public class ExNetSFX : NetSoundEffect
    {
        private List<string> _sounds = new List<string>();

        private List<string> _rareSounds = new List<string>();

        private int _localIndex;

        private int _index;

        public Sound currentSound;

        public new int index
        {
            get
            {
                return this._index;
            }
            set
            {
                this._index = value;
                if (this._localIndex != this._index)
                {
                    this._localIndex = this._index;
                    this.PlaySound(1f, 0f);
                }
            }
        }

        public ExNetSFX() : base()
        {
        }

        public ExNetSFX(params string[] sounds) : base(sounds)
        {
            this._sounds = new List<string>(sounds);
        }

        public ExNetSFX(List<string> sounds, List<string> rareSounds) : base(sounds, rareSounds)
        {
            this._sounds = sounds;
            this._rareSounds = rareSounds;
        }

        public virtual void Play(float vol = 1f, float pit = 0f)
        {
            this.PlaySound(vol, pit);
            this._index++;
            if (this._index > 3)
            {
                this._index = 0;
            }
            this._localIndex = this._index;
        }

        private void PlaySound(float vol = 1f, float pit = 0f)
        {
            this.function?.Invoke();
            vol *= this.volume;
            pit += this.pitch;
            pit += Rando.Float(this.pitchVariationLow, this.pitchVariationHigh);
            if (pit < -1f)
            {
                pit = -1f;
            }
            if (pit > 1f)
            {
                pit = 1f;
            }
            if (this._sounds.Count > 0)
            {
                if (this.pitchBinding != null)
                {
                    pit = (float)((byte)this.pitchBinding.value) / 255f;
                }
                string append = "";
                if (this.appendBinding != null)
                {
                    append = ((byte)this.appendBinding.value).ToString();
                }

                //If previous sound is already playing, don't play another.
                //if (currentSound != null)
                //    if (currentSound.State == Microsoft.Xna.Framework.Audio.SoundState.Playing) return;

                if (this._rareSounds.Count > 0 && Rando.Float(1f) > 0.9f)
                {
                    currentSound = SFX.Play(this._rareSounds[Rando.Int(this._rareSounds.Count - 1)] + append, vol, pit, 0f, false);
                }
                else
                {
                    currentSound = SFX.Play(this._sounds[Rando.Int(this._sounds.Count - 1)] + append, vol, pit, 0f, false);
                }
                
            }
        }
    }
}
