using System;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    public sealed class MouseLook
    {
        [SerializeField] private float m_XSensitivity = 2f;
        [SerializeField] private float m_YSensitivity = 2f;
        [SerializeField] private bool m_ClampVerticalRotation = true;
        [SerializeField] private float m_MinimumX = -90f;
        [SerializeField] private float m_MaximumX = 90f;
        [SerializeField] private bool m_Smooth = false;
        [SerializeField] private float m_SmoothTime = 5f;
        [SerializeField] private bool m_LockCursor = true;

        private Quaternion m_CharacterTargetRotation;
        private Quaternion m_CameraTargetRotation;

        public float XSensitivity
        {
            get => m_XSensitivity;
            set => m_XSensitivity = value;
        }

        public float YSensitivity
        {
            get => m_YSensitivity;
            set => m_YSensitivity = value;
        }

        public bool LockCursor
        {
            get => m_LockCursor;
            set
            {
                if (!(m_LockCursor = value))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }


        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRotation = character.localRotation;
            m_CameraTargetRotation = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera, float deltaTime)
        {
            float yRot = Input.GetAxis("Mouse X") * m_XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * m_YSensitivity;

            m_CharacterTargetRotation *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRotation *= Quaternion.Euler(-xRot, 0f, 0f);

            if (m_ClampVerticalRotation)
            {
                m_CameraTargetRotation = ClampRotationAroundXAxis(m_CameraTargetRotation);
            }

            if (m_Smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRotation, m_SmoothTime * deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRotation, m_SmoothTime * deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRotation;
                camera.localRotation = m_CameraTargetRotation;
            }
        }

        public void UpdateCursorLock()
        {
            if (!m_LockCursor)
                return;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, m_MinimumX, m_MaximumX);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}