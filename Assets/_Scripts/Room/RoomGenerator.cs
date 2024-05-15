using System;
using System.Collections;
using System.Collections.Generic;
using JustGame.Script.Level;
using JustGame.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    public Room StartRoom;
    [Header("Exit Room")]
    public Room EndRoomUp;
    public Room EndRoomDown;
    public Room EndRoomLeft;
    public Room EndRoomRight;
    
    [Header("Room List")]
    public Room[] RoomList;

    public bool IsGenerating;

    public int OffsetRoomPos;
    public int MaxNormalRoom = 15;
    public int CurrentNormalCounter = 0;

    public List<Room> GeneratedRooms;
    
    [ContextMenu("Generate")]
    private void Generate()
    {
        if (GeneratedRooms == null)
        {
            GeneratedRooms = new List<Room>();
        }
        else
        {
            GeneratedRooms.Clear();
        }
        StartCoroutine(GenerateRoutine());
    }

    private IEnumerator GenerateRoutine()
    {
        if (IsGenerating)
        {
            yield break;
        }
        IsGenerating = true;
        StopAllCoroutines();
        CurrentNormalCounter = 0;
        var room = Instantiate(StartRoom, Vector3.zero, Quaternion.identity);

        StartCoroutine(GenerateRoom(room));
        
        IsGenerating = false;
    }

    private IEnumerator GenerateRoom(Room curRoom)
    {
        yield return new WaitForSecondsRealtime(1);
        for (int i = 0; i < curRoom.DoorList.Length; i++)
        {
            if(curRoom.DoorList[i].ConnectDoor!= null) continue;
            if (CurrentNormalCounter >= MaxNormalRoom)
            {
                break;
            }

            var roomPrefab = GenerateRoomPrefab(curRoom.DoorList[i].DoorType);
            var pos = GetSpawnPosition(curRoom, i, roomPrefab);
            
            //If there's room spawned in this position (overlapped) we wont spawn this room
            if (!CanSpawn(pos, roomPrefab.Size))
            {
                roomPrefab = GetEndingRoomPrefab(curRoom.DoorList[i].DoorType);
                Debug.Log("GET ENDING ROOM PREFAB");
                //yield break;
            }
            
            //Create room
            var room = Instantiate(roomPrefab,Vector2.zero,Quaternion.identity);
            room.transform.position = pos;
            GeneratedRooms.Add(room);
            
            //Find door to connect
            Door door = null;
            for (int j = 0; j < room.DoorList.Length; j++)
            {
                switch (curRoom.DoorList[i].DoorType)
                {
                    case DoorType.GO_UP:
                        if (room.DoorList[j].DoorType == DoorType.GO_DOWN)
                        {
                            door = room.DoorList[j];
                        }

                        break;
                    case DoorType.GO_DOWN:
                        if (room.DoorList[j].DoorType == DoorType.GO_UP)
                        {
                            door = room.DoorList[j];
                        }

                        break;
                    case DoorType.GO_LEFT:
                        if (room.DoorList[j].DoorType == DoorType.GO_RIGHT)
                        {
                            door = room.DoorList[j];
                        }

                        break;
                    case DoorType.GO_RIGHT:
                        if (room.DoorList[j].DoorType == DoorType.GO_LEFT)
                        {
                            door = room.DoorList[j];
                        }

                        break;
                }
            }
            
            //Connect 2 doors
            curRoom.DoorList[i].ConnectDoor = door;
            door.ConnectDoor = curRoom.DoorList[i];
            
            Debug.Log($"Spawn room {CurrentNormalCounter.ToString()} connect to room {curRoom.gameObject.name}");
            Debug.Log("===========================");
            room.gameObject.name = $"Room {CurrentNormalCounter.ToString()}";
            CurrentNormalCounter++;
            
            StartCoroutine(GenerateRoom(room));
        }
    }

    private bool CanSpawn(Vector2 pos, Vector2 size)
    {
        for (int i = 0; i < GeneratedRooms.Count; i++)
        {
            if (MathHelpers.Is2RectCollided(pos, size,
                    GeneratedRooms[i].RoomPosition, GeneratedRooms[i].Size))
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 GetSpawnPosition(Room curRoom,int doorIndex, Room spawnedRoom)
    {
        var doorType = curRoom.DoorList[doorIndex].DoorType;
        var roomPos = curRoom.RoomPosition;
        var roomSize = curRoom.Size;
        
        Debug.Log($"<color=red>Inspect room {curRoom.gameObject.name}</color>");
        
        Vector3 pos = roomPos;
        switch (doorType)
        {
            case DoorType.GO_UP:
                pos.y += (OffsetRoomPos + roomSize.y);
                Debug.Log("<color=yellow>Change pos y up</color>");
                break;
            case DoorType.GO_DOWN:
                pos.y -= (OffsetRoomPos + roomSize.y);
                Debug.Log("<color=yellow>Change pos y down</color>");
                break;
            case DoorType.GO_LEFT:
                pos.x -= (OffsetRoomPos + roomSize.x);
                Debug.Log("<color=yellow>Change pos x left</color>");
                break;
            case DoorType.GO_RIGHT:
                pos.x += (OffsetRoomPos + roomSize.x);
                Debug.Log("<color=yellow>Change pos x right</color>");
                break;
        }
        
        return pos;
    }

    /// <summary>
    /// Find random room that has matched door to connect to
    /// </summary>
    /// <param name="doorType"></param>
    /// <returns></returns>
    private Room GenerateRoomPrefab(DoorType doorType)
    {
        var allRoom = Array.FindAll(RoomList, x => x.HasMatchType(doorType));
        
        return allRoom[Random.Range(0,allRoom.Length)];
    }

    private Room GetEndingRoomPrefab(DoorType doorType)
    {
        if (EndRoomDown.HasMatchType(doorType))
        {
            return EndRoomDown;
        }
        if (EndRoomUp.HasMatchType(doorType))
        {
            return EndRoomUp;
        }
        if (EndRoomLeft.HasMatchType(doorType))
        {
            return EndRoomLeft;
        }
        if (EndRoomRight.HasMatchType(doorType))
        {
            return EndRoomRight;
        }

        return null;
    }
}
