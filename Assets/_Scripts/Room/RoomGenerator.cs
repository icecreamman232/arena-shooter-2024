using System;
using System.Collections;
using JustGame.Script.Level;
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

    public int MaxLength = 5;
    public int MaxNormalRoom = 15;
    public int CurrentNormalCounter = 0;
    
    [ContextMenu("Generate")]
    private void Generate()
    {
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
            
            var pos = curRoom.transform.position + GetSpawnPosition(curRoom.DoorList[i].DoorType,curRoom.Size);
            var room = Instantiate(GenerateNextRoom(curRoom.DoorList[i].DoorType),pos,Quaternion.identity);
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
            CurrentNormalCounter++;
            
            StartCoroutine(GenerateRoom(room));
        }
    }

    private Vector3 GetSpawnPosition(DoorType type, Vector2 roomSize)
    {
        Vector3 pos = Vector3.zero;
        switch (type)
        {
            case DoorType.GO_UP:
                pos.y += roomSize.y;
                break;
            case DoorType.GO_DOWN:
                pos.y -= roomSize.y;
                break;
            case DoorType.GO_LEFT:
                pos.x -= roomSize.x;
                break;
            case DoorType.GO_RIGHT:
                pos.x += roomSize.x;
                break;
        }

        return pos;
    }

    private Room GenerateNextRoom(DoorType doorType)
    {
        var room = Array.FindAll(RoomList, x => x.HasMatchType(doorType));

        return room[Random.Range(0,room.Length)];
    }
}
