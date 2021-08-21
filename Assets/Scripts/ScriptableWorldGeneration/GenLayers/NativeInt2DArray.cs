using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Lua;
using Unity.Collections;

namespace Minecraft.ScriptableWorldGeneration.GenLayers
{
    [XLua.GCOptimize]
    public struct NativeInt2DArray : IEnumerable<int>, IDisposable, ILuaCallCSharp
    {
        private NativeArray<int> m_Array;
        private readonly int m_Length0;

        public int this[int index0, int index1]
        {
            get => m_Array[m_Length0 * index0 + index1];
            set => m_Array[m_Length0 * index0 + index1] = value;
        }

        public NativeInt2DArray(int Length0, int Length1, Allocator allocator)
        {
            m_Array = new NativeArray<int>(Length0 * Length1, allocator);
            m_Length0 = Length0;
        }

        public void Dispose()
        {
            m_Array.Dispose();
        }

        public NativeArray<int>.Enumerator GetEnumerator()
        {
            return m_Array.GetEnumerator();
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
