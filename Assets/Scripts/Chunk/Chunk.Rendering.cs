using Minecraft.BlocksData;
using Minecraft.Buffers;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public sealed partial class Chunk
    {
        public void RenderChunk(Material solidMaterial, Material liquidMaterial, Camera camera, MaterialPropertyBlock solidProp, MaterialPropertyBlock liquidProp)
        {
            if (!m_HasBuildedMesh)
                return;

            Vector3 pos = new Vector3(PositionX, 0, PositionZ);

            for (int i = 0; i < SectionCountInChunk; i++)
            {
                Mesh mesh = m_SolidMeshes[i];

                if (mesh)
                {
                    Graphics.DrawMesh(mesh, pos, Quaternion.identity, solidMaterial, BlockLayer, camera, 0, solidProp, false, false, false);
                }

                mesh = m_LiquidMeshes[i];

                if (mesh)
                {
                    Graphics.DrawMesh(mesh, pos, Quaternion.identity, liquidMaterial, BlockLayer, camera, 0, liquidProp, false, false, false);
                }

                pos.y += SectionHeight;
            }
        }

        public void TryBuildingMeshAsync()
        {
            if (m_IsBuildingMesh)
                return;

            m_IsBuildingMesh = true;

            try
            {
                if (!ThreadPool.QueueUserWorkItem(Build, WorldManager.Active.SyncContext))
                {
                    m_IsBuildingMesh = false;
                }
            }
            catch
            {
                m_IsBuildingMesh = false;
                throw;
            }
        }

        private void Build(object arg)
        {
            int data = m_DirtyData;
            Interlocked.CompareExchange(ref m_DirtyData, 0, data);

            ushort indices = (ushort)((data >> 8) & 0xFFFF);
            MeshDirtyFlags flags = (MeshDirtyFlags)(data & 0xFF);

            bool updateSolid = (flags & MeshDirtyFlags.SolidMesh) == MeshDirtyFlags.SolidMesh;
            bool updateLiquid = (flags & MeshDirtyFlags.LiquidMesh) == MeshDirtyFlags.LiquidMesh;

            try
            {
                if (updateSolid || updateLiquid)
                {
                    if (m_ShouldWaitForNeighborChunksLoaded)
                    {
                        m_ShouldWaitForNeighborChunksLoaded = false;

                        WorldManager.Active.ChunkManager.WaitForAllNeighborChunksLoaded(this);
                    }

                    SynchronizationContext context = arg as SynchronizationContext;

                    for (int i = SectionCountInChunk - 1; i > -1; i--)
                    {
                        if ((indices & (1u << i)) == 0)
                            continue;

                        if (updateSolid)
                        {
                            m_MeshDataBuffer.BeginRewriting(i);
                            BuildSolidMeshData(m_MeshDataBuffer);
                            m_MeshDataBuffer.ApplyToMesh(m_SolidMeshes, context);

                            m_HasBuildedMesh = true;
                        }

                        if (updateLiquid)
                        {
                            m_MeshDataBuffer.BeginRewriting(i);
                            BuildLiquidMeshData(m_MeshDataBuffer);
                            m_MeshDataBuffer.ApplyToMesh(m_LiquidMeshes, context);
                        }
                    }
                }
            }
#if !UNITY_EDITOR
            catch
            {
            }
#endif
            finally
            {
                m_IsBuildingMesh = false;
            }
        }

        private void BuildSolidMeshData(ChunkMeshDataBuffer buffer)
        {
            WorldManager world = WorldManager.Active;

            Profiler.BeginSample("Build Solid Mesh");

            try
            {
                if (m_Data.GetRenderableSolidCount(buffer.SectionIndex) <= 0)
                    return;

                int baseY = buffer.SectionIndex * SectionHeight;

                for (int dx = 0; dx < ChunkWidth; dx++)
                {
                    for (int dz = 0; dz < ChunkWidth; dz++)
                    {
                        int rx = PositionX + dx;
                        int rz = PositionZ + dz;

                        int maxY = m_Data.GetTopNonAirIndex(dx, dz);
                        
                        for (int dy = 0; dy < SectionHeight; dy++)
                        {
                            int ry = baseY + dy;

                            if (ry > maxY)
                                break;

                            BlockType type = m_Data.GetBlockType(dx, ry, dz);
                            Block block = world.DataManager.GetBlockByType(type);

                            if (block.VertexType == BlockVertexType.None || block.HasAnyFlag(BlockFlags.Liquid))
                                continue;

                            switch (block.VertexType)
                            {
                                case BlockVertexType.Cube:
                                    {
                                        float light = block.LightValue * OverMaxLight;

                                        if (world.IsBlockTransparent(rx + 1, ry, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx + 1, ry, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;

                                            buffer.AddCubeTriangles();
                                            buffer.AddCubeVertexPX(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block);
                                        }

                                        if (world.IsBlockTransparent(rx - 1, ry, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx - 1, ry, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            buffer.AddCubeTriangles();
                                            buffer.AddCubeVertexNX(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block);
                                        }

                                        if (world.IsBlockTransparent(rx, ry + 1, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry + 1, rz) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            buffer.AddCubeTriangles();
                                            buffer.AddCubeVertexPY(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block);
                                        }

                                        if (ry > 0 && world.IsBlockTransparent(rx, ry - 1, rz))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry - 1, rz) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;

                                            buffer.AddCubeTriangles();
                                            buffer.AddCubeVertexNY(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block);
                                        }

                                        if (world.IsBlockTransparent(rx, ry, rz + 1))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry - 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx - 1, ry, rz + 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry, rz + 1) + world.GetFinalLightLevel(rx, ry + 1, rz + 1) + world.GetFinalLightLevel(rx + 1, ry, rz + 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz + 1)) * 0.25f * OverMaxLight;

                                            buffer.AddCubeTriangles();
                                            buffer.AddCubeVertexPZ(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block);
                                        }

                                        if (world.IsBlockTransparent(rx, ry, rz - 1))
                                        {
                                            float lb = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rb = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry - 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry - 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float rt = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx + 1, ry, rz - 1) + world.GetFinalLightLevel(rx + 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;
                                            float lt = (world.GetFinalLightLevel(rx, ry, rz - 1) + world.GetFinalLightLevel(rx, ry + 1, rz - 1) + world.GetFinalLightLevel(rx - 1, ry, rz - 1) + world.GetFinalLightLevel(rx - 1, ry + 1, rz - 1)) * 0.25f * OverMaxLight;

                                            buffer.AddCubeTriangles();
                                            buffer.AddCubeVertexNZ(dx, dy, dz, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block);
                                        }
                                    }
                                    break;
                                case BlockVertexType.PerpendicularQuads:
                                    {
                                        float light = GetFinalLightLevelPrivate(dx, ry, dz) * OverMaxLight;

                                        buffer.AddPerpendicularQuadsTriangles();
                                        buffer.AddPerpendicularQuadsVertexFirst(dx, dy, dz, light, block);

                                        buffer.AddPerpendicularQuadsTriangles();
                                        buffer.AddPerpendicularQuadsVertexSecond(dx, dy, dz, light, block);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
#if !UNITY_EDITOR
            catch
            {
                
            }
#endif
            finally
            {
                Profiler.EndSample();
            }
        }

        private void BuildLiquidMeshData(ChunkMeshDataBuffer buffer)
        {
            WorldManager world = WorldManager.Active;

            Profiler.BeginSample("Build Liquid Mesh");

            try
            {
                if (m_Data.GetRenderableLiquidCount(buffer.SectionIndex) <= 0)
                    return;

                int baseY = buffer.SectionIndex * SectionHeight;

                for (int dx = 0; dx < ChunkWidth; dx++)
                {
                    for (int dz = 0; dz < ChunkWidth; dz++)
                    {
                        int rx = PositionX + dx;
                        int rz = PositionZ + dz;

                        int maxY = m_Data.GetTopNonAirIndex(dx, dz);
                        
                        for (int dy = 0; dy < SectionHeight; dy++)
                        {
                            int ry = baseY + dy;

                            if (ry > maxY)
                                break;

                            //if (ry == 0)
                            //    continue;

                            BlockType type = m_Data.GetBlockType(dx, ry, dz);
                            Block block = world.DataManager.GetBlockByType(type);

                            if (block.VertexType != BlockVertexType.Cube || !block.HasAllFlags(BlockFlags.Liquid))
                                continue;

                            float light = GetFinalLightLevelPrivate(dx, ry, dz) * OverMaxLight;

                            if (world.IsBlockTransparentAndNotWater(rx + 1, ry, rz))
                            {
                                buffer.AddCubeTriangles();
                                buffer.AddCubeVertexPX(dx, dy, dz, light, light, light, light, block);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx - 1, ry, rz))
                            {
                                buffer.AddCubeTriangles();
                                buffer.AddCubeVertexNX(dx, dy, dz, light, light, light, light, block);
                            }

                            //if (world.IsBlockTransparentAndNotWater(rx, ry + 1, rz))
                            if (world.GetBlockType(rx, ry + 1, rz) != BlockType.Water)
                            {
                                buffer.AddCubeTriangles();
                                buffer.AddCubeVertexPY(dx, dy, dz, light, light, light, light, block);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry - 1, rz))
                            {
                                buffer.AddCubeTriangles();
                                buffer.AddCubeVertexNY(dx, dy, dz, light, light, light, light, block);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry, rz + 1))
                            {
                                buffer.AddCubeTriangles();
                                buffer.AddCubeVertexPZ(dx, dy, dz, light, light, light, light, block);
                            }

                            if (world.IsBlockTransparentAndNotWater(rx, ry, rz - 1))
                            {
                                buffer.AddCubeTriangles();
                                buffer.AddCubeVertexNZ(dx, dy, dz, light, light, light, light, block);
                            }
                        }
                    }
                }
            }
#if !UNITY_EDITOR
            catch
            {

            }
#endif
            finally
            {
                Profiler.EndSample();
            }
        }
    }
}