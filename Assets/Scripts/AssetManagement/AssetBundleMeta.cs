using System;

namespace Minecraft.AssetManagement
{
    [Serializable]
    public sealed class AssetBundleMeta
    {
        public string Name;
        public string FileName;
        public string[] Dependencies;
    }
}