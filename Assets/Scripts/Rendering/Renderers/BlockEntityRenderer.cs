using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minecraft.Rendering
{
    [CreateAssetMenu(menuName = "Minecraft/Renderers/Block Entity", fileName = "Block Entity")]
    public class BlockEntityRenderer : WorldRenderer
    {
        public override bool Render(AbstractMesh mesh, Camera camera)
        {
            return false;
        }
    }
}