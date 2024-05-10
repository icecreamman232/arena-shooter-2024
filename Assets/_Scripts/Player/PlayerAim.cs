using System;
using UnityEngine;

namespace JustGame.Script.Player
{
    public class PlayerAim : MonoBehaviour
    {
        private Camera m_camera;
        private Vector3 m_worldPos;
        private Vector2 m_aimDirection;
        private Quaternion m_aimRotation;
        public Vector2 AimDirection => m_aimDirection;
        public Quaternion AimRotation => m_aimRotation;

        private void Start()
        {
            m_camera = Camera.main;
        }

        private void Update()
        {
            UpdateAim(Input.mousePosition);
        }

        private void UpdateAim(Vector2 mousePos)
        {
            m_worldPos = m_camera.ScreenToWorldPoint(mousePos);
            m_worldPos.z -= m_camera.transform.position.z;

            m_aimDirection = (m_worldPos - transform.position).normalized;
            var angle = Mathf.Atan2(m_aimDirection.y, m_aimDirection.x) * Mathf.Rad2Deg;
            m_aimRotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(m_worldPos,Vector3.one * 0.3f);
        }
    }
}
