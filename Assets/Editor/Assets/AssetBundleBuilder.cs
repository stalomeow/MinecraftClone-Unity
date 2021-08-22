using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Minecraft.Assets;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor.Assets
{
    public static class AssetBundleBuilder
    {
        [MenuItem("Minecraft-Unity/Assets/Build AssetBundles")]
        public static void Build()
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.StrictMode;

            string folder = EditorUtility.OpenFolderPanel("Build AssetBundles", Application.streamingAssetsPath, "");
            DirectoryInfo directory = new DirectoryInfo(folder);

            if (!directory.Exists)
            {
                Debug.LogError($"Directory '{folder}' does not exist!");
                return;
            }

            directory.Delete(true);
            directory.Create(); // clear it

            try
            {
                BuildPipeline.BuildAssetBundles(directory.FullName, options, buildTarget);

                foreach (var manifest in directory.GetFiles("*.manifest", SearchOption.AllDirectories))
                {
                    manifest.Delete();
                }

                string manifestBundlePath = Path.Combine(directory.FullName, directory.Name);
                File.Delete(manifestBundlePath);

                AssetCatalog catalog = AssetUtility.CreateEditorAssetCatalog();
                string catalogJson = JsonConvert.SerializeObject(catalog);
                File.WriteAllText(Path.Combine(directory.FullName, AssetCatalog.FileName), catalogJson);

                AssetDatabase.Refresh();
                Debug.Log("Build Successfully");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Build Failed");
            }
        }
    }
}