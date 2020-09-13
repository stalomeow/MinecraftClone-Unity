using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using static Minecraft.WorldConsts;

namespace Minecraft.Buffers
{
    /// <summary>
    /// 表示一个mesh数据缓冲区
    /// </summary>
    public class MeshDataBuffer
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct VertexData
        {
            public float X;
            public float Y;
            public float Z;

            public float NORMAL_X;
            public float NORMAL_Y;
            public float NORMAL_Z;

            public float UV_X;
            public float UV_Y;
            public float LIGHT;
        }

        private static readonly VertexAttributeDescriptor[] s_VertexLayout = new VertexAttributeDescriptor[]
        {
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 3)
        };

        private static readonly Bounds s_SectionBounds = new Bounds(
            new Vector3(ChunkWidth >> 1, SectionHeight >> 1, ChunkWidth >> 1),
            new Vector3(ChunkWidth, SectionHeight, ChunkWidth)
        );

        private static readonly MeshUpdateFlags s_MeshUpdateFlags = MeshUpdateFlags.DontValidateIndices
                                                                    | MeshUpdateFlags.DontNotifyMeshUsers
                                                                    | MeshUpdateFlags.DontRecalculateBounds
                                                                    | MeshUpdateFlags.DontResetBoneBounds;

        private const int InitialCapacity = 30;


        private readonly List<VertexData> m_VertexBuffer;
        private readonly List<int> m_TriangleBuffer;


        public int VertexCount => m_VertexBuffer.Count;

        public int TriangleCount => m_TriangleBuffer.Count;


        public MeshDataBuffer()
        {
            m_VertexBuffer = new List<VertexData>(InitialCapacity);
            m_TriangleBuffer = new List<int>(InitialCapacity);
        }


        /// <summary>
        /// 添加一个三角面，每个顶点会添加一个指定的偏移（<see cref="VertexCount"/>）
        /// </summary>
        /// <param name="a">顶点1</param>
        /// <param name="b">顶点2</param>
        /// <param name="c">顶点3</param>
        public void AddTriangle(int a, int b, int c)
        {
            int vertexCount = m_VertexBuffer.Count;

            m_TriangleBuffer.Add(a + vertexCount);
            m_TriangleBuffer.Add(b + vertexCount);
            m_TriangleBuffer.Add(c + vertexCount);
        }

        /// <summary>
        /// 添加一个顶点
        /// </summary>
        /// <param name="pos">顶点坐标</param>
        /// <param name="normal">法线</param>
        /// <param name="uv">贴图位置</param>
        /// <param name="light">光照强度</param>
        public void AddVertex(Vector3 pos, Vector3 normal, Vector2 uv, float light)
        {
            VertexData vertex = new VertexData
            {
                X = pos.x,
                Y = pos.y,
                Z = pos.z,

                NORMAL_X = normal.x,
                NORMAL_Y = normal.y,
                NORMAL_Z = normal.z,

                UV_X = uv.x,
                UV_Y = uv.y,

                LIGHT = light
            };

            m_VertexBuffer.Add(vertex);
        }

        public void BeginRewriting()
        {
            m_VertexBuffer.Clear();
            m_TriangleBuffer.Clear();
        }

        public void ApplyToMesh(ref Mesh mesh)
        {
            if (mesh)
            {
                mesh.Clear();
            }
            else
            {
                mesh = new Mesh
                {
                    indexFormat = SystemInfo.supports32bitsIndexBuffer ? IndexFormat.UInt32 : IndexFormat.UInt16,
                    bounds = s_SectionBounds
                };
                mesh.MarkDynamic();
            }

            mesh.SetVertexBufferParams(m_VertexBuffer.Count, s_VertexLayout);
            mesh.SetVertexBufferData(m_VertexBuffer, 0, 0, m_VertexBuffer.Count, flags: s_MeshUpdateFlags);
            mesh.SetTriangles(m_TriangleBuffer, 0, false);
            mesh.UploadMeshData(false);
        }
    }
}