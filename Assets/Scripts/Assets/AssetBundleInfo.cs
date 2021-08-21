using System;
using Minecraft.Lua;

namespace Minecraft.Assets
{
    [Serializable]
    public sealed class AssetBundleInfo : ILuaCallCSharp
    {
        public string FileName;
        public string[] Assets;
        public string[] Dependencies;
    }
}