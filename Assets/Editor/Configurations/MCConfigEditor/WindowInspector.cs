using UnityEditor;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public abstract class WindowInspector
    {
        public MainWindow MainWnd { get; }

        private Rect m_LastRect;
        private Vector2 m_ScrollPos;

        protected WindowInspector(MainWindow mainWindow)
        {
            MainWnd = mainWindow;
            m_LastRect = default;
            m_ScrollPos = default;
        }

        public virtual void OnEnable() { }

        public virtual void OnDisable() { }

        public bool OnGUI(Rect rect, int index)
        {
            Rect initialRect = rect;
            VerticalGUIRect guiRect = GetGUIRectWithMargin(ref rect);

            OnBeforeScrollableGUI(ref guiRect, index);

            Rect scrollViewRect = new Rect(initialRect.x, initialRect.y + guiRect.UsedHeight, initialRect.width, initialRect.height - guiRect.UsedHeight);
            m_ScrollPos = GUI.BeginScrollView(scrollViewRect, m_ScrollPos, m_LastRect);

            EditorGUI.BeginChangeCheck();
            OnScrollableGUI(ref guiRect, index);
            guiRect.Space(10);
            bool changed = EditorGUI.EndChangeCheck();

            GUI.EndScrollView();
            m_LastRect = new Rect(scrollViewRect.x, scrollViewRect.y, rect.xMax - scrollViewRect.x, guiRect.UsedHeight + scrollViewRect.height - initialRect.height);
            return changed;
        }

        protected virtual void OnBeforeScrollableGUI(ref VerticalGUIRect rect, int index) { }

        protected abstract void OnScrollableGUI(ref VerticalGUIRect rect, int index);

        protected static VerticalGUIRect GetGUIRectWithMargin(ref Rect rect)
        {
            rect.x += 10;
            rect.width -= 30; // reserve for scrollbar
            return rect;
        }
    }
}
