using UnityEngine;

namespace Minecraft
{
    public sealed class MenuCamera : MonoBehaviour
    {
        [SerializeField] private float m_RotateSpeed = 10;

        private void Update()
        {
            transform.Rotate(0, Time.deltaTime * m_RotateSpeed, 0, Space.World);
        }
    }
}