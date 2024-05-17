using System;
using System.Linq;
using UnityEngine;

namespace JustGame.Script.Level
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 m_roomSize;
        [SerializeField] private Vector2 m_roomOffset;
        [SerializeField] private SpriteMask m_mask;
        [SerializeField] private Door[] m_doorList;
        
        public Vector2 Size => m_roomSize;

        public Vector2 RoomPosition => (Vector2)transform.position + m_roomOffset;
        public Door[] DoorList => m_doorList;
        
        public void Show()
        {
            m_mask.enabled = false;
        }

        public void Hide()
        {
            m_mask.enabled = true;
        }

        public bool HasMatchType(DoorType type)
        {
            Door door = null;
            switch (type)
            {
                case DoorType.GO_UP:
                    door = DoorList.FirstOrDefault(x => x.DoorType == DoorType.GO_DOWN);
                    break;
                case DoorType.GO_DOWN:
                    door = DoorList.FirstOrDefault(x => x.DoorType == DoorType.GO_UP);
                    break;
                case DoorType.GO_LEFT:
                    door = DoorList.FirstOrDefault(x => x.DoorType == DoorType.GO_RIGHT);
                    break;
                case DoorType.GO_RIGHT:
                    door = DoorList.FirstOrDefault(x => x.DoorType == DoorType.GO_LEFT);
                    break;
            }

            return door != null;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + (Vector3)m_roomOffset, m_roomSize);
        }
    }
}
