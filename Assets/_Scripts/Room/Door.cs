using System;
using System.Collections;
using JustGame.Script.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace JustGame.Script.Level
{
    public enum DoorType
    {
        GO_UP,
        GO_DOWN,
        GO_LEFT,
        GO_RIGHT,
    }
    public class Door : MonoBehaviour
    {
        [SerializeField] private DoorType m_doorType;
        [SerializeField] private TeleportEvent m_teleportEvent;
        [SerializeField] private GameObject m_unlockState;
        [SerializeField] private GameObject m_lockState;
        [SerializeField] private bool m_isUnlocked;
        [SerializeField] private Transform m_spawnPivot;
        [SerializeField] private Door m_connectDoor;
        [SerializeField] private UnityEvent m_eventToTrigger;

        private bool m_isTeleporting;
        private Room m_room;

        public Door ConnectDoor
        {
            get
            {
                return m_connectDoor;
            }
            set
            {
                m_connectDoor = value;
            }
        }

        public DoorType DoorType => m_doorType;
        
        public Vector2 SpawnPos => m_spawnPivot.position;
        public Room Room => m_room;
        
        private void Start()
        {
            m_room = GetComponentInParent<Room>();
            if (m_isUnlocked)
            {
                Unlock();
            }
            else
            {
                Lock();
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!m_isUnlocked) return;
            if (m_connectDoor == null) return;

            if (other.gameObject.layer != LayerManager.PlayerLayer) return;

            m_teleportEvent.Raise(this,m_connectDoor);
            
            m_eventToTrigger?.Invoke();
        }

        public void Unlock()
        {
            m_isUnlocked = true;
            m_unlockState.SetActive(true);
            m_lockState.SetActive(false);
        }

        public void Lock()
        {
            m_isUnlocked = false;
            m_unlockState.SetActive(false);
            m_lockState.SetActive(true);
        }

        private void OnDrawGizmos()
        {
            if (m_connectDoor == null) return;
            var arrowSize = 0.3f;
            Gizmos.color = Color.green;
            Vector3 direction = (m_connectDoor.transform.position - transform.position).normalized;
            
            Gizmos.DrawLine(transform.position,m_connectDoor.transform.position);
            
            Quaternion rightRotation = Quaternion.Euler(0f, 0, 135f);
            Quaternion leftRotation = Quaternion.Euler(0f, 0, -135f);
            Vector3 rightArrowTip = m_connectDoor.transform.position - direction * arrowSize + (rightRotation * direction) * arrowSize;
            Vector3 leftArrowTip = m_connectDoor.transform.position - direction * arrowSize + (leftRotation * direction) * arrowSize;

            Gizmos.DrawLine(m_connectDoor.transform.position, rightArrowTip);
            Gizmos.DrawLine(m_connectDoor.transform.position, leftArrowTip);
        }
    }
}
