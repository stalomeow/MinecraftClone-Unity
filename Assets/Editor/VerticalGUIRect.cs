using UnityEditor;
using UnityEngine;

namespace MinecraftEditor
{
    public struct VerticalGUIRect
    {
        private readonly float m_InitialPosX;
        private readonly float m_InitialPosY;
        private readonly float m_Width;
        private float m_PosY;

        public float UsedHeight => m_PosY - m_InitialPosY;

        public Rect Next => GetNext();

        public VerticalGUIRect(Rect rect)
        {
            m_InitialPosX = rect.x;
            m_InitialPosY = rect.y;
            m_Width = rect.width;
            m_PosY = m_InitialPosY;
        }

        public Rect GetNext()
        {
            return GetNext(EditorGUIUtility.singleLineHeight, true, true);
        }

        public Rect GetNext(bool withStandardVerticalSpacing)
        {
            return GetNext(EditorGUIUtility.singleLineHeight, true, withStandardVerticalSpacing);
        }

        public Rect GetNext(float height, bool addIndent, bool withStandardVerticalSpacing)
        {
            Rect rect = new Rect(m_InitialPosX, m_PosY, m_Width, height);

            m_PosY += height;

            if (withStandardVerticalSpacing)
            {
                m_PosY += EditorGUIUtility.standardVerticalSpacing;
            }

            return addIndent ? EditorGUI.IndentedRect(rect) : rect;
        }

        public void Space(float height)
        {
            GetNext(height, false, false);
        }

        public static implicit operator VerticalGUIRect(Rect rect)
        {
            return new VerticalGUIRect(rect);
        }
    }
}
