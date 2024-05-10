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
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,m_connectDoor.transform.position);
        }
    }
}
