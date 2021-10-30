using System;
using Minecraft.Configurations;
using Minecraft.PhysicSystem;
using UnityEngine;

namespace Minecraft.Rendering
{
    public class BlockMeshBuilder<TIndex> : MeshBuilder<BlockMeshVertexData, TIndex> where TIndex : unmanaged
    {
        public bool WriteBlockWSPosToVertexData { get; set; }

        public bool EnableAmbientOcclusion { get; set; }

        public bool EnableFaceClipping { get; set; }

        public bool AggressiveBlockFaceClipping { get; set; }


        public BlockMeshBuilder(int subMeshCount) : base(BlockMeshVertexData.VertexAttributes, subMeshCount) { }


        public void AddBlock(Vector3Int pos, Vector3Int renderOffset, BlockData block, IWorldRAccessor accessor)
        {
            Quaternion rotation = accessor.GetBlockRotation(pos.x, pos.y, pos.z, Quaternion.identity);
            BlockMesh mesh = accessor.World.BlockDataTable.GetMesh(block.Mesh.Value);

            for (int i = 0; i < mesh.Faces.Length; i++)
            {
                BlockMesh.FaceData face = mesh.Faces[i];
                BlockFace faceDir = RotateFace(face.Face, rotation);

                if (EnableFaceClipping)
                {
                    // 没有撑满一格的方块所有的面都渲染
                    Vector3 size = mesh.BoundingBox.Size;
                    bool neverClip = face.NeverClip | size.x < 1 | size.y < 1 | size.z < 1;

                    if (!neverClip && ClipFace(pos, block, faceDir, accessor))
                    {
                        continue;
                    }
                }

                int?[] texIndices = block.Textures[i];

                // !!! must add indices first
                for (int j = 0; j < face.Indices.Length; j++)
                {
                    AddIndex(face.Indices[j], block.Material.Value);
                }

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    BlockVertexData vertex = face.Vertices[j];
                    vertex.Position = MathUtility.RotatePoint(vertex.Position, rotation, mesh.Pivot);

                    float emission = block.GetEmissionValue();
                    Vector2 ambient = LightingUtility.AmbientOcclusion(pos, faceDir, vertex.CornerInFace, accessor, !EnableAmbientOcclusion);

                    Vector3 posWS = WriteBlockWSPosToVertexData ? (pos + accessor.WorldSpaceOrigin) : Vector3.down;

                    AddVertex(new BlockMeshVertexData
                    {
                        PositionOS = vertex.Position + pos + renderOffset,
                        UV = vertex.UV,
                        TexIndices = new Vector3Int(texIndices[0].Value, texIndices[1].Value, texIndices[2].Value),
                        Lights = new Vector3(emission, ambient.x, ambient.y),
                        BlockPositionWS = posWS
                    });
                }
            }
        }

        protected bool ClipFace(Vector3Int pos, BlockData block, BlockFace face, IWorldRAccessor accessor)
        {
            switch (face)
            {
                case BlockFace.PositiveX: pos.x++; break;
                case BlockFace.PositiveY: pos.y++; break;
                case BlockFace.PositiveZ: pos.z++; break;
                case BlockFace.NegativeX: pos.x--; break;
                case BlockFace.NegativeY: pos.y--; break;
                case BlockFace.NegativeZ: pos.z--; break;
                default: throw new NotSupportedException("Unknown BlockFace.");
            }

            BlockData neighbor = accessor.GetBlock(pos.x, pos.y, pos.z);

            if (neighbor == null)
            {
                return AggressiveBlockFaceClipping;
            }

            BlockMesh mesh = accessor.World.BlockDataTable.GetMesh(neighbor.Mesh.Value);
            Vector3 size = mesh.BoundingBox.Size;

            if (size.x < 1 || size.y < 1 || size.z < 1)
            {
                return false;
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

        protected override Vector3 GetPositionOS(in BlockMeshVertexData vertex)
        {
            return vertex.PositionOS;
        }

        protected static BlockFace RotateFace(BlockFace face, Quaternion rotation)
        {
            if (rotation == Quaternion.identity)
            {
                return face;
            }

            Vector3 normal = face switch
            {
                BlockFace.PositiveX => Vector3.right,
                BlockFace.PositiveY => Vector3.up,
                BlockFace.PositiveZ => Vector3.forward,
                BlockFace.NegativeX => Vector3.left,
                BlockFace.NegativeY => Vector3.down,
                BlockFace.NegativeZ => Vector3.back,
                _ => throw new NotSupportedException("Unknown BlockFace.")
            };

            normal = Vector3.Normalize(rotation * normal).RoundToInt();

            return normal switch
            {
                { x: 1.0f, y: 0.0f, z: 0.0f } => BlockFace.PositiveX,
                { x: 0.0f, y: 1.0f, z: 0.0f } => BlockFace.PositiveY,
                { x: 0.0f, y: 0.0f, z: 1.0f } => BlockFace.PositiveZ,
                { x: -1.0f, y: 0.0f, z: 0.0f } => BlockFace.NegativeX,
                { x: 0.0f, y: -1.0f, z: 0.0f } => BlockFace.NegativeY,
                { x: 0.0f, y: 0.0f, z: -1.0f } => BlockFace.NegativeZ,
                _ => throw new InvalidOperationException($"Invalid Rotation: {rotation.eulerAngles}.")
            };
        }

        public static BlockMeshBuilder<TIndex> CreateBlockEntityMeshBuilder(bool ambientOcclusion)
        {
            return new BlockMeshBuilder<TIndex>(1)
            {
                WriteBlockWSPosToVertexData = false,
                EnableAmbientOcclusion = ambientOcclusion,
                EnableFaceClipping = false,
                AggressiveBlockFaceClipping = false
            };
        }

        public static BlockMeshBuilder<TIndex> CreateSectionMeshBuilder(int subMeshCount, bool ambientOcclusion, bool clipFace, bool aggressiveBlockFaceClipping)
        {
            return new BlockMeshBuilder<TIndex>(subMeshCount)
            {
                WriteBlockWSPosToVertexData = true,
                EnableAmbientOcclusion = ambientOcclusion,
                EnableFaceClipping = clipFace,
                AggressiveBlockFaceClipping = aggressiveBlockFaceClipping
            };
        }
    }
}
