using UnityEditor;

namespace Minecraft.BlocksData
{
    [CustomEditor(typeof(BlockRegistry))]
    public sealed class BlockRegistryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Block[] blocks = (target as BlockRegistry).RegisteredBlocks;

            int blockCount = blocks == null ? 0 : blocks.Length;
            EditorGUILayout.LabelField($"Blocks ({blockCount})", EditorStyles.boldLabel);

            if (blockCount == 0)
                return;

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledGroupScope(true))
            {
                foreach (var block in blocks)
                {
                    EditorGUILayout.ObjectField(block, typeof(Block), true);
                }
            }
        }
    }
}