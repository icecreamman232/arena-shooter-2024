using System.Collections;
using JustGame.Script.Level;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private ProceduralGenerator m_proceduralGenerator;
    [SerializeField] private TeleportEvent m_teleportEvent;
    [SerializeField] private Transform m_player;
    private bool m_isTeleporting;
    
    private void Start()
    {
        m_teleportEvent.AddListener(OnReceiveTeleportEvent);
        m_proceduralGenerator.Initialize();
        m_proceduralGenerator.GenerateDungeon();
        
        m_proceduralGenerator.GeneratedRooms[0].Show();
        m_player.position = m_proceduralGenerator.GeneratedRooms[0].transform.position;
        
        for (int i = 1; i < m_proceduralGenerator.GeneratedRooms.Count; i++)
        {
            m_proceduralGenerator.GeneratedRooms[i].Hide();
        }
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
