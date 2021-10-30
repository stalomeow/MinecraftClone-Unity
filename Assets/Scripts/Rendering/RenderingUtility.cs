using System.Runtime.CompilerServices;
using Minecraft.Configurations;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using static Minecraft.WorldConsts;
using static Unity.Mathematics.math;

namespace Minecraft.Rendering
{
    [XLua.LuaCallCSharp]
    public static class RenderingUtility
    {
        public const int FrustumPlaneCount = 6;
        public const int SectionHeight = 16;
        public const int SectionCountInChunk = ChunkHeight / SectionHeight;

        public static readonly MeshTopology SectionMeshTopology = MeshTopology.Triangles;
        public static readonly MeshUpdateFlags SectionMeshUpdateFlags = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSectionIndex(float y)
        {
            return Mathf.FloorToInt(y / SectionHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSectionY(int sectionIndex)
        {
            return sectionIndex * SectionHeight;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int GetSection(float x, float y, float z)
        {
            int chunkX = Mathf.FloorToInt(x / ChunkWidth) * ChunkWidth;
            int chunkZ = Mathf.FloorToInt(z / ChunkWidth) * ChunkWidth;
            int sectionY = Mathf.FloorToInt(y / SectionHeight) * SectionHeight;
            return new Vector3Int(chunkX, sectionY, chunkZ);
        }

        public static void CalculateFrustumPlanes(Camera camera, Transform transform, NativeArray<float4> planes)
        {
            Matrix4x4 matrix = transform.localToWorldMatrix;
            Minecraft.Rendering.Jobs.CalculateFrustumPlaneJob job = new Jobs.CalculateFrustumPlaneJob
            {
                Near = camera.nearClipPlane,
                Far = camera.farClipPlane,
                FOV = camera.fieldOfView,
                Aspect = camera.aspect,
                Camera = float3x4(matrix.m00, matrix.m01, matrix.m02, matrix.m03, matrix.m10, matrix.m11, matrix.m12, matrix.m13, matrix.m20, matrix.m21, matrix.m22, matrix.m23),
                Planes = planes
            };
            job.Run();
        }

        public static void AddSection<TIndex>(this BlockMeshBuilder<TIndex> builder, Vector3Int section, Chunk3x3Accessor accessor) where TIndex : unmanaged
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    int maxY = accessor.GetTopVisibleBlockY(x, z);

                    for (int y = 0; y < SectionHeight; y++)
                    {
                        int worldY = y + section.y;

                        if (worldY > maxY)
                        {
                            break;
                        }

                        BlockData block = accessor.GetBlock(x, worldY, z);

                        if (block.EntityConversion != BlockEntityConversion.Initial)
                        {
                            Vector3Int pos = new Vector3Int(x, worldY, z);
                            Vector3Int offset = new Vector3Int(0, -section.y, 0);
                            builder.AddBlock(pos, offset, block, accessor);
                        }
                    }
                }
            }
        }
    }
}
