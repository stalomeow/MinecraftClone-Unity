using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Minecraft.BlocksData
{
    [CustomEditor(typeof(Block))]
    public sealed class BlockEditor : Editor
    {
        private const string BlockTypeScriptPath = "/Scripts/BlocksData/BlockType.cs";

        private bool m_UVFoldout;
        private Vector2Int m_UVCountInTex;
        private Vector2Int m_UVIndex;
        private bool m_AudioClipsFoldout;
        private bool m_EventsFoldout;

        private SerializedProperty m_StepSoundsProperty;
        private SerializedProperty m_ExtraAssetsProperty;
        private ReorderableList m_StepSounds;
        private ReorderableList m_ExtraAssets;

        private void OnEnable()
        {
            m_StepSoundsProperty = serializedObject.FindProperty("m_StepAudios");
            m_StepSounds = new ReorderableList(serializedObject, m_StepSoundsProperty, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Step Audios"),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty p = m_StepSoundsProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, p);
                }
            };

            m_ExtraAssetsProperty = serializedObject.FindProperty("m_ExtraAssets");
            m_ExtraAssets = new ReorderableList(serializedObject, m_ExtraAssetsProperty, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Extra Assets"),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty p = m_ExtraAssetsProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, p);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BlockName"));

            SerializedProperty ty = serializedObject.FindProperty("m_Type");
            BlockType type = (BlockType)Enum.Parse(typeof(BlockType), ty.enumNames[ty.enumValueIndex], true);
            ShowBlockTypeEnum(ty);

            SerializedProperty fs = serializedObject.FindProperty("m_Flags");
            BlockFlags flags = (target as Block).Flags;
            EditorGUILayout.PropertyField(fs);

            SerializedProperty vertexType = serializedObject.FindProperty("m_VertexType");
            BlockVertexType v = (BlockVertexType)Enum.Parse(typeof(BlockVertexType), vertexType.enumNames[vertexType.enumValueIndex], true);
            EditorGUILayout.PropertyField(vertexType);

            if ((flags & BlockFlags.Liquid) == BlockFlags.Liquid && v == BlockVertexType.PerpendicularQuads)
            {
                Debug.LogWarning("液体的顶点类型应该是立方体");
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MoveResistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightOpacity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightValue"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Hardness"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DestoryEffectColor"));

            EditorGUILayout.Space();
            m_ExtraAssets.DoLayoutList();

            EditorGUILayout.Space();
            UVFoldout(v);

            EditorGUILayout.Space();
            AudioClipsFoldout(type);

            serializedObject.ApplyModifiedProperties();
        }

        protected override bool ShouldHideOpenButton()
        {
            return true;
        }

        private void ShowBlockTypeEnum(SerializedProperty property)
        {
            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            Rect enumRect = new Rect(rect.x, rect.y, rect.width - 40 - 5, rect.height);
            Rect buttonRect = new Rect(enumRect.xMax + 5, rect.y, 40, rect.height);

            EditorGUI.PropertyField(enumRect, property);

            if (GUI.Button(buttonRect, "New"))
            {
                if (Enum.TryParse<BlockType>(target.name, true, out _))
                    return;

                string scriptPath = Application.dataPath + BlockTypeScriptPath;
                BlockType[] blockTypes = Enum.GetValues(typeof(BlockType)) as BlockType[];

                string script = "using XLua;\n\nnamespace Minecraft.BlocksData\n{\n    [LuaCallCSharp]\n    public enum BlockType : byte\n    {\n";

                foreach (var t in blockTypes)
                {
                    script += string.Format($"        {t.ToString()} = {((byte)t).ToString()},\n");
                }

                script += string.Format($"        {target.name} = {blockTypes.Length.ToString()},\n");
                script += "    }\n}";

                File.WriteAllText(scriptPath, script);
                AssetDatabase.Refresh();
            }
        }

        private void UVFoldout(BlockVertexType type)
        {
            if (m_UVFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(m_UVFoldout, "UV Data"))
            {
                switch (type)
                {
                    case BlockVertexType.Cube:
                        CubeUV();
                        break;
                    case BlockVertexType.PerpendicularQuads:
                        PerpendicularQuadsUV();
                        break;
                    default:
                        NoneUV();
                        break;
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void PerpendicularQuadsUV()
        {
            SerializedProperty lb = serializedObject.FindProperty("PositiveXLB");
            SerializedProperty rb = serializedObject.FindProperty("PositiveXRB");
            SerializedProperty rt = serializedObject.FindProperty("PositiveXRT");
            SerializedProperty lt = serializedObject.FindProperty("PositiveXLT");

            UVData("Main", lb.vector2Value, rb.vector2Value, rt.vector2Value, lt.vector2Value);

            EditorGUILayout.Space();
            TexConfig();
            EditorGUILayout.Space();

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Calculate", GUILayout.MaxWidth(75)))
                {
                    CalculateUV(lb, rb, rt, lt);
                }

                if (GUILayout.Button("Shrink", GUILayout.MaxWidth(75)))
                {
                    Shrink(lb, rb, rt, lt);
                }
            }
        }

        private void CubeUV()
        {
            SerializedProperty pxlb = serializedObject.FindProperty("PositiveXLB");
            SerializedProperty pxrb = serializedObject.FindProperty("PositiveXRB");
            SerializedProperty pxrt = serializedObject.FindProperty("PositiveXRT");
            SerializedProperty pxlt = serializedObject.FindProperty("PositiveXLT");

            SerializedProperty pylb = serializedObject.FindProperty("PositiveYLB");
            SerializedProperty pyrb = serializedObject.FindProperty("PositiveYRB");
            SerializedProperty pyrt = serializedObject.FindProperty("PositiveYRT");
            SerializedProperty pylt = serializedObject.FindProperty("PositiveYLT");

            SerializedProperty pzlb = serializedObject.FindProperty("PositiveZLB");
            SerializedProperty pzrb = serializedObject.FindProperty("PositiveZRB");
            SerializedProperty pzrt = serializedObject.FindProperty("PositiveZRT");
            SerializedProperty pzlt = serializedObject.FindProperty("PositiveZLT");

            SerializedProperty nxlb = serializedObject.FindProperty("NegativeXLB");
            SerializedProperty nxrb = serializedObject.FindProperty("NegativeXRB");
            SerializedProperty nxrt = serializedObject.FindProperty("NegativeXRT");
            SerializedProperty nxlt = serializedObject.FindProperty("NegativeXLT");

            SerializedProperty nylb = serializedObject.FindProperty("NegativeYLB");
            SerializedProperty nyrb = serializedObject.FindProperty("NegativeYRB");
            SerializedProperty nyrt = serializedObject.FindProperty("NegativeYRT");
            SerializedProperty nylt = serializedObject.FindProperty("NegativeYLT");

            SerializedProperty nzlb = serializedObject.FindProperty("NegativeZLB");
            SerializedProperty nzrb = serializedObject.FindProperty("NegativeZRB");
            SerializedProperty nzrt = serializedObject.FindProperty("NegativeZRT");
            SerializedProperty nzlt = serializedObject.FindProperty("NegativeZLT");

            UVData("X+", pxlb.vector2Value, pxrb.vector2Value, pxrt.vector2Value, pxlt.vector2Value);
            UVData("Y+", pylb.vector2Value, pyrb.vector2Value, pyrt.vector2Value, pylt.vector2Value);
            UVData("Z+", pzlb.vector2Value, pzrb.vector2Value, pzrt.vector2Value, pzlt.vector2Value);
            UVData("X-", nxlb.vector2Value, nxrb.vector2Value, nxrt.vector2Value, nxlt.vector2Value);
            UVData("Y-", nylb.vector2Value, nyrb.vector2Value, nyrt.vector2Value, nylt.vector2Value);
            UVData("Z-", nzlb.vector2Value, nzrb.vector2Value, nzrt.vector2Value, nzlt.vector2Value);

            EditorGUILayout.Space();
            TexConfig();
            EditorGUILayout.Space();

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (EditorGUILayout.DropdownButton(new GUIContent("Calculate"), FocusType.Passive, GUILayout.MaxWidth(75)))
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("X+"), false, () => CalculateUV(pxlb, pxrb, pxrt, pxlt));
                    menu.AddItem(new GUIContent("Y+"), false, () => CalculateUV(pylb, pyrb, pyrt, pylt));
                    menu.AddItem(new GUIContent("Z+"), false, () => CalculateUV(pzlb, pzrb, pzrt, pzlt));
                    menu.AddItem(new GUIContent("X-"), false, () => CalculateUV(nxlb, nxrb, nxrt, nxlt));
                    menu.AddItem(new GUIContent("Y-"), false, () => CalculateUV(nylb, nyrb, nyrt, nylt));
                    menu.AddItem(new GUIContent("Z-"), false, () => CalculateUV(nzlb, nzrb, nzrt, nzlt));

                    menu.AddSeparator("");

                    menu.AddItem(new GUIContent("X And Z"), false, () =>
                    {
                        CalculateUV(pxlb, pxrb, pxrt, pxlt);
                        CalculateUV(pzlb, pzrb, pzrt, pzlt);
                        CalculateUV(nxlb, nxrb, nxrt, nxlt);
                        CalculateUV(nzlb, nzrb, nzrt, nzlt);
                    });

                    menu.AddItem(new GUIContent("Y"), false, () =>
                    {
                        CalculateUV(pylb, pyrb, pyrt, pylt);
                        CalculateUV(nylb, nyrb, nyrt, nylt);
                    });

                    menu.AddItem(new GUIContent("All"), false, () =>
                    {
                        CalculateUV(pxlb, pxrb, pxrt, pxlt);
                        CalculateUV(pylb, pyrb, pyrt, pylt);
                        CalculateUV(pzlb, pzrb, pzrt, pzlt);
                        CalculateUV(nxlb, nxrb, nxrt, nxlt);
                        CalculateUV(nylb, nyrb, nyrt, nylt);
                        CalculateUV(nzlb, nzrb, nzrt, nzlt);
                    });

                    menu.ShowAsContext();
                }

                if (GUILayout.Button("Shrink", GUILayout.MaxWidth(75)))
                {
                    Shrink(pxlb, pxrb, pxrt, pxlt);
                    Shrink(pylb, pyrb, pyrt, pylt);
                    Shrink(pzlb, pzrb, pzrt, pzlt);
                    Shrink(nxlb, nxrb, nxrt, nxlt);
                    Shrink(nylb, nyrb, nyrt, nylt);
                    Shrink(nzlb, nzrb, nzrt, nzlt);
                }
            }
        }

        private void NoneUV()
        {
            EditorGUILayout.HelpBox("No UV", MessageType.Info);
        }

        private void CalculateUV(SerializedProperty lb, SerializedProperty rb, SerializedProperty rt, SerializedProperty lt)
        {
            try
            {
                if (m_UVCountInTex.x <= 0 || m_UVCountInTex.y <= 0)
                    throw new ArgumentOutOfRangeException("UVCount");

                float uvWidth = 1f / m_UVCountInTex.x;
                float uvHeight = 1f / m_UVCountInTex.y;

                lb.vector2Value = new Vector2(uvWidth * m_UVIndex.x, uvHeight * m_UVIndex.y);
                rb.vector2Value = new Vector2(uvWidth * (m_UVIndex.x + 1), uvHeight * m_UVIndex.y);
                rt.vector2Value = new Vector2(uvWidth * (m_UVIndex.x + 1), uvHeight * (m_UVIndex.y + 1));
                lt.vector2Value = new Vector2(uvWidth * m_UVIndex.x, uvHeight * (m_UVIndex.y + 1));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void Shrink(SerializedProperty lb, SerializedProperty rb, SerializedProperty rt, SerializedProperty lt)
        {
            try
            {
                if (m_UVCountInTex.x <= 0 || m_UVCountInTex.y <= 0)
                    throw new ArgumentOutOfRangeException("UVCount");

                float x = 1f / m_UVCountInTex.x / 16f / 4f;
                float y = 1f / m_UVCountInTex.y / 16f / 4f;

                lb.vector2Value += new Vector2(x, y);
                rb.vector2Value += new Vector2(-x, y);
                rt.vector2Value += new Vector2(-x, -y);
                lt.vector2Value += new Vector2(x, -y);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void TexConfig()
        {
            EditorGUILayout.LabelField("Calculator", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                m_UVCountInTex = EditorGUILayout.Vector2IntField("UV Count In Texture", m_UVCountInTex);
                m_UVIndex = EditorGUILayout.Vector2IntField("UV Index", m_UVIndex);
            }
        }

        private void UVData(string label, Vector2 lb, Vector2 rb, Vector2 rt, Vector2 lt)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.LabelField(lt.ToString(), rt.ToString());
                EditorGUILayout.LabelField(lb.ToString(), rb.ToString());
            }
        }

        private void AudioClipsFoldout(BlockType type)
        {
            if (m_AudioClipsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(m_AudioClipsFoldout, "Audio Clips"))
            {
                if (type == BlockType.Air)
                {
                    EditorGUILayout.HelpBox("No Audio Clip", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DigAudio"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PlaceAudio"));

                    EditorGUILayout.Space();

                    m_StepSounds.DoLayoutList();
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}