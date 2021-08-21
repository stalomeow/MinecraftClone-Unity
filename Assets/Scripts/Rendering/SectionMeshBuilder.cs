using System;
using Minecraft.Configurations;
using Minecraft.PhysicSystem;
using UnityEngine;

namespace Minecraft.Rendering
{
    public class SectionMeshBuilder<TIndex> : MeshBuilder<SectionMeshVertexData, TIndex> where TIndex : unmanaged
    {
        public bool AggressiveBlockFaceClipping { get; set; }

        public SectionMeshBuilder(int subMeshCount, bool aggressiveBlockFaceClipping = false)
            : base(SectionMeshVertexData.VertexAttributes, subMeshCount)
        {
            AggressiveBlockFaceClipping = aggressiveBlockFaceClipping;
        }

        public void AddBlock(int x, int y, int z, int sectionY, BlockData block, Chunk3x3Accessor accessor)
        {
            if (block.EntityConversion == BlockEntityConversion.Initial)
            {
                return;
            }

            int worldY = y + sectionY;
            BlockMesh mesh = accessor.World.BlockDataTable.GetMesh(block.Mesh.Value);

            for (int i = 0; i < mesh.Faces.Length; i++)
            {
                BlockMesh.FaceData face = mesh.Faces[i];

                if (!face.NeverClip && ClipFace(x, worldY, z, block, face.Face, accessor))
                    continue;

                int?[] texIndices = block.Textures[i];

                // !!! must add indices first
                for (int j = 0; j < face.Indices.Length; j++)
                {
                    AddIndex(face.Indices[j], block.Material.Value);
                }

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    BlockVertexData vertex = face.Vertices[j];
                    float emission = block.GetEmissionValue();
                    Vector2 ambient = LightingUtility.AmbientOcclusion(x, worldY, z, face.Face, vertex.CornerInFace, accessor);

                    AddVertex(new SectionMeshVertexData
                    {
                        PositionOS = vertex.Position + new Vector3(x, y, z),
                        UV = vertex.UV,
                        TexIndices = new Vector3Int(texIndices[0].Value, texIndices[1].Value, texIndices[2].Value),
                        Lights = new Vector3(emission, ambient.x, ambient.y),
                        BlockPositionWS = new Vector3(x, worldY, z) + accessor.WorldSpaceOrigin
                    });
                }
            }
        }

        protected bool ClipFace(int x, int y, int z, BlockData block, BlockFace face, IWorldRAccessor accessor)
        {
            switch (face)
            {
                case BlockFace.PositiveX: x++; break;
                case BlockFace.PositiveY: y++; break;
                case BlockFace.PositiveZ: z++; break;
                case BlockFace.NegativeX: x--; break;
                case BlockFace.NegativeY: y--; break;
                case BlockFace.NegativeZ: z--; break;
                default: throw new NotSupportedException("Unknown BlockFace.");
            }

            BlockData neighbor = accessor.GetBlock(x, y, z);

            if (neighbor == null)
            {
                return AggressiveBlockFaceClipping;
            }

            switch (block.PhysicState)
            {
                case PhysicState.Fluid:
                    return (block == neighbor) || neighbor.IsOpaqueBlock();
                case PhysicState.Solid:
                    return neighbor.IsOpaqueBlock();
                default:
                    throw new NotSupportedException("Unknown BlockPhysicalState");
            }
        }

        protected override Vector3 GetPositionOS(in SectionMeshVertexData vertex)
        {
            return vertex.PositionOS;
        }
    }
}
