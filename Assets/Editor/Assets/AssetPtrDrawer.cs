using System;
using System.Reflection;
using Minecraft.Assets;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MinecraftEditor.Assets
{
    [CustomPropertyDrawer(typeof(AssetPtr), true)]
    public class AssetPtrDrawer : PropertyDrawer
    {
        private Type m_AssetType = null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorAssetUtility.GetAssetPtrFieldHeight(GetAssetType());
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorAssetUtility.AssetPtrField(position, label, property, GetAssetType());
        }

        private Type GetAssetType()
        {
            if (m_AssetType == null)
            {
                EnsureAssetTypeAttribute assetType = fieldInfo.GetCustomAttribute<EnsureAssetTypeAttribute>();
                m_AssetType = typeof(Object);

                if (assetType != null)
                {
                    if (typeof(Object).IsAssignableFrom(assetType.AssetType))
                    {
                        m_AssetType = assetType.AssetType;
                    }
                    else
                    {
                        Debug.LogError($"Invalid AssetType: {assetType.AssetType}, it must be derived from UnityEngine.Object.");
                    }
                }
            }

            return m_AssetType;
        }
    }
}
