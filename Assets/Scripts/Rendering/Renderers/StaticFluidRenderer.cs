using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft.Rendering
{
    [CreateAssetMenu(menuName = "Minecraft/Renderers/Static Fluid", fileName = "Static Fluid")]
    public class StaticFluidRenderer : WorldRenderer
    {
        public override bool Render(AbstractMesh mesh, Camera camera)
        {
            if ((mesh is ChunkMeshSlice cm) && (cm.SliceType == MeshSliceType.Fluid))
            {
                Vector3 pos = new Vector3(cm.PositionX, cm.SectionIndex * SectionHeight, cm.PositionZ);
                mesh.Render(pos, Quaternion.identity, Material, MaterialProperty, camera, BlockLayer);
                return true;
            }

            return false;
        }
    }
}