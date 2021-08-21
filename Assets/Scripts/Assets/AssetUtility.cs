using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Minecraft.Assets
{
    public static class AssetUtility
    {
#if UNITY_EDITOR
        public static AssetCatalog CreateEditorAssetCatalog()
        {
            Dictionary<string, AssetInfo> assets = new Dictionary<string, AssetInfo>();
            Dictionary<string, AssetBundleInfo> assetBundles = new Dictionary<string, AssetBundleInfo>();

            foreach (string assetBundleName in AssetDatabase.GetAllAssetBundleNames())
            {
                string[] assetsInAssetBundle = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);

                foreach (string asset in assetsInAssetBundle)
                {
                    AssetInfo assetInfo = new AssetInfo
                    {
                        AssetName = asset,
                        AssetBundleName = assetBundleName
                    };
                    assets.Add(asset, assetInfo);
                    assets.Add(AssetDatabase.AssetPathToGUID(asset), assetInfo);
                }

                assetBundles.Add(assetBundleName, new AssetBundleInfo
                {
                    FileName = assetBundleName,
                    Assets = assetsInAssetBundle,
                    Dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, false)
                });
            }

            return new AssetCatalog
            {
                Assets = assets,
                AssetBundles = assetBundles
            };
        }
#endif
    }
}
