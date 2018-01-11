using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.EdoMod
{
    public static class EdoSFX
    {
        public static Sound Play(string sound, float vol = 1f, float pitch = 0f, float pan = 0f, bool looped = false)
        {
            if (sound != null) return Play(sound, vol, pitch, pan, looped);
            return null;
        }

        public static Sound _Play(string sound, float vol = 1f, float pitch = 0f, float pan = 0f, bool looped = false)
        {
            Console.WriteLine("This is here as a placeholder.");
            return null;
        }

    }
}
