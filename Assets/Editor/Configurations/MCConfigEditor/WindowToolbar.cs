using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class WindowToolbar
    {
        public static readonly float Height = 20;
        public static readonly GUIContent BrowseConfigFolderContent = new GUIContent("Browse Config Folder");

        private readonly MainWindow m_MainWindow;
        private readonly SearchField m_SearchField;

        public string SearchString { get; set; }

        public event Action OnAddButtonClicked;
        public event Action OnRemoveButtonClicked;
        public event Action OnSaveButtonClicked;

        public WindowToolbar(MainWindow mainWindow, string searchString)
        {
            m_MainWindow = mainWindow;
            m_SearchField = new SearchField();

            SearchString = searchString;
        }

        public void OnGUI()
        {
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar, GUILayout.Height(Height)))
            {
                m_MainWindow.GUIMode = (WindowGUIMode)EditorGUILayout.EnumPopup(m_MainWindow.GUIMode, EditorStyles.toolbarPopup, GUILayout.Width(70));

                if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus"), EditorStyles.toolbarButton))
                {
                    OnAddButtonClicked?.Invoke();
                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), EditorStyles.toolbarButton))
                {
                    OnRemoveButtonClicked?.Invoke();
                }

                GUILayout.FlexibleSpace();
                SearchString = m_SearchField.OnToolbarGUI(SearchString, GUILayout.MaxWidth(300));
                EditorGUILayout.Space();

                if (GUILayout.Button(EditorGUIUtility.IconContent("SaveActive"), EditorStyles.toolbarButton))
                {
                    OnSaveButtonClicked?.Invoke();
                }

                DrawSettingsButton();
            }
        }

        private void DrawSettingsButton()
        {
            if (EditorGUILayout.DropdownButton(EditorGUIUtility.IconContent("SettingsIcon"), FocusType.Passive, EditorStyles.toolbarButton))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(BrowseConfigFolderContent, false, () =>
                {
                    m_MainWindow.ConfigFolder = EditorUtility.SaveFolderPanel(BrowseConfigFolderContent.text, Application.dataPath, string.Empty);
                });

                menu.ShowAsContext();
            }
        }
    }
}
