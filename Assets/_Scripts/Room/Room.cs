using System;
using UnityEngine;

namespace JustGame.Script.Level
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 m_roomSize;
        [SerializeField] private Vector2 m_roomOffset;
        [SerializeField] private SpriteMask m_mask;
        [SerializeField] private Door[] m_doorList;
        
        public void Initialize()
        {
            UpdateMaskSize();
        }

        public void Show()
        {
            m_mask.enabled = false;
        }

        public void Hide()
        {
            m_mask.enabled = true;
        }

        private void UpdateMaskSize()
        {
            Vector2 spriteSize = m_mask.sprite.bounds.size;
            Vector2 scale = m_roomSize / spriteSize;

            m_mask.enabled = true;
            m_mask.transform.localScale = new Vector3(scale.x, scale.y, 1);
            m_mask.transform.position += (Vector3)m_roomOffset;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + (Vector3)m_roomOffset, m_roomSize);
        }
    }
}
