using System.Text;
using TMPro;
using UnityEngine;

namespace Minecraft.DebugUtils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ChunkInfoPrinter : MonoBehaviour
    {
        private TextMeshProUGUI m_Text;
        private StringBuilder m_StrBuilder;

        private void Start()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_StrBuilder = new StringBuilder();
        }

        private void LateUpdate()
        {
            ChunkManager manager = WorldManager.Active.ChunkManager;
            m_StrBuilder.Clear();
            m_StrBuilder.AppendFormat("{0} loaded chunks, {1} rendered", manager.LoadedChunkCount.ToString(), manager.RenderChunkCount.ToString());
            m_Text.text = m_StrBuilder.ToString();
        }
    }
}