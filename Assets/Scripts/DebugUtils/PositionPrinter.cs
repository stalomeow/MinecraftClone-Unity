using System.Text;
using TMPro;
using UnityEngine;

namespace Minecraft.DebugUtils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PositionPrinter : MonoBehaviour
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
            Vector3 pos = WorldManager.Active.EntityManager.PlayerObj.transform.localPosition;

            int playerX = Mathf.FloorToInt(pos.x);
            int playerY = Mathf.FloorToInt(pos.y);
            int playerZ = Mathf.FloorToInt(pos.z);

            Vector2Int cp = Chunk.NormalizeToChunkPosition(pos.x, pos.z);

            m_StrBuilder.Clear();
            m_StrBuilder.AppendFormat("position <{0}, {1}, {2}> in chunk <{3}, {4}>",
                playerX.ToString(),
                playerY.ToString(),
                playerZ.ToString(),
                cp.x.ToString(),
                cp.y.ToString()
            );

            m_Text.text = m_StrBuilder.ToString();
        }
    }
}