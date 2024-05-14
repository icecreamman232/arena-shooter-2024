using UnityEngine;

namespace JustGame.Script.Level
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 m_roomSize;
        [SerializeField] private Vector2 m_roomOffset;
        [SerializeField] private SpriteMask m_mask;
        [SerializeField] private Door[] m_doorList;
        

        public void Show()
        {
            m_mask.enabled = false;
        }

        public void Hide()
        {
            m_mask.enabled = true;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + (Vector3)m_roomOffset, m_roomSize);
        }
    }
}
