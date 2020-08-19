using System;

namespace Minecraft.AssetManagement
{
    [Serializable]
    public sealed class AssetBundleManifest
    {
        public const string FileName = "manifest";

        public AssetBundleMeta[] AssetBundles;
    }
}