using System.Collections;
using JustGame.Script.Level;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private TeleportEvent m_teleportEvent;
    [SerializeField] private Transform m_player;
    [SerializeField] private Room m_startRoom;
    [SerializeField] private Room[] m_rooms;
    private bool m_isTeleporting;
    
    private void Start()
    {
        m_startRoom.Initialize();
        for (int i = 0; i < m_rooms.Length; i++)
        {
            m_rooms[i].Initialize();
        }
        
        m_startRoom.Show();
        m_teleportEvent.AddListener(OnReceiveTeleportEvent);
    }

    private void OnDestroy()
    {
        m_teleportEvent.RemoveListener(OnReceiveTeleportEvent);
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
