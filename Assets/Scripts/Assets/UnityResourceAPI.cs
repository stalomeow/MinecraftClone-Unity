using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minecraft.Assets
{
    internal class UnityResourceAPI : ResourcesAPI
    {
        // TODO

        protected override UnityEngine.Object[] FindObjectsOfTypeAll(Type systemTypeInstance)
        {
            return base.FindObjectsOfTypeAll(systemTypeInstance);
        }

        protected override Shader FindShaderByName(string name)
        {
            return base.FindShaderByName(name);
        }

        protected override UnityEngine.Object Load(string path, Type systemTypeInstance)
        {
            return base.Load(path, systemTypeInstance);
        }

        protected override UnityEngine.Object[] LoadAll(string path, Type systemTypeInstance)
        {
            return base.LoadAll(path, systemTypeInstance);
        }

        protected override ResourceRequest LoadAsync(string path, Type systemTypeInstance)
        {
            return base.LoadAsync(path, systemTypeInstance);
        }

        protected override void UnloadAsset(UnityEngine.Object assetToUnload)
        {
            base.UnloadAsset(assetToUnload);
        }
    }
}
