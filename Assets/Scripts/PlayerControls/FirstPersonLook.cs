using System;
using Minecraft.InspectorExtensions;
using UnityEngine;

namespace Minecraft.PlayerControls
{
    [Serializable]
    public class FirstPersonLook
    {
        public bool EnableSmooth = false;
        [ConditionalDisplay("EnableSmooth")] public float SmoothTime = 5f;

        [Space]

        public bool ClampVerticalRotation = true;
        public Vector2 Sensitivity = new Vector2(0.2f, 0.2f);
        [MinMaxSlider(-180, 180)] public Vector2 AngleRangeX = new Vector2(-90f, 90f);

        [NonSerialized] private bool m_CursorLocked;
        [NonSerialized] private Transform m_Character;
        [NonSerialized] private Transform m_Camera;
        [NonSerialized] private Quaternion m_CharacterRotation;
        [NonSerialized] private Quaternion m_CameraRotation;


        public void Initialize(Transform character, Transform camera, bool lockCursor)
        {
            m_CursorLocked = !lockCursor;
            m_Character = character;
            m_Camera = camera;
            m_CharacterRotation = character.localRotation;
            m_CameraRotation = camera.localRotation;

            SetCursorLockMode(true);
        }

        public void LookRotation(Vector2 rotation, float deltaTime)
        {
            rotation *= Sensitivity;
            m_CharacterRotation *= Quaternion.Euler(0f, rotation.x, 0f);
            m_CameraRotation *= Quaternion.Euler(-rotation.y, 0f, 0f);

            if (ClampVerticalRotation)
            {
                ClampRotationAroundXAxis(ref m_CameraRotation);
            }

            if (EnableSmooth)
            {
                m_Character.localRotation = Quaternion.Slerp(m_Character.localRotation, m_CharacterRotation, SmoothTime * deltaTime);
                m_Camera.localRotation = Quaternion.Slerp(m_Camera.localRotation, m_CameraRotation, SmoothTime * deltaTime);
            }
            else
            {
                m_Character.localRotation = m_CharacterRotation;
                m_Camera.localRotation = m_CameraRotation;
            }
        }

        public void SetCursorLockMode(bool lockCursor)
        {
            m_CursorLocked = lockCursor;

            if (m_CursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void ClampRotationAroundXAxis(ref Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = Mathf.Clamp(2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x), AngleRangeX.x, AngleRangeX.y);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        }
    }
}