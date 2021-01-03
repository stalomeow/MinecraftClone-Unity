using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using static Minecraft.WorldConsts;

namespace Minecraft.Rendering
{
    public sealed class ChunkMeshSlice : AbstractMesh, IEquatable<ChunkMeshSlice>
    {
        private static readonly Bounds s_Bounds = new Bounds(
            new Vector3(ChunkWidth >> 1, SectionHeight >> 1, ChunkWidth >> 1),
            new Vector3(ChunkWidth, SectionHeight, ChunkWidth)
        );

        private static readonly IndexFormat s_IndexFormat = 
            SystemInfo.supports32bitsIndexBuffer ? IndexFormat.UInt32 : IndexFormat.UInt16;


        private Chunk m_Chunk;

        public int SectionIndex { get; private set; }

        public MeshSliceType SliceType { get; private set; }

        public int PositionX => m_Chunk.PositionX;

        public int PositionZ => m_Chunk.PositionZ;

        
        public ChunkMeshSlice() : base(30, 30) { }

        public void Initialize(Chunk chunk, int sectionIndex, MeshSliceType sliceType)
        {
            m_Chunk = chunk;
            SectionIndex = sectionIndex;
            SliceType = sliceType;
        }


        public bool UpdateGeometry(BlockTextureTable texTable)
        {
            if (!m_Chunk.IsSectionVisible(SectionIndex, SliceType))
            {
                DisableRendering();
                return false;
            }

            Profiler.BeginSample("ChunkMeshSlice.UpdateGeometry");

            int baseY = SectionIndex * SectionHeight;
            Clear(false);

            try
            {
                for (int localX = 0; localX < ChunkWidth; localX++)
                {
                    for (int localZ = 0; localZ < ChunkWidth; localZ++)
                    {
                        int maxY = m_Chunk.GetTopNonAirBlockIndex(localX, localZ);

                        for (int localY = 0; localY < SectionHeight; localY++)
                        {
                            int y = baseY + localY;

                            if (y > maxY)
                            {
                                break;
                            }

                            byte id = m_Chunk.GetBlockId(localX, y, localZ);
                            Block block = m_Chunk.World.GetBlock(id);
                            BlockMeshWriter meshWriter = block.MeshWriter;

                            if (meshWriter.IsEmpty)
                            {
                                continue;
                            }

                            switch (SliceType)
                            {
                                case MeshSliceType.Solid when !block.IsFluid:
                                    meshWriter.Write(this, localX, localY, localZ, block, texTable, EvalateSolidLight);
                                    break;

                                case MeshSliceType.Fluid when block.IsFluid:
                                    meshWriter.Write(this, localX, localY, localZ, block, texTable, EvalateFluidLight);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Profiler.EndSample();
            }

            return true;
        }

        private bool EvalateSolidLight(int x, int y, int z, Block block, BlockDirection? dir, out VertexLightingData lighting)
        {
            y += SectionIndex * SectionHeight;

            if (dir == null)
            {
                lighting = new VertexLightingData(m_Chunk.GetLightValue(x, y, z));
                return true;
            }

            m_Chunk.LocalToWorldPosition(ref x, ref z);

            if (!ShouldRenderBlockFace(x, y, z, block, dir.Value))
            {
                lighting = default;
                return false;
            }

            float lb, rb, rt, lt;

            switch (dir)
            {
                case BlockDirection.PositiveX:
                    lb = GetAverageLights((x + 1, y, z), (x + 1, y - 1, z), (x + 1, y, z - 1), (x + 1, y - 1, z - 1));
                    rb = GetAverageLights((x + 1, y, z), (x + 1, y - 1, z), (x + 1, y, z + 1), (x + 1, y - 1, z + 1));
                    rt = GetAverageLights((x + 1, y, z), (x + 1, y + 1, z), (x + 1, y, z + 1), (x + 1, y + 1, z + 1));
                    lt = GetAverageLights((x + 1, y, z), (x + 1, y + 1, z), (x + 1, y, z - 1), (x + 1, y + 1, z - 1));
                    break;

                case BlockDirection.PositiveY:
                    lb = GetAverageLights((x, y + 1, z), (x, y + 1, z - 1), (x - 1, y + 1, z), (x - 1, y + 1, z - 1));
                    rb = GetAverageLights((x, y + 1, z), (x, y + 1, z - 1), (x + 1, y + 1, z), (x + 1, y + 1, z - 1));
                    rt = GetAverageLights((x, y + 1, z), (x, y + 1, z + 1), (x + 1, y + 1, z), (x + 1, y + 1, z + 1));
                    lt = GetAverageLights((x, y + 1, z), (x, y + 1, z + 1), (x - 1, y + 1, z), (x - 1, y + 1, z + 1));
                    break;

                case BlockDirection.PositiveZ:
                    lb = GetAverageLights((x, y, z + 1), (x, y - 1, z + 1), (x + 1, y, z + 1), (x + 1, y - 1, z + 1));
                    rb = GetAverageLights((x, y, z + 1), (x, y - 1, z + 1), (x - 1, y, z + 1), (x - 1, y - 1, z + 1));
                    rt = GetAverageLights((x, y, z + 1), (x, y + 1, z + 1), (x - 1, y, z + 1), (x - 1, y + 1, z + 1));
                    lt = GetAverageLights((x, y, z + 1), (x, y + 1, z + 1), (x + 1, y, z + 1), (x + 1, y + 1, z + 1));
                    break;

                case BlockDirection.NegativeX:
                    lb = GetAverageLights((x - 1, y, z), (x - 1, y - 1, z), (x - 1, y, z + 1), (x - 1, y - 1, z + 1));
                    rb = GetAverageLights((x - 1, y, z), (x - 1, y - 1, z), (x - 1, y, z - 1), (x - 1, y - 1, z - 1));
                    rt = GetAverageLights((x - 1, y, z), (x - 1, y + 1, z), (x - 1, y, z - 1), (x - 1, y + 1, z - 1));
                    lt = GetAverageLights((x - 1, y, z), (x - 1, y + 1, z), (x - 1, y, z + 1), (x - 1, y + 1, z + 1));
                    break;

                case BlockDirection.NegativeY:
                    lb = GetAverageLights((x, y - 1, z), (x, y - 1, z - 1), (x + 1, y - 1, z), (x + 1, y - 1, z - 1));
                    rb = GetAverageLights((x, y - 1, z), (x, y - 1, z - 1), (x - 1, y - 1, z), (x - 1, y - 1, z - 1));
                    rt = GetAverageLights((x, y - 1, z), (x, y - 1, z + 1), (x - 1, y - 1, z), (x - 1, y - 1, z + 1));
                    lt = GetAverageLights((x, y - 1, z), (x, y - 1, z + 1), (x + 1, y - 1, z), (x + 1, y - 1, z + 1));
                    break;

                case BlockDirection.NegativeZ:
                    lb = GetAverageLights((x, y, z - 1), (x, y - 1, z - 1), (x - 1, y, z - 1), (x - 1, y - 1, z - 1));
                    rb = GetAverageLights((x, y, z - 1), (x, y - 1, z - 1), (x + 1, y, z - 1), (x + 1, y - 1, z - 1));
                    rt = GetAverageLights((x, y, z - 1), (x, y + 1, z - 1), (x + 1, y, z - 1), (x + 1, y + 1, z - 1));
                    lt = GetAverageLights((x, y, z - 1), (x, y + 1, z - 1), (x - 1, y, z - 1), (x - 1, y + 1, z - 1));
                    break;

                default:
                    lb = 0;
                    rb = 0;
                    rt = 0;
                    lt = 0;
                    break;
            }

            lighting = new VertexLightingData((byte)lb, (byte)rb, (byte)rt, (byte)lt);
            return true;
        }

        private float GetAverageLights((int x, int y, int z) b1, (int x, int y, int z) b2, (int x, int y, int z) b3, (int x, int y, int z) b4)
        {
            byte l1 = m_Chunk.World.GetLightValue(b1.x, b1.y, b1.z);
            byte l2 = m_Chunk.World.GetLightValue(b2.x, b2.y, b2.z);
            byte l3 = m_Chunk.World.GetLightValue(b3.x, b3.y, b3.z);
            byte l4 = m_Chunk.World.GetLightValue(b4.x, b4.y, b4.z);
            return (l1 + l2 + l3 + l4) * 0.25f;
        }

        private bool EvalateFluidLight(int x, int y, int z, Block block, BlockDirection? dir, out VertexLightingData lighting)
        {
            int localX = x;
            int localZ = z;

            y += SectionIndex * SectionHeight;
            m_Chunk.LocalToWorldPosition(ref x, ref z);

            if (ShouldRenderBlockFace(x, y, z, block, dir.Value))
            {
                lighting = new VertexLightingData(m_Chunk.GetLightValue(localX, y, localZ));
                return true;
            }

            lighting = default;
            return false;
        }

        private bool ShouldRenderBlockFace(int x, int y, int z, Block block, BlockDirection direction)
        {
            switch (direction)
            {
                case BlockDirection.PositiveX: x += 1; break;
                case BlockDirection.PositiveY: y += 1; break;
                case BlockDirection.PositiveZ: z += 1; break;
                case BlockDirection.NegativeX: x -= 1; break;
                case BlockDirection.NegativeY: y -= 1; break;
                case BlockDirection.NegativeZ: z -= 1; break;
                default: return false;
            }

            Block neighbor = m_Chunk.World.GetBlock(x, y, z, nullable: true);
            
            if (neighbor == null)
            {
                return false; // 假设是非透明方块
            }

            bool shouldRender = neighbor.LightOpacity < LightingUtility.MaxLight;

            if (block.IsFluid)
            {
                bool diffBlock = neighbor.Id != block.Id;
                return (direction == BlockDirection.PositiveY) ? diffBlock : (diffBlock && shouldRender);
            }

            return shouldRender;
        }


        protected override Mesh CreateNewMesh()
        {
            Mesh mesh = new Mesh
            {
                indexFormat = s_IndexFormat,
                bounds = s_Bounds
            };
            mesh.MarkDynamic();
            return mesh;
        }

        protected override MeshUpdateFlags GetMeshUpdateFlags()
        {
            return MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontResetBoneBounds;
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as ChunkMeshSlice);
        }

        public bool Equals(ChunkMeshSlice other)
        {
            return !(other is null)
                && m_Chunk == other.m_Chunk
                && SectionIndex == other.SectionIndex
                && SliceType == other.SliceType;
        }

        public override int GetHashCode()
        {
            var hashCode = m_Chunk.GetHashCode();
            hashCode = (hashCode * 31) + SectionIndex.GetHashCode();
            hashCode = (hashCode * 31) + SliceType.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ChunkMeshSlice left, ChunkMeshSlice right)
        {
            bool flag1 = left is null;
            bool flag2 = right is null;

            if (flag1 && flag2)
            {
                return true;
            }

            if (!flag1 && !flag2)
            {
                return left.Equals(right);
            }

            return false;
        }

        public static bool operator !=(ChunkMeshSlice left, ChunkMeshSlice right)
        {
            return !(left == right);
        }
    }
}