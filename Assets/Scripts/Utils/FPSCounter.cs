using System.Text;
using UnityEngine;

namespace Minecraft.Utils
{
    public class FPSCounter : MonoBehaviour
    {
        private StringBuilder m_StringBuilder;
        private int m_AccumulatedFrames;
        private float m_LastUpdateTextTime;

        private void Start()
        {
            m_StringBuilder = new StringBuilder();
            m_AccumulatedFrames = 0;
            m_LastUpdateTextTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            if ((Time.realtimeSinceStartup - m_LastUpdateTextTime) >= 1)
            {
                m_StringBuilder.Clear().Append("FPS/ ").Append(m_AccumulatedFrames);
                m_LastUpdateTextTime = Time.realtimeSinceStartup;
                m_AccumulatedFrames = 0;
            }
            else
            {
                m_AccumulatedFrames++;
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 400, 100), m_StringBuilder.ToString());
        }
    }
}
