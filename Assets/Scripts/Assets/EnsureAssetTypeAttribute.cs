using System;

namespace Minecraft.Assets
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class EnsureAssetTypeAttribute : Attribute
    {
        public Type AssetType { get; }

        public EnsureAssetTypeAttribute(Type type)
        {
            AssetType = type ?? typeof(UnityEngine.Object);
        }
    }
}
