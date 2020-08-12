using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Minecraft.BlocksData
{
    [CustomEditor(typeof(BlockEvents), true)]
    public sealed class BlockEventsEditor : Editor
    {
        private Dictionary<string, List<string>> m_Events;
        private bool[] m_Foldouts;

        private void OnEnable()
        {
            m_Events = new Dictionary<string, List<string>>();

            Type type = target.GetType();
            char[] separator = new char[] { '_' };
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            IEnumerable<MethodInfo> methodsSorted = from m in methods orderby m.Name ascending select m;

            foreach (MethodInfo method in methodsSorted)
            {
                if (!method.Name.StartsWith("BlockEvent_"))
                    continue;

                string[] keywords = method.Name.Split(separator);

                if (keywords.Length < 3)
                    continue;

                string block = string.Join("_", keywords, 2, keywords.Length - 2);

                if (m_Events.TryGetValue(keywords[1], out List<string> blocks))
                {
                    blocks.Add(block);
                }
                else
                {
                    blocks = new List<string>() { block };
                    m_Events.Add(keywords[1], blocks);
                }
            }

            m_Foldouts = new bool[m_Events.Count];
        }

        public override void OnInspectorGUI()
        {
            int i = 0;

            foreach (var pair in m_Events)
            {
                List<string> blocks = pair.Value;

                if (m_Foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(m_Foldouts[i], pair.Key))
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        foreach (string v in blocks)
                        {
                            EditorGUILayout.LabelField(v);
                        }
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();

                i++;
            }
        }

        protected override bool ShouldHideOpenButton()
        {
            return true;
        }
    }
}