using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace Minecraft.Rendering
{
    /// <summary>
    /// 表示一个mesh
    /// </summary>
    public abstract class AbstractMesh
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
            public float TEX_OFFSET;
            public float LIGHT;
        }

        private static readonly VertexAttributeDescriptor[] s_VertexLayout = new VertexAttributeDescriptor[]
        {
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4)
        };


        private readonly List<VertexData> m_VertexBuffer;
        private readonly List<int> m_TriangleBuffer;
        private bool m_AllowRendering;
        private Mesh m_Mesh;


        /// <summary>
        /// 获取缓冲区中的顶点数量
        /// </summary>
        public int VertexCountInBuffer => m_VertexBuffer.Count;

        /// <summary>
        /// 获取缓冲区中的三角形数量
        /// </summary>
        public int TriangleCountInBuffer => m_TriangleBuffer.Count;


        protected AbstractMesh(int vertexCapacity, int triangleCapacity)
        {
            m_VertexBuffer = new List<VertexData>(vertexCapacity);
            m_TriangleBuffer = new List<int>(triangleCapacity);
            m_AllowRendering = false;
            m_Mesh = null;
        }


        /// <summary>
        /// 添加一个三角面，每个顶点会添加一个指定的偏移（<see cref="VertexCountInBuffer"/>）
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
        /// <param name="texOffset">贴图在贴图组中的偏移量</param>
        /// <param name="light">光照强度</param>
        public void AddVertex(Vector3 pos, Vector3 normal, Vector2 uv, int texOffset, float light)
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
                TEX_OFFSET = texOffset,
                LIGHT = light
            };

            m_VertexBuffer.Add(vertex);
        }

        /// <summary>
        /// 渲染当前的mesh
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="material"></param>
        /// <param name="property"></param>
        /// <param name="camera"></param>
        /// <param name="layer"></param>
        public void Render(Vector3 position, Quaternion rotation, Material material, MaterialPropertyBlock property, Camera camera, int layer)
        {
            if (m_AllowRendering && m_Mesh && m_Mesh.vertexCount > 0)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
                Graphics.DrawMesh(m_Mesh, matrix, material, layer, camera, 0, property, ShadowCastingMode.Off, false, null, LightProbeUsage.Off, null);
            }
        }

        /// <summary>
        /// 将缓冲区数据提交至mesh中
        /// </summary>
        public void Apply()
        {
            Profiler.BeginSample("Apply Mesh");

            if (m_Mesh)
            {
                m_Mesh.Clear(false);
            }
            else
            {
                m_Mesh = CreateNewMesh();
            }

            MeshUpdateFlags flags = GetMeshUpdateFlags();
            m_Mesh.SetVertexBufferParams(m_VertexBuffer.Count, s_VertexLayout);
            m_Mesh.SetVertexBufferData(m_VertexBuffer, 0, 0, m_VertexBuffer.Count, 0, flags);
            m_Mesh.SetTriangles(m_TriangleBuffer, 0, false);
            m_Mesh.UploadMeshData(false);

            m_AllowRendering = true;

            Profiler.EndSample();
        }

        /// <summary>
        /// 尝试释放缓冲区中多余的空间
        /// </summary>
        public void TrimExcess()
        {
            m_VertexBuffer.TrimExcess();
            m_TriangleBuffer.TrimExcess();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        /// <param name="clearNativeData">是否清空mesh中的非托管数据</param>
        public void Clear(bool clearNativeData)
        {
            if (clearNativeData && m_Mesh)
            {
                m_Mesh.Clear(false);
            }

            m_VertexBuffer.Clear();
            m_TriangleBuffer.Clear();
        }

        /// <summary>
        /// 销毁mesh
        /// </summary>
        public void Destroy()
        {
            if (m_Mesh)
            {
                Object.Destroy(m_Mesh);
                m_Mesh = null;
            }
        }

        /// <summary>
        /// 禁用渲染，直到下一次提交mesh
        /// </summary>
        public void DisableRendering()
        {
            m_AllowRendering = false;
        }


        protected abstract MeshUpdateFlags GetMeshUpdateFlags();

        protected abstract Mesh CreateNewMesh();
    }
}