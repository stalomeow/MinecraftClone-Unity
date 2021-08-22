using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Minecraft.Lua;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Minecraft.Rendering
{
    public abstract class MeshBuilder<TVertex, TIndex> : ILuaCallCSharp where TVertex : unmanaged where TIndex : unmanaged
    {
        private struct MeshIndex
        {
            public int SubMeshIndex;
            public TIndex Value;
        }


        public IndexFormat MeshIndexFormat { get; }

        private readonly List<TVertex> m_VertexBuffer;
        private readonly List<MeshIndex> m_IndexBuffer;
        private readonly int[] m_IndexOffsetBuffer;
        private readonly VertexAttributeDescriptor[] m_VertexAttributes;


        public int VertexCount => m_VertexBuffer.Count;

        public int IndexCount => m_IndexBuffer.Count;

        public int SubMeshCount => m_IndexOffsetBuffer.Length;


        protected unsafe MeshBuilder(VertexAttributeDescriptor[] vertexAttributes, int subMeshCount)
        {
            MeshIndexFormat = sizeof(TIndex) switch
            {
                2 => IndexFormat.UInt16,
                4 => IndexFormat.UInt32,
                _ => throw new NotSupportedException($"The index type '{typeof(TIndex)}' is not supported! The size of the type must be 2 or 4.")
            };

            m_VertexBuffer = new List<TVertex>();
            m_IndexBuffer = new List<MeshIndex>();
            m_IndexOffsetBuffer = new int[subMeshCount];
            m_VertexAttributes = vertexAttributes;
        }

        public void ClearBuffers()
        {
            m_VertexBuffer.Clear();
            m_IndexBuffer.Clear();
            Array.Clear(m_IndexOffsetBuffer, 0, m_IndexOffsetBuffer.Length);
        }

        public void AddVertex(in TVertex vertex)
        {
            m_VertexBuffer.Add(vertex);
        }

        public unsafe void AddIndex(int index, int subMesh = 0)
        {
            TIndex value;
            index += m_VertexBuffer.Count;

            switch (MeshIndexFormat)
            {
                case IndexFormat.UInt16:
                    ushort temp = checked((ushort)index);
                    value = *(TIndex*)&temp;
                    break;
                case IndexFormat.UInt32:
                    value = *(TIndex*)&index;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported index format '{MeshIndexFormat}'!");
            }

            m_IndexBuffer.Add(new MeshIndex
            {
                SubMeshIndex = subMesh,
                Value = value
            });

            for (int i = subMesh + 1; i < m_IndexOffsetBuffer.Length; i++)
            {
                m_IndexOffsetBuffer[i]++;
            }
        }

        public Bounds CalculateObjectSpaceBounds()
        {
            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;

            for (int i = 0; i < m_VertexBuffer.Count; i++)
            {
                Vector3 pos = GetPositionOS(m_VertexBuffer[i]);
                min = Vector3.Min(min, pos);
                max = Vector3.Max(max, pos);
            }

            return new Bounds((min + max) * 0.5f, max - min);
        }

        public void ApplyAndClearBuffers(Mesh.MeshData meshData, MeshTopology topology)
        {
            // 为了避免额外分配空间，SubMesh会在顶点和索引之前被提交，
            // 如果Unity执行检查就可能会报错（主要是RecalculateBounds）。
            const MeshUpdateFlags flags = MeshUpdateFlags.DontValidateIndices
                | MeshUpdateFlags.DontResetBoneBounds
                | MeshUpdateFlags.DontNotifyMeshUsers
                | MeshUpdateFlags.DontRecalculateBounds;

            meshData.subMeshCount = m_IndexOffsetBuffer.Length;
            meshData.SetVertexBufferParams(m_VertexBuffer.Count, m_VertexAttributes);
            meshData.SetIndexBufferParams(m_IndexBuffer.Count, MeshIndexFormat);

            // submesh
            for (int i = 0; i < SubMeshCount; i++)
            {
                SubMeshDescriptor subMesh = GetSubMeshDescriptor(i, topology);
                meshData.SetSubMesh(i, subMesh, flags);
            }

            // vertex
            NativeArray<TVertex> vertices = meshData.GetVertexData<TVertex>();
            for (int i = 0; i < m_VertexBuffer.Count; i++)
            {
                vertices[i] = m_VertexBuffer[i];
            }

            // index
            NativeArray<TIndex> indices = meshData.GetIndexData<TIndex>();
            for (int i = 0; i < m_IndexBuffer.Count; i++)
            {
                CopyMeshIndex(indices, m_IndexBuffer[i]);
            }

            ClearBuffers();
        }

        public void ApplyAndClearBuffers(Mesh mesh, MeshTopology topology, bool markNoLongerReadable, Allocator allocator)
        {
            // 为了避免额外分配空间，SubMesh会在顶点和索引之前被提交，
            // 如果Unity执行检查就可能会报错（主要是RecalculateBounds）。
            const MeshUpdateFlags flags = MeshUpdateFlags.DontValidateIndices
                | MeshUpdateFlags.DontResetBoneBounds
                | MeshUpdateFlags.DontNotifyMeshUsers
                | MeshUpdateFlags.DontRecalculateBounds;

            mesh.subMeshCount = 0; // 防止多个 subMesh 共享三角形的警告
            mesh.subMeshCount = m_IndexOffsetBuffer.Length;
            mesh.SetVertexBufferParams(m_VertexBuffer.Count, m_VertexAttributes);
            mesh.SetIndexBufferParams(m_IndexBuffer.Count, MeshIndexFormat);

            // submesh
            for (int i = 0; i < SubMeshCount; i++)
            {
                SubMeshDescriptor subMesh = GetSubMeshDescriptor(i, topology);
                mesh.SetSubMesh(i, subMesh, flags);
            }

            // vertex
            mesh.SetVertexBufferData<TVertex>(m_VertexBuffer, 0, 0, m_VertexBuffer.Count, 0, flags);

            // index
            NativeArray<TIndex> indices = new NativeArray<TIndex>(m_IndexBuffer.Count, allocator, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < m_IndexBuffer.Count; i++)
            {
                CopyMeshIndex(indices, m_IndexBuffer[i]);
            }
            mesh.SetIndexBufferData(indices, 0, 0, indices.Length, flags);
            indices.Dispose();

            mesh.UploadMeshData(markNoLongerReadable);
            ClearBuffers();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SubMeshDescriptor GetSubMeshDescriptor(int subMesh, MeshTopology topology)
        {
            int nextSubMesh = subMesh + 1;
            int offset = m_IndexOffsetBuffer[subMesh];
            int indexCount = (nextSubMesh < SubMeshCount ? m_IndexOffsetBuffer[nextSubMesh] : m_IndexBuffer.Count) - offset;
            return new SubMeshDescriptor(offset, indexCount, topology);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CopyMeshIndex(NativeArray<TIndex> destination, MeshIndex index)
        {
            // 由于每一个SubMesh占有一段连续的Index，所以这里根据Index所属的SubMesh来进行填充。
            destination[m_IndexOffsetBuffer[index.SubMeshIndex]++] = index.Value;
        }


        protected abstract Vector3 GetPositionOS(in TVertex vertex);
    }
}
