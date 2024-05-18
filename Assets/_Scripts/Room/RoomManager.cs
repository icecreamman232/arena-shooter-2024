using System.Collections;
using JustGame.Script.Manager;
using JustGame.Scripts.ScriptableEvent;
using UnityEngine;

namespace JustGame.Script.Level
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private int m_curDungeonLevel;
        [SerializeField] private ProceduralGenerator m_proceduralGenerator;
        [SerializeField] private Transform m_player;
        [Header("Events")]
        [SerializeField] private TeleportEvent m_teleportEvent;
        [SerializeField] private BoolEvent m_fadeScreenEvent;
        
        private bool m_isTeleporting;
        
        private IEnumerator Start()
        {
            m_fadeScreenEvent.Raise(true);
            TimeManager.Instance.Pause();
            
            m_curDungeonLevel = 1;
            m_teleportEvent.AddListener(OnReceiveTeleportEvent);
            m_proceduralGenerator.Initialize();
            m_proceduralGenerator.GenerateDungeon();
            
            m_proceduralGenerator.GeneratedRooms[0].Show();
            m_player.position = m_proceduralGenerator.GeneratedRooms[0].transform.position;
            
            for (int i = 1; i < m_proceduralGenerator.GeneratedRooms.Count; i++)
            {
                m_proceduralGenerator.GeneratedRooms[i].Hide();
            }

            yield return new WaitForSecondsRealtime(2f);
            
            TimeManager.Instance.Unpause();
            m_fadeScreenEvent.Raise(false);
        }
    
        private void OnDestroy()
        {
            m_teleportEvent.RemoveListener(OnReceiveTeleportEvent);
        }

        private void OnGoToNextDungeon()
        {
            m_curDungeonLevel++;
        }
    
        private void OnReceiveTeleportEvent(Door cur, Door next)
        {
            StartCoroutine(TeleportToRoom(cur, next));
        }
    
        private IEnumerator TeleportToRoom(Door cur, Door next)
        {
            if (m_isTeleporting)
            {
                yield break;
            }
    
            m_isTeleporting = true;
            var nextRoom = next.Room;
            var curRoom = cur.Room;
            curRoom.Hide();
            nextRoom.Show();
            m_player.position = next.SpawnPos;
            yield return new WaitForSeconds(1);
            m_isTeleporting = false;
        }
    }
}

