using System;
using System.Collections.Generic;
using Minecraft.Lua;

namespace Minecraft.Assets
{
    [Serializable]
    public class AssetCatalog : ILuaCallCSharp
    {
        public const string FileName = "catalog.json";

        public Dictionary<string, AssetInfo> Assets;
        public Dictionary<string, AssetBundleInfo> AssetBundles;
    }
}
