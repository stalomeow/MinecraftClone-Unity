using Minecraft.BlocksData;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Minecraft.Buffers
{
    public static class MeshDataBufferUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeTriangles(this MeshDataBuffer buffer)
        {
            buffer.AddTriangle(0, 3, 2);
            buffer.AddTriangle(2, 1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexPX(this MeshDataBuffer buffer, int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block)
        {
            block.GetPositiveXUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            buffer.AddVertex(new Vector3(x + 1, y, z), Vector3.right, lb, lightLB);
            buffer.AddVertex(new Vector3(x + 1, y, z + 1), Vector3.right, rb, lightRB);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z + 1), Vector3.right, rt, lightRT);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z), Vector3.right, lt, lightLT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexPY(this MeshDataBuffer buffer, int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block)
        {
            block.GetPositiveYUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            buffer.AddVertex(new Vector3(x, y + 1, z), Vector3.up, lb, lightLB);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z), Vector3.up, rb, lightRB);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z + 1), Vector3.up, rt, lightRT);
            buffer.AddVertex(new Vector3(x, y + 1, z + 1), Vector3.up, lt, lightLT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexPZ(this MeshDataBuffer buffer, int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block)
        {
            block.GetPositiveZUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            buffer.AddVertex(new Vector3(x + 1, y, z + 1), Vector3.forward, lb, lightLB);
            buffer.AddVertex(new Vector3(x, y, z + 1), Vector3.forward, rb, lightRB);
            buffer.AddVertex(new Vector3(x, y + 1, z + 1), Vector3.forward, rt, lightRT);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z + 1), Vector3.forward, lt, lightLT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexNX(this MeshDataBuffer buffer, int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block)
        {
            block.GetNegativeXUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            buffer.AddVertex(new Vector3(x, y, z + 1), Vector3.left, lb, lightLB);
            buffer.AddVertex(new Vector3(x, y, z), Vector3.left, rb, lightRB);
            buffer.AddVertex(new Vector3(x, y + 1, z), Vector3.left, rt, lightRT);
            buffer.AddVertex(new Vector3(x, y + 1, z + 1), Vector3.left, lt, lightLT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexNY(this MeshDataBuffer buffer, int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block)
        {
            block.GetNegativeYUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            buffer.AddVertex(new Vector3(x + 1, y, z), Vector3.down, lb, lightLB);
            buffer.AddVertex(new Vector3(x, y, z), Vector3.down, rb, lightRB);
            buffer.AddVertex(new Vector3(x, y, z + 1), Vector3.down, rt, lightRT);
            buffer.AddVertex(new Vector3(x + 1, y, z + 1), Vector3.down, lt, lightLT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCubeVertexNZ(this MeshDataBuffer buffer, int x, int y, int z, float lightLB, float lightRB, float lightRT, float lightLT, Block block)
        {
            block.GetNegativeZUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            buffer.AddVertex(new Vector3(x, y, z), Vector3.back, lb, lightLB);
            buffer.AddVertex(new Vector3(x + 1, y, z), Vector3.back, rb, lightRB);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z), Vector3.back, rt, lightRT);
            buffer.AddVertex(new Vector3(x, y + 1, z), Vector3.back, lt, lightLT);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPerpendicularQuadsTriangles(this MeshDataBuffer buffer)
        {
            buffer.AddTriangle(0, 3, 2);
            buffer.AddTriangle(2, 1, 0);

            buffer.AddTriangle(3, 0, 1);
            buffer.AddTriangle(1, 2, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPerpendicularQuadsVertexFirst(this MeshDataBuffer buffer, int x, int y, int z, float light, Block block)
        {
            block.GetMainUVForPerpendicularQuadsVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            Vector3 normal = new Vector3(-0.70711f, 0, -0.70711f);

            buffer.AddVertex(new Vector3(x, y, z + 1), normal, lb, light);
            buffer.AddVertex(new Vector3(x + 1, y, z), normal, rb, light);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z), normal, rt, light);
            buffer.AddVertex(new Vector3(x, y + 1, z + 1), normal, lt, light);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddPerpendicularQuadsVertexSecond(this MeshDataBuffer buffer, int x, int y, int z, float light, Block block)
        {
            block.GetMainUVForPerpendicularQuadsVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt);

            Vector3 normal = new Vector3(-0.70711f, 0, 0.70711f);

            buffer.AddVertex(new Vector3(x, y, z), normal, lb, light);
            buffer.AddVertex(new Vector3(x + 1, y, z + 1), normal, rb, light);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z + 1), normal, rt, light);
            buffer.AddVertex(new Vector3(x, y + 1, z), normal, lt, light);
        }
    }
}