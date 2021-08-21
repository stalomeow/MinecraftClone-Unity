using UnityEngine;
using UnityEngine.Rendering;
using XLua;

namespace Minecraft.Entities
{
    [LuaCallCSharp]
    public interface IRenderableEntity : IAABBEntity
    {
        bool EnableRendering { get; set; }

        Mesh SharedMesh { get; }

        Material SharedMaterial { get; }

        MaterialPropertyBlock MaterialProperty { get; }

        void Render(int layer, Camera camera, ShadowCastingMode castShadows, bool receiveShadows);
    }
}
