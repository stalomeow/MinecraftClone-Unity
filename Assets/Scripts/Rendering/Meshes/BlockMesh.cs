using UnityEngine;
using UnityEngine.Rendering;

namespace Minecraft.Rendering
{
    /// <summary>
    /// 表示一个方块的mesh
    /// </summary>
    public class BlockMesh : AbstractMesh
    {
        public BlockMesh() : base(8, 12) { }

        protected override Mesh CreateNewMesh()
        {
            return new Mesh
            {
                indexFormat = SystemInfo.supports32bitsIndexBuffer ? IndexFormat.UInt32 : IndexFormat.UInt16,
                bounds = new Bounds(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one)
            };
        }

        protected override MeshUpdateFlags GetMeshUpdateFlags()
        {
            return MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontResetBoneBounds;
        }
    }
}