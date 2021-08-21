using System;
using Minecraft.Lua;

namespace Minecraft.Assets
{
    [Serializable]
    public class AssetInfo : ILuaCallCSharp
    {
        public string AssetName;
        public string AssetBundleName;
    }
}
