using System;

namespace DuckGame.EdoMod
{
    internal static class EdoExtensions
    {
        internal static byte PlayerIndex(this Duck duck)
        {
            return (byte)Persona.Number(duck.persona);
        }
    }
}
