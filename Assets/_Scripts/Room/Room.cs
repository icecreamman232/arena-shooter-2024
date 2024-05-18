using System;
using UnityEngine;

namespace JustGame.Script.Level
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 m_roomSize;
        [SerializeField] private Vector2 m_roomOffset;
        [SerializeField] private SpriteMask m_mask;
        [SerializeField] private BoxCollider2D m_roomBounds;
        [SerializeField] private Door[] m_doorList;
        
        public Vector2 Size => m_roomSize;

        public Vector2 RoomPosition => (Vector2)transform.position + m_roomOffset;
        public Door[] DoorList => m_doorList;

        public void SetBounds(bool isEnable) => m_roomBounds.enabled = isEnable;

        private void Awake()
        {
            m_roomBounds.size = m_roomSize;
            m_roomBounds.offset = m_roomOffset;
            UpdateMaskSize();
        }
        
        private void UpdateMaskSize()
        {
            Vector2 spriteSize = m_mask.sprite.bounds.size;
            Vector2 scale = m_roomSize / spriteSize;

            m_mask.enabled = true;
            m_mask.transform.localScale = new Vector3(scale.x, scale.y, 1);
            m_mask.transform.position += (Vector3)m_roomOffset;
        }
        

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
