using System;
using Object = UnityEngine.Object;

namespace Minecraft.AssetManagement
{
    public interface IAssetBundle
    {
        string Name { get; }

        bool IsStreamedSceneAssetBundle { get; }

        AsyncHandler AsyncHandler { get; }

        AsyncAsset<T> LoadAsset<T>(string path) where T : Object;

        AsyncAsset<Object> LoadAsset(string path, Type type);

        AsyncAssets LoadAllAssets<T>() where T : Object;

        AsyncAssets LoadAllAssets(Type type);

        void Unload(bool unloadAllLoadedObjects);
    }
}