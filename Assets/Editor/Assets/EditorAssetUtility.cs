using System;
using System.Reflection;
using Minecraft.Assets;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MinecraftEditor.Assets
{
    public static class EditorAssetUtility
    {
        //UnityEditor.EditorGUI.kObjectFieldThumbnailHeight
        private static FieldInfo kObjectFieldThumbnailHeight = typeof(EditorGUI).GetField("kObjectFieldThumbnailHeight", BindingFlags.Static | BindingFlags.NonPublic);

        public static float GetAssetPtrFieldHeight(Type assetType)
        {
            if (EditorGUIUtility.HasObjectThumbnail(assetType))
            {
                return (float)kObjectFieldThumbnailHeight.GetValue(null);
            }

            return EditorGUIUtility.singleLineHeight;
        }

        public static AssetPtr AssetPtrField(Rect rect, GUIContent label, AssetPtr value, Type assetType, out bool changed)
        {
            value ??= AssetPtr.NullPtr;
            Object asset = LoadAssetByGUID(value.AssetGUID, assetType);
            Object newAsset = EditorGUI.ObjectField(rect, label, asset, assetType, false);
            changed = newAsset != asset;
            return changed ? newAsset.GetAssetPtr() : value;
        }

        public static AssetPtr AssetPtrField(Rect rect, GUIContent label, AssetPtr value, Type assetType)
        {
            return AssetPtrField(rect, label, value, assetType, out _);
        }

        public static void AssetPtrField(Rect rect, GUIContent label, SerializedProperty property, Type assetType)
        {
            label = EditorGUI.BeginProperty(rect, label, property);
            EditorGUI.BeginChangeCheck();

            SerializedProperty guid = GetAssetPtrGUIDProperty(property);
            Object asset = LoadAssetByGUID(guid.stringValue, assetType);
            asset = EditorGUI.ObjectField(rect, label, asset, assetType, false);

            if (EditorGUI.EndChangeCheck())
            {
                guid.stringValue = GetAssetGUID(asset);
            }

            EditorGUI.EndProperty();
        }

        public static SerializedProperty GetAssetPtrGUIDProperty(SerializedProperty assetPtr)
        {
            return assetPtr.FindPropertyRelative("AssetGUID");
        }

        public static void SetAssetPtrGUID(SerializedProperty assetPtr, string guid)
        {
            GetAssetPtrGUIDProperty(assetPtr).stringValue = guid;
        }

        public static void SetAssetPtrGUID(SerializedProperty assetPtr, Object asset)
        {
            SetAssetPtrGUID(assetPtr, GetAssetGUID(asset));
        }

        public static Object LoadAssetByGUID(string guid, Type type)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath(path, type);
        }

        public static T LoadAssetByGUID<T>(string guid) where T : Object
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static string GetAssetGUID(Object asset)
        {
            if (!asset)
            {
                return string.Empty;
            }

            string path = AssetDatabase.GetAssetPath(asset);
            return AssetDatabase.AssetPathToGUID(path);
        }

        public static AssetPtr GetAssetPtr(this Object asset)
        {
            return new AssetPtr(GetAssetGUID(asset));
        }
    }
}
