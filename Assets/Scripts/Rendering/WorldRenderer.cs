using UnityEngine;

namespace Minecraft.Rendering
{
    public abstract class WorldRenderer : ScriptableObject
    {
        [SerializeField] private Material m_Material;

        protected Material Material => m_Material;

        protected MaterialPropertyBlock MaterialProperty { get; private set; }


        protected WorldRenderer() { }


        public virtual void Initialize(BlockTextureTable texTable)
        {
            MaterialProperty = new MaterialPropertyBlock();
            MaterialProperty.SetTexture("_TexArr", texTable.TextureArray);
        }

        public abstract bool Render(AbstractMesh mesh, Camera camera);
    }
}