using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DuckGame.EdoMod
{
    public static class DynamicMojo
    {
        /// <summary>
        /// Swaps the function pointers for a and b, effectively swapping the method bodies.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// a and b must have same signature
        /// </exception>
        /// <param name="a">Method to swap</param>
        /// <param name="b">Method to swap</param>
        public static void SwapMethodBodies(MethodInfo b, MethodInfo a)
        {
            if (!HasSameSignature(a, b))
            {
                throw new ArgumentException("a and b must have have same signature");
            }

            RuntimeHelpers.PrepareMethod(a.MethodHandle);
            RuntimeHelpers.PrepareMethod(b.MethodHandle);

            if (b.IsVirtual)
            {
                //replace virtual
                ReplaceVirtualInner(a, b);
            }
            else
            {
                //replace normal
                ReplaceInner(a, b);
            }

            
        }

        static void ReplaceInner(MethodInfo methodToReplace, MethodInfo methodToInject)
        {
            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
                    *tar = *inj;
                }
                else
                {
                    ulong* inj = (ulong*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    ulong* tar = (ulong*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
                    *tar = *inj;
                }
            }
        }

        static void ReplaceVirtualInner(MethodInfo methodToReplace, MethodInfo methodToInject)
        {
            unsafe
            {
                UInt64* methodDesc = (UInt64*)(methodToReplace.MethodHandle.Value.ToPointer());
                int index = (int)(((*methodDesc) >> 32) & 0xFF);
                if (IntPtr.Size == 4)
                {
                    uint* classStart = (uint*)methodToReplace.DeclaringType.TypeHandle.Value.ToPointer();
                    classStart += 10;
                    classStart = (uint*)*classStart;
                    uint* tar = classStart + index;

                    uint* inj = (uint*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    //int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
                    *tar = *inj;
                }
                else
                {
                    ulong* classStart = (ulong*)methodToReplace.DeclaringType.TypeHandle.Value.ToPointer();
                    classStart += 8;
                    classStart = (ulong*)*classStart;
                    ulong* tar = classStart + index;

                    ulong* inj = (ulong*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    //ulong* tar = (ulong*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
                    *tar = *inj;
                }
            }
        }

        private static bool HasSameSignature(MethodInfo a, MethodInfo b)
        {
            bool sameParams = !a.GetParameters().Any(x => !b.GetParameters().Any(y => x == y));
            bool sameReturnType = a.ReturnType == b.ReturnType;
            return sameParams && sameReturnType;
        }
    }
}
