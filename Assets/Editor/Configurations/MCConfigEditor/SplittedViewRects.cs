using System;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    [Serializable]
    public struct SplittedViewRects : IEquatable<SplittedViewRects>
    {
        public const float SplitterLineWidth = 1f;

        [SerializeField] private float m_LeftWidth;

        public SplittedViewRects(float initialLeftWidth)
        {
            m_LeftWidth = initialLeftWidth;
        }

        public void GetRects(Rect rect, float splitterWidth, float leftMinWidth, float rightMinWidth, out Rect left, out Rect right)
        {
            EditorGUIExtension.DrawSplitter(new Rect(rect.x + m_LeftWidth, rect.y, SplitterLineWidth, rect.height));
            Rect dragRect = new Rect(rect.x + m_LeftWidth, rect.y, splitterWidth, rect.height);
            Rect newRect = EditorGUIExtension.HandleHorizontalSplitter(dragRect, rect.width, leftMinWidth, rightMinWidth);
            m_LeftWidth = newRect.x;
            left = new Rect(rect.x, rect.y, m_LeftWidth, rect.height);
            right = new Rect(left.xMax, rect.y, rect.width - left.width, rect.height);
        }

        public bool Equals(SplittedViewRects other)
        {
            return m_LeftWidth == other.m_LeftWidth;
        }
    }
}
