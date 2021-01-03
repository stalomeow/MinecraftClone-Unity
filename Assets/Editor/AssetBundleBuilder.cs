using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Minecraft.AssetManagement
{
    public static class AssetBundleBuilder
    {
        //public static void Build(string outputPath, BuildTarget buildTarget, Texture2D icon)
        //{
        //    string desOutputPath = outputPath;
        //    outputPath = Application.dataPath.Replace("Assets", string.Empty) + "Library/AssetBundleBuild";

        //    try
        //    {
        //        DirectoryInfo directory = new DirectoryInfo(outputPath);

        //        if (directory.Exists)
        //        {
        //            directory.Delete(true);
        //        }

        //        directory.Create();

        //        var manifest = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.StrictMode, buildTarget);
        //        string manifestBundlePath = Path.Combine(directory.FullName, directory.Name);

        //        FileInfo[] bundleFiles = (from info in directory.GetFiles("*.*", SearchOption.AllDirectories)
        //                                  let path = info.FullName
        //                                  where (!path.EndsWith(".manifest")) && (path.Replace('/', '\\') != manifestBundlePath.Replace('/', '\\'))
        //                                  select info).ToArray();

        //        List<AssetBundleMeta> assetBundleMetas = new List<AssetBundleMeta>();

        //        for (int i = 0; i < bundleFiles.Length; i++)
        //        {
        //            FileInfo file = bundleFiles[i];
        //            string bundlePath = file.FullName;
        //            AssetBundle ab = AssetBundle.LoadFromFile(bundlePath);
        //            AssetBundleMeta meta = null;

        //            try
        //            {
        //                meta = new AssetBundleMeta
        //                {
        //                    Name = ab.name,
        //                    FileName = ab.name,
        //                    Dependencies = manifest.GetAllDependencies(ab.name)
        //                };
        //            }
        //            finally
        //            {
        //                ab.Unload(true);
        //            }

        //            if (meta != null)
        //            {
        //                assetBundleMetas.Add(meta);
        //            }

        //            EditorUtility.DisplayProgressBar("Hold On...", "Building Asset Bundles", (float)(i + 1) / bundleFiles.Length);
        //        }

        //        EditorUtility.ClearProgressBar();

        //        AssetBundleManifest m = new AssetBundleManifest
        //        {
        //            AssetBundles = assetBundleMetas.ToArray()
        //        };
                
        //        string json = JsonUtility.ToJson(m, false);
        //        File.WriteAllText(Path.Combine(outputPath, AssetBundleManifest.FileName), json);

        //        FileInfo[] manifests = directory.GetFiles("*.manifest", SearchOption.AllDirectories);

        //        for (int i = 0; i < manifests.Length; i++)
        //        {
        //            manifests[i].Delete();
        //        }

        //        File.Delete(manifestBundlePath);

        //        string iconPath = Path.Combine(outputPath, WorldConsts.ResourcePackageIconName);
        //        byte[] iconData = icon.EncodeToPNG();
        //        File.WriteAllBytes(iconPath, iconData);

        //        DirectoryCopy(outputPath, desOutputPath);

        //        AssetDatabase.Refresh();
        //        Debug.Log("Build Successfully");
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //        Debug.Log("Build Failed");
        //    }
        //}

        //private static void DirectoryCopy(string sourceDirName, string destDirName)
        //{
        //    if (!Directory.Exists(destDirName))
        //    {
        //        Directory.CreateDirectory(destDirName);
        //    }

        //    foreach (string folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
        //    {
        //        if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
        //        {
        //            Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
        //        }
        //    }

        //    foreach (string filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
        //    {
        //        var fileDirName = Path.GetDirectoryName(filePath).Replace("\\", "/");
        //        var fileName = Path.GetFileName(filePath);
        //        string newFilePath = Path.Combine(fileDirName.Replace(sourceDirName, destDirName), fileName);

        //        File.Copy(filePath, newFilePath, true);
        //    }
        //}
    }
}