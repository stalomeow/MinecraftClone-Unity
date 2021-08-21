using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Assets;
using MinecraftEditor.Assets;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class AssetPtrList<T> : IEnumerable<AssetPtr> where T : Object
    {
        [Serializable]
        public sealed class Element
        {
            public AssetPtr Asset;
            public int RefCount;
        }

        [JsonProperty] public List<Element> Elements;
        [JsonProperty] public Dictionary<string, int> IndexMap;

        public event Action<int> OnElementRemoved;

        public AssetPtr this[int? index] => index == null ? AssetPtr.NullPtr : Elements[index.Value].Asset;

        public AssetPtrList()
        {
            Elements = new List<Element>();
            IndexMap = new Dictionary<string, int>();
        }

        public void ElementGUI(Rect rect, GUIContent label, ref int? location)
        {
            AssetPtr asset = location == null ? AssetPtr.NullPtr : Elements[location.Value].Asset;
            AssetPtr newAsset = EditorAssetUtility.AssetPtrField(rect, label, asset, typeof(T), out bool changed);

            if (changed)
            {
                SetAssetToLocation(ref location, newAsset);
            }
        }

        public void SetAssetToLocation(ref int? location, AssetPtr asset)
        {
            if (location != null)
            {
                int index = location.Value;
                Element element = Elements[index];
                Assert.AreNotEqual(element.Asset, AssetPtr.NullPtr);

                if (element.Asset == asset)
                {
                    return;
                }

                if (--element.RefCount == 0)
                {
                    Elements.RemoveAt(index);
                    IndexMap.Remove(element.Asset.AssetGUID);

                    for (int i = index; i < Elements.Count; i++)
                    {
                        IndexMap[Elements[i].Asset.AssetGUID] = i;
                    }

                    OnElementRemoved?.Invoke(index);
                }
            }

            if (asset == AssetPtr.NullPtr)
            {
                location = null;
            }
            else if (IndexMap.TryGetValue(asset.AssetGUID, out int index))
            {
                location = index;
                Elements[index].RefCount++;
            }
            else
            {
                location = index = Elements.Count;
                Elements.Add(new Element
                {
                    Asset = asset,
                    RefCount = 1
                });
                IndexMap.Add(asset.AssetGUID, index);
            }
        }

        public AssetPtr[] ToArray()
        {
            AssetPtr[] result = new AssetPtr[Elements.Count];

            for (int i = 0; i < Elements.Count; i++)
            {
                result[i] = Elements[i].Asset;
            }

            return result;
        }

        public IEnumerator<AssetPtr> GetEnumerator()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                yield return Elements[i].Asset;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
