using JustGame.Script.Level;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Room m_startRoom;
    [SerializeField] private Room[] m_rooms;
    private void Start()
    {
        m_startRoom.Initialize();
        for (int i = 0; i < m_rooms.Length; i++)
        {
            m_rooms[i].Initialize();
        }
        
        m_startRoom.Show();
    }
}
