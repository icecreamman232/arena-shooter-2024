using System;
using System.Collections;
using JustGame.Script.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace JustGame.Script.Level
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject m_unlockState;
        [SerializeField] private GameObject m_lockState;
        [SerializeField] private bool m_isUnlocked;
        [SerializeField] private Transform m_spawnPivot;
        [SerializeField] private Door m_connectDoor;
        [SerializeField] private UnityEvent m_eventToTrigger;

        private bool m_isTeleporting;

        public Vector2 SpawnPos => m_spawnPivot.position;
        
        private void Start()
        {
 
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

            StartCoroutine(TeleportRoutine(other.transform));
            
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
            m_isUnlocked = true;
            m_unlockState.SetActive(false);
            m_lockState.SetActive(true);
        }

        private IEnumerator TeleportRoutine(Transform player)
        {
            if (m_isTeleporting)
            {
                yield break;
            }

            m_isTeleporting = true;
            
            player.position = m_connectDoor.SpawnPos;
            
            m_isTeleporting = false;
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
