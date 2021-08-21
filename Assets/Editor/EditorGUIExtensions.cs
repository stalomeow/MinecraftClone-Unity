using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor
{
    public static class EditorGUIExtension
    {
        private static readonly MethodInfo s_Method = typeof(EditorGUIUtility).GetMethod("HandleHorizontalSplitter", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly Color s_NormalSplitterColor = new Color(0.6f, 0.6f, 0.6f, 1.333f);
        private static readonly Color s_ProSplitterColor = new Color(0.12f, 0.12f, 0.12f, 1.333f);

        public static Rect HandleHorizontalSplitter(Rect dragRect, float width, float minLeftSide, float minRightSide)
        {
            return (Rect)s_Method.Invoke(null, new object[] { dragRect, width, minLeftSide, minRightSide });
        }

        public static void DrawSplitter(Rect rect)
        {
            Color color = EditorGUIUtility.isProSkin ? s_ProSplitterColor : s_NormalSplitterColor;
            Color guiColor = GUI.color;
            GUI.color *= color;
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            GUI.color = guiColor;
        }
    }
}
