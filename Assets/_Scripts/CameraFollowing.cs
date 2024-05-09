using UnityEngine;

namespace JustGame.Scripts.Managers
{
    public class CameraFollowing : MonoBehaviour
    {
        [SerializeField] private Transform m_cameraTransform;
        [SerializeField] private Transform m_targetTransform;
        [SerializeField] private float m_followingSpeed;
        private Vector3 m_targetPos;

        private bool m_canFollow;
        
        private void Awake()
        {
            m_canFollow = true;
        }

        public void SetPermission(bool value)
        {
            m_canFollow = value;
        }
        public void SetTarget(Transform target)
        {
            m_targetTransform = target;
        }

        public void SetCameraPosition(Vector3 newPosition)
        {
            m_cameraTransform.position = newPosition;
        }
        private void FixedUpdate()
        {
            if (!m_canFollow) return;
            if (m_targetTransform == null) return;
            m_targetPos = m_targetTransform.position;
            m_targetPos.z = -10;
            m_cameraTransform.position = Vector3.Lerp(m_cameraTransform.position, m_targetPos, Time.deltaTime * m_followingSpeed / 10);
            
            
        }

        public void ResetCamera()
        {
            m_cameraTransform.position = new Vector3(0, 0, -10);
        }
    }
}

