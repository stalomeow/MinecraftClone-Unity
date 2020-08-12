using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Minecraft.BlocksData
{
    public static class BlockVertexHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct VertexData
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


        public static readonly VertexAttributeDescriptor[] VertexLayout = new VertexAttributeDescriptor[]
        {
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 3)
        };


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexTriangles(List<int> triangles, int verticesCount)
        {
            triangles.Add(0 + verticesCount);
            triangles.Add(3 + verticesCount);
            triangles.Add(2 + verticesCount);

            triangles.Add(2 + verticesCount);
            triangles.Add(1 + verticesCount);
            triangles.Add(0 + verticesCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexDataPX(int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block, List<VertexData> vertices)
        {
            block.GetPositiveXUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z,

                NORMAL_X = 1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = lightLB
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z + 1,

                NORMAL_X = 1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = lightRB
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = 1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = lightRT
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z,

                NORMAL_X = 1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = lightLT
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexDataPY(int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block, List<VertexData> vertices)
        {
            block.GetPositiveYUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = 1,
                NORMAL_Z = 0,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = lightLB
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = 1,
                NORMAL_Z = 0,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = lightRB
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = 1,
                NORMAL_Z = 0,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = lightRT
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = 1,
                NORMAL_Z = 0,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = lightLT
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexDataPZ(int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block, List<VertexData> vertices)
        {
            block.GetPositiveZUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = 1,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = lightLB
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = 1,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = lightRB
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = 1,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = lightRT
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = 1,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = lightLT
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexDataNX(int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block, List<VertexData> vertices)
        {
            block.GetNegativeXUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z + 1,

                NORMAL_X = -1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = lightLB
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z,

                NORMAL_X = -1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = lightRB
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z,

                NORMAL_X = -1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = lightRT
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = -1,
                NORMAL_Y = 0,
                NORMAL_Z = 0,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = lightLT
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexDataNY(int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block, List<VertexData> vertices)
        {
            block.GetNegativeYUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = -1,
                NORMAL_Z = 0,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = lightLB
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = -1,
                NORMAL_Z = 0,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = lightRB
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = -1,
                NORMAL_Z = 0,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = lightRT
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z + 1,

                NORMAL_X = 0,
                NORMAL_Y = -1,
                NORMAL_Z = 0,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = lightLT
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexDataNZ(int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block, List<VertexData> vertices)
        {
            block.GetNegativeZUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = -1,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = lightLB
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = -1,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = lightRB
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = -1,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = lightRT
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z,

                NORMAL_X = 0,
                NORMAL_Y = 0,
                NORMAL_Z = -1,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = lightLT
            });
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPerpendicularQuadsTriangles(List<int> triangles, int verticesCount)
        {
            triangles.Add(0 + verticesCount);
            triangles.Add(3 + verticesCount);
            triangles.Add(2 + verticesCount);

            triangles.Add(2 + verticesCount);
            triangles.Add(1 + verticesCount);
            triangles.Add(0 + verticesCount);

            triangles.Add(3 + verticesCount);
            triangles.Add(0 + verticesCount);
            triangles.Add(1 + verticesCount);

            triangles.Add(1 + verticesCount);
            triangles.Add(2 + verticesCount);
            triangles.Add(3 + verticesCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPerpendicularQuadsVertexDataFirstQuad(int x, int y, int z, float light, Block block, List<VertexData> vertices)
        {
            block.GetMainUVForPerpendicularQuadsVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z + 1,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = -0.70711f,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = light
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = -0.70711f,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = light
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = -0.70711f,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = light
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = -0.70711f,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = light
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPerpendicularQuadsVertexDataSecondQuad(int x, int y, int z, float light, Block block, List<VertexData> vertices)
        {
            block.GetMainUVForPerpendicularQuadsVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            vertices.Add(new VertexData
            {
                X = x,
                Y = y,
                Z = z,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = 0.70711f,

                UV_X = lb.x,
                UV_Y = lb.y,
                LIGHT = light
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y,
                Z = z + 1,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = 0.70711f,

                UV_X = rb.x,
                UV_Y = rb.y,
                LIGHT = light
            });

            vertices.Add(new VertexData
            {
                X = x + 1,
                Y = y + 1,
                Z = z + 1,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = 0.70711f,

                UV_X = rt.x,
                UV_Y = rt.y,
                LIGHT = light
            });

            vertices.Add(new VertexData
            {
                X = x,
                Y = y + 1,
                Z = z,

                NORMAL_X = -0.70711f,
                NORMAL_Y = 0,
                NORMAL_Z = 0.70711f,

                UV_X = lt.x,
                UV_Y = lt.y,
                LIGHT = light
            });
        }
    }
}