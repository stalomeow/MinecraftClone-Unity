using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MinecraftEditor
{
    public static class TextureHelper
    {
        [MenuItem("Minecraft-Unity/Textures/Merge Textures")]
        private static void Merge()
        {
            Object[] objs = Selection.objects;
            List<Texture2D> textures = new List<Texture2D>();

            foreach (Object obj in objs)
            {
                if (obj is Texture2D tex)
                {
                    textures.Add(tex);
                }
            }

            if (textures.Count == 0)
            {
                Debug.LogWarning("Nothing to merge.");
                return;
            }

            Texture2D firstTex = textures[0];
            Texture2DArray array = new Texture2DArray(firstTex.width, firstTex.height, textures.Count, firstTex.format, false)
            {
                anisoLevel = firstTex.anisoLevel,
                mipMapBias = firstTex.mipMapBias,
                wrapMode = firstTex.wrapMode,
                filterMode = firstTex.filterMode
            };

            try
            {
                for (int i = 0; i < textures.Count; i++)
                {
                    EditorUtility.DisplayProgressBar("Hold On...", "Merging textures...", (float)i / textures.Count);

                    Texture2D texture = textures[i];
                    Graphics.CopyTexture(texture, 0, 0, array, i, 0);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            string path = EditorUtility.SaveFilePanelInProject("Save", "Textures", "asset", string.Empty);

            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(array, path);
                AssetDatabase.Refresh();
            }
        }

        [MenuItem("Minecraft-Unity/Textures/Split Texture From Array")]
        private static void Split()
        {
            Object obj = Selection.activeObject;

            if (!(obj is Texture2DArray array))
            {
                Debug.LogWarning("Selected object is not a Texture2DArray.");
                return;
            }

            if (array.depth == 0)
            {
                Debug.LogWarning("There is no texture in the Texture2DArray.");
                return;
            }

            List<Texture2D> textures = new List<Texture2D>();

            try
            {
                for (int i = 0; i < array.depth; i++)
                {
                    EditorUtility.DisplayProgressBar("Hold On...", "Splitting textures...", (float)i / array.depth);

                    Texture2D texture = new Texture2D(array.width, array.height);
                    Graphics.CopyTexture(array, i, 0, texture, 0, 0);
                    textures.Add(texture);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "texture", "png");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string folder = Path.GetDirectoryName(path);
            string file = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);

            try
            {
                for (int i = 0; i < textures.Count; i++)
                {
                    EditorUtility.DisplayProgressBar("Hold On...", "Saving textures...", (float)i / textures.Count);

                    byte[] bytes = ext switch
                    {
                        ".png" => textures[i].EncodeToPNG(),
                        ".jpg" => textures[i].EncodeToJPG(),
                        _ => throw new NotSupportedException("Unsupported extension: " + ext)
                    };
                    File.WriteAllBytes(Path.Combine(folder, $"{file}_{i}{ext}"), bytes);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.Refresh();
        }
    }
}
