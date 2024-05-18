using System;
using System.Collections;
using System.Collections.Generic;
using JustGame.Script.Level;
using JustGame.Script.Manager;
using JustGame.Scripts.Managers;
using UnityEngine;

public enum RoomKind
{
    START_ROOM,
    PLUS_SHAPE,
    T_DOWN,
    T_UP,
    T_LEFT,
    T_RIGHT,
    BOT_LEFT,
    BOT_RIGHT,
    TOP_LEFT,
    TOP_RIGHT,
    STRAGHT_TOP_BOT,
    STRAIGHT_LEFT_RIGHT,
}

public class ProceduralGenerator : MonoBehaviour
{
    [Header("Params")]
    public int RoomSize;
    public int MaxWidth;
    public int MaxHeight;
    public int StartingX;
    public int StartingY;
    public int MaxRoomForMainRoute;
    [Header("Start Rooms")] 
    public Room StartLeftRoomPrefab;
    public Room StartRightRoomPrefab;
    public Room StartUpRoomPrefab;
    public Room StartDownRoomPrefab;
    [Header("Corner Room")] 
    public Room TopLeftCornerPrefab;
    public Room TopRightCornerPrefab;
    public Room BotLeftCornerPrefab;
    public Room BotRightCornerPrefab;
    [Header("T Room")] 
    public Room T_UpRoomPrefab;
    public Room T_DownRoomPrefab;
    public Room T_LeftRoomPrefab;
    public Room T_RightRoomPrefab;
    public Room CrossShapeRoomPrefab;
    [Header("Straight Room")] 
    public Room HorizontalStraightRoomPrefab;
    public Room VerticalStraightRoomPrefab;
    [Header("Ending Room")] 
    public Room EndingUpRoomPrefab;
    public Room EndingDownRoomPrefab;
    public Room EndingLeftRoomPrefab;
    public Room EndingRightRoomPrefab;
    
    //Step 1: Generate whole dungeon with raw data
        //Step 1.a: Generate main route from starting room to ending room
        //Step 1.b: Random pick rooms in main route and generate random sub-room connected to it
    
    //Step 2:Using raw data to create room game object


    public int[,] CoordArr;
    public List<(int x, int y)> OccupiedCellList;
    public List<Room> GeneratedRooms;

    private bool m_isCreatingRoom;

    private void Start()
    {
        CoordArr = new int[MaxWidth, MaxHeight];
        OccupiedCellList = new List<(int x, int y)>();
        GeneratedRooms = new List<Room>();
        ResetCoordinateArray();
    }

    private void ResetCoordinateArray()
    {
        for (int i = 0; i < CoordArr.GetLength(0); i++)
        {
            for (int j = 0; j < CoordArr.GetLength(1); j++)
            {
                CoordArr[i, j] = 0;
            }
        }   
    }

    [ContextMenu("Generate")]
    private void CreateRawRoomData()
    {
        ResetCoordinateArray();
        OccupiedCellList.Clear();
        
        CoordArr[StartingX, StartingY] = 1;
        OccupiedCellList.Add((StartingX,StartingY));
        Debug.Log($"Add room at {StartingX} {StartingY}");
        
        int curX = StartingX;
        int curY = StartingY;
        int roomCounter = 1;
        int countToBreak = 0;
        var lastDirection = 2;
        while(roomCounter < MaxRoomForMainRoute)
        {
            var coord = (0, 0);
            int numberNeighbor = 0;
            var randomDirection = MathHelpers.ChooseRandomValueFrom(0, 1, 2, 3);
            if (lastDirection == randomDirection)
            {
                continue;
            }
            lastDirection = randomDirection;
            switch (randomDirection)
            {
                //Left
                case 0:
                    coord = (curX - 1, curY);
                    numberNeighbor = GetNumberOfNeighbor(coord);
                    if (IsValidAndEmptyCoord(coord) && numberNeighbor < 2)
                    {
                        curX -= 1;
                        CoordArr[curX, curY] = 1;
                        OccupiedCellList.Add((curX,curY));
                        Debug.Log($"Add room at {curX} {curY}");
                        roomCounter++;
                    }
                    break;
                //Right
                case 1:
                    coord = (curX + 1, curY);
                    numberNeighbor = GetNumberOfNeighbor(coord);
                    if (IsValidAndEmptyCoord(coord) && numberNeighbor < 2)
                    {
                        curX += 1;
                        CoordArr[curX, curY] = 1;
                        OccupiedCellList.Add((curX,curY));
                        Debug.Log($"Add room at {curX} {curY}");
                        roomCounter++;
                    }
                    break;
                //Up
                case 2:
                    coord = (curX , curY + 1);
                    numberNeighbor = GetNumberOfNeighbor(coord);
                    if (IsValidAndEmptyCoord(coord) && numberNeighbor < 2)
                    {
                        curY += 1;
                        CoordArr[curX, curY] = 1;
                        OccupiedCellList.Add((curX,curY));
                        Debug.Log($"Add room at {curX} {curY}");
                        roomCounter++;
                    }
                    break;
                //Down
                case 3:
                    coord = (curX, curY - 1);
                    numberNeighbor = GetNumberOfNeighbor(coord);
                    if (IsValidAndEmptyCoord(coord) && numberNeighbor < 2)
                    {
                        curY -= 1;
                        CoordArr[curX, curY] = 1;
                        OccupiedCellList.Add((curX,curY));
                        Debug.Log($"Add room at {curX} {curY}");
                        roomCounter++;
                    }
                    break;
            }
        }

        CreateRoomData();

        ConnectDoors();
    }

    private int GetNumberOfNeighbor((int x, int y) coord)
    {
        int count = 0;
        
        //Left
        if (IsValidCoord((coord.x - 1, coord.y)))
        {
            if (CoordArr[coord.x - 1, coord.y] == 1)
            {
                count++;
            }
            
        }
        //Right
        if (IsValidCoord((coord.x + 1, coord.y)))
        {
            if (CoordArr[coord.x + 1, coord.y] == 1)
            {
                count++;
            }
        }
        //Up
        if (IsValidCoord((coord.x, coord.y + 1)))
        {
            
            if (CoordArr[coord.x, coord.y + 1] == 1)
            {
                count++;
            }
        }
        //Down
        if (IsValidCoord((coord.x, coord.y -1)))
        {
            if (CoordArr[coord.x, coord.y -1] == 1)
            {
                count++;
            }
        }
        
        
        // //Top Left
        // if (IsValidCoord((coord.x - 1, coord.y + 1)))
        // {
        //     if (CoordArr[coord.x - 1, coord.y + 1] == 1)
        //     {
        //         count++;
        //     }
        // }
        //
        // //Top Right
        // if (IsValidCoord((coord.x + 1, coord.y + 1)))
        // {
        //     if (CoordArr[coord.x + 1, coord.y + 1] == 1)
        //     {
        //         count++;
        //     }
        // }
        //
        // //Bot Left
        // if (IsValidCoord((coord.x - 1, coord.y - 1)))
        // {
        //     if (CoordArr[coord.x - 1, coord.y - 1] == 1)
        //     {
        //         count++;
        //     }
        // }
        //
        // //Bot Right
        // if (IsValidCoord((coord.x + 1, coord.y - 1)))
        // {
        //     if (CoordArr[coord.x + 1, coord.y - 1] == 1)
        //     {
        //         count++;
        //     }
        // }
        
        return count;
    }
    
    /// <summary>
    /// Based on raw data, we transfer it into room data
    /// </summary>
    private void CreateRoomData()
    {
        for (int i = 0; i < GeneratedRooms.Count; i++)
        {
            Destroy(GeneratedRooms[i].gameObject);
        }
        GeneratedRooms.Clear();
        
        //Create starting room
        var startPrefab = ChooseStartRoom(OccupiedCellList[0]);
        var staringRoom = Instantiate(startPrefab, GetWorldPosFromCoord(OccupiedCellList[0]), Quaternion.identity);
        GeneratedRooms.Add(staringRoom);
        
        for (int i = 1; i < OccupiedCellList.Count; i++)
        {
            var numberNeighbor = GetNumberOfNeighbor(OccupiedCellList[i]);
            var roomPrefab = ChoosePrefab(numberNeighbor, OccupiedCellList[i]);
            var room = Instantiate(roomPrefab, GetWorldPosFromCoord(OccupiedCellList[i]), Quaternion.identity);
            GeneratedRooms.Add(room);
        }
    }

    private Room ChoosePrefab(int numberNeighbor, (int x, int y)coord)
    {
        switch (numberNeighbor)
        {
            case 1:
                return ChooseEndingRoom(coord);
            case 2:
                return ChooseCornerRoom(coord);
            case 3:
                return Choose_T_Shape_Room(coord);
            case 4:
                return CrossShapeRoomPrefab;
        }
        return null;
    }
    
    private Room ChooseStartRoom((int x, int y) coord)
    {
        var left = (coord.x - 1, coord.y);
        var right = (coord.x +1, coord.y);
        var up = (coord.x, coord.y + 1);
        var down = (coord.x, coord.y - 1);
        
        if (IsOccupied(left))
        {
            return StartLeftRoomPrefab;
        }
        if (IsOccupied(right))
        {
            return StartRightRoomPrefab;
        }
        if (IsOccupied(up))
        {
            return StartUpRoomPrefab;
        }
        if (IsOccupied(down))
        {
            return StartDownRoomPrefab;
        }
            
        
        return null;
    }

    private Room ChooseEndingRoom((int x, int y) coord)
    {
        var left = (coord.x - 1, coord.y);
        var right = (coord.x +1, coord.y);
        var up = (coord.x, coord.y + 1);
        var down = (coord.x, coord.y - 1);

        if (IsOccupied(left))
        {
            return EndingLeftRoomPrefab;
        }
        if (IsOccupied(right))
        {
            return EndingRightRoomPrefab;
        }
        if (IsOccupied(up))
        {
            return EndingUpRoomPrefab;
        }
        if (IsOccupied(down))
        {
            return EndingDownRoomPrefab;
        }
            
        
        return null;
    }

    private Room ChooseCornerRoom((int x, int y) coord)
    {
        var left = (coord.x - 1, coord.y);
        var right = (coord.x +1, coord.y);
        var up = (coord.x, coord.y + 1);
        var down = (coord.x, coord.y - 1);
        
        
        if (IsOccupied(down) && IsOccupied(right))
        {
            return TopLeftCornerPrefab;
        }
        if (IsOccupied(down) && IsOccupied(left))
        {
            return TopRightCornerPrefab;
        }
        if (IsOccupied(up) && IsOccupied(right))
        {
            return BotLeftCornerPrefab;
        }
        if (IsOccupied(up) && IsOccupied(left))
        {
            return BotRightCornerPrefab;
        }
        if (IsOccupied(left) && IsOccupied(right))
        {
            return HorizontalStraightRoomPrefab;
        }
        if (IsOccupied(up) && IsOccupied(down))
        {
            return VerticalStraightRoomPrefab;
        }
        
        Debug.Log("NULL Corner");
        return null;
    }

    private Room Choose_T_Shape_Room((int x, int y) coord)
    {
        var left = (coord.x - 1, coord.y);
        var right = (coord.x +1, coord.y);
        var up = (coord.x, coord.y + 1);
        var down = (coord.x, coord.y - 1);

        if (IsOccupied(left) && IsOccupied(right) && IsOccupied(up))
        {
            return T_UpRoomPrefab;
        }
        if (IsOccupied(left) && IsOccupied(right) && IsOccupied(down))
        {
            return T_DownRoomPrefab;
        }
        if (IsOccupied(up) && IsOccupied(down) && IsOccupied(left))
        {
            return T_LeftRoomPrefab;
        }
        if (IsOccupied(up) && IsOccupied(down) && IsOccupied(right))
        {
            return T_RightRoomPrefab;
        }
        
        Debug.Log("NULL T");
        return null;
    }
    
    private Vector3 GetWorldPosFromCoord((int x, int y) coord)
    {
        return new Vector3(coord.x * RoomSize, coord.y * RoomSize, 0);
    }
    
    private bool IsValidAndEmptyCoord((int x, int y)coord)
    {
        if (coord.x < 0 || coord.x >= MaxWidth) return false;
        if (coord.y < 0 || coord.y >= MaxHeight) return false;

        if (CoordArr[coord.x, coord.y] == 1) return false;
        
        return true;
    }

    private bool IsOccupied((int x,int y) coord)
    {
        if (coord.x < 0 || coord.x >= MaxWidth) return false;
        if (coord.y < 0 || coord.y >= MaxHeight) return false;
        
        return (CoordArr[coord.x, coord.y] == 1);
    }

    private bool IsValidCoord((int x,int y) coord)
    {
        if (coord.x < 0 || coord.x >= MaxWidth) return false;
        if (coord.y < 0 || coord.y >= MaxHeight) return false;
        return true;
    }

    private void ConnectDoors()
    {
        for (int i = 0; i < GeneratedRooms.Count; i++)
        {
            for (int j = 0; j < GeneratedRooms[i].DoorList.Length; j++)
            {
                var door = GeneratedRooms[i].DoorList[j];
                var direction = Vector2.zero;
                switch (door.DoorType)
                {
                    case DoorType.GO_UP:
                        direction = Vector2.up;
                        break;
                    case DoorType.GO_DOWN:
                        direction = Vector2.down;
                        break;
                    case DoorType.GO_LEFT:
                        direction = Vector2.left;
                        break;
                    case DoorType.GO_RIGHT:
                        direction = Vector2.right;
                        break;
                }
                GeneratedRooms[i].SetBounds(false);

                var hit = Physics2D.Raycast(door.transform.position, direction, 30, LayerManager.RoomBoundsMask);
                if (hit.collider != null)
                {
                    var targetRoom = hit.transform.parent.GetComponent<Room>();
                    for (int k = 0; k < targetRoom.DoorList.Length; k++)
                    {
                        if (IsValidDoor(door, targetRoom.DoorList[k]))
                        {
                            door.ConnectDoor = targetRoom.DoorList[k];
                            targetRoom.DoorList[k].ConnectDoor = door;
                        }
                    }
                }
                else
                {
                    Debug.Log("NOT HIT");
                }
                GeneratedRooms[i].SetBounds(true);
            }
        }
    }


    private bool IsValidDoor(Door currentDoor, Door targetDoor)
    {
        if (currentDoor.DoorType == DoorType.GO_UP && targetDoor.DoorType == DoorType.GO_DOWN)
        {
            return true;
        }
        if (currentDoor.DoorType == DoorType.GO_DOWN && targetDoor.DoorType == DoorType.GO_UP)
        {
            return true;
        }
        if (currentDoor.DoorType == DoorType.GO_LEFT && targetDoor.DoorType == DoorType.GO_RIGHT)
        {
            return true;
        }
        if (currentDoor.DoorType == DoorType.GO_RIGHT && targetDoor.DoorType == DoorType.GO_LEFT)
        {
            return true;
        }

        return false;
    }
    
    

    private void OnDrawGizmos()
    {
        if (CoordArr == null) return;
        
        for (int i = 0; i < CoordArr.GetLength(0); i++)
        {
            for (int j = 0; j < CoordArr.GetLength(1); j++)
            {
                Gizmos.color = CoordArr[i, j] == 0 ? Color.yellow : Color.green;
                Gizmos.DrawCube(new Vector2(i,j),Vector3.one * 0.3f);
            }
        }   
    }
}
