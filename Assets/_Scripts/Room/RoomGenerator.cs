using System;
using System.Collections;
using System.Collections.Generic;
using JustGame.Script.Level;
using JustGame.Scripts.Managers;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public enum RoomType
{
    Four_Direction,
    Left_Right,
    Top_Bot,
    Left_Bot,
    Left_Top,
    Right_Bot,
    Right_Top,
    Left,
    Right,
    Top,
    Bot,
}

public class RoomGenerator : MonoBehaviour
{
    public Room StartRoom;
    [Header("Exit Room")]
    public Room EndRoomUp;
    public Room EndRoomDown;
    public Room EndRoomLeft;
    public Room EndRoomRight;

    [Header("Room List")] 
    public Room m_topLeftCornerRoom;
    public Room m_topRightCornerRoom;
    public Room m_botLeftCornerRoom;
    public Room m_botRightCornerRoom;
    public Room m_topBotRoom;
    public Room m_leftRightRoom;
    public Room m_plusRoom;
    
    //T shape
    public Room m_T_Up_Room;
    public Room m_T_Down_Room;
    public Room m_T_Left_Room;
    public Room m_T_Right_Room;
    
    public Room[] RoomList;

    public bool IsGenerating;

    public int OffsetRoomPos;
    public int MaxNormalRoom = 15;
    public int CurrentNormalCounter = 0;

    public List<Room> GeneratedRooms;

    public List<RoomType> GenerateRoomType;
    
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
        
        if (GenerateRoomType == null)
        {
            GenerateRoomType = new List<RoomType>();
        }
        else
        {
            GenerateRoomType.Clear();
        }

        StartCoroutine(CreateRoomRoutine());
    }
    
    [SerializeField] private int[,] worldArr;
    [SerializeField] private List<(int xCoord, int yCoord)> m_roomCoordList;

    [ContextMenu("Test")]
    private void CreateRoomPlan()
    {
        worldArr = new int[5, 5];
        m_roomCoordList = new List<(int, int)>();
        
        for (int i = 0; i < worldArr.GetLength(0); i++)
        {
            for (int j = 0; j < worldArr.GetLength(1); j++)
            {
                worldArr[i, j] = -1;
            }
        }

        int x = 2;
        int y = 0;
        worldArr[x, y] = 1;
        m_roomCoordList.Add((2,0));
        m_roomCoordList.Add((2,1));
        
        // //============================FORCE VALUES
        // m_roomCoordList.Add((2,1));
        // m_roomCoordList.Add((1,1));
        // m_roomCoordList.Add((0,1));
        // m_roomCoordList.Add((0,0));
        // m_roomCoordList.Add((1,0));
        //
        //
        // return;
        //
        //
        //
        //
        
        int direction = -1;
        int counter = 1;
        int breakCounter = 0;
        int lastDirection = 1;
        
        while (counter < MaxNormalRoom || breakCounter < 4)
        {
            if (lastDirection == 1)
            {
                direction =  MathHelpers.ChooseRandomValueFrom(0, 2, 3);
            }
            else if (lastDirection == 0)
            {
                direction =  MathHelpers.ChooseRandomValueFrom(1, 2, 3);
            }
            else if (lastDirection == 2)
            {
                direction =  MathHelpers.ChooseRandomValueFrom(0, 1, 3);
            }
            else if (lastDirection == 3)
            {
                direction =  MathHelpers.ChooseRandomValueFrom(0, 1, 2);
            }

            lastDirection = direction;
            
            switch (direction)
            {
                //left
                case 0:
                    var tempLeft = x - 1;
                    if (tempLeft < 0 || tempLeft >= 5)
                    {
                        breakCounter++;
                        break;
                    }
                    if (worldArr[tempLeft, y] == -1)
                    {
                        worldArr[tempLeft, y] = 1;
                        m_roomCoordList.Add((tempLeft,y));
                        x = tempLeft;
                        counter++;
                    }
                    else
                    {
                        breakCounter++;
                    }
                    break;
                //top
                case 1:
                    var tempTop = y - 1;
                    if (tempTop < 0 || tempTop >= 5)
                    {
                        breakCounter++;
                        break;
                    }
                    if (worldArr[x, tempTop] == -1)
                    {
                        worldArr[x, tempTop] = 1;
                        m_roomCoordList.Add((x,tempTop));
                        y = tempTop;
                        counter++;
                    }
                    else
                    {
                        breakCounter++;
                    }
                    break;
                //right
                case 2:
                    var tempRight = x + 1;
                    if (tempRight < 0 || tempRight >= 5)
                    {
                        breakCounter++;
                        break;
                    }
                    if (worldArr[tempRight, y] == -1)
                    {
                        worldArr[tempRight, y] = 1;
                        m_roomCoordList.Add((tempRight,y));
                        x = tempRight;
                        counter++;
                    }
                    else
                    {
                        breakCounter++;
                    }
                    break;
                //bot
                case 3:
                    var tempBot = y + 1;
                    if (tempBot < 0 || tempBot >= 5)
                    {
                        breakCounter++;
                        break;
                    }
                    if (worldArr[x, tempBot] == -1)
                    {
                        worldArr[x, tempBot] = 1;
                        m_roomCoordList.Add((x,tempBot));
                        y = tempBot;
                        counter++;
                    }
                    else
                    {
                        breakCounter++;
                    }
                    break;
            }
        }
        Debug.Log("Done");
    }


    private IEnumerator CreateRoomRoutine()
    {
        if (IsGenerating)
        {
            yield break;
        }

        IsGenerating = true;
        
        CreateRoomPlan();

        yield return new WaitForSecondsRealtime(1);
        //Create start room
        var startRoom = Instantiate(StartRoom, Vector3.zero, quaternion.identity);
        GeneratedRooms.Add(startRoom);
        GenerateRoomType.Add(RoomType.Top);
        yield return new WaitForSecondsRealtime(1.0f);

        CreateRoom();

        IsGenerating = false;
    }


    private void CreateRoom()
    {
        bool hasStartingRoomAsNeighbor = false;
        //Run from 1 to exclude starting room
        for (int i = 1; i < m_roomCoordList.Count; i++)
        {
            var x = m_roomCoordList[i].xCoord;
            var y = m_roomCoordList[i].yCoord;
            var top =  y + 1;
            var bot = y - 1;
            var left = x - 1;
            var right = x + 1;
            
            //T up
            if (HasRoomAtCoord((x,top),(x,y)) 
                && HasRoomAtCoord((left,y),(x,y))
                && HasRoomAtCoord((right,y),(x,y)))
            {
                var tUpRoom = Instantiate(m_T_Up_Room, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(tUpRoom);
                Debug.Log($"Create T_UP at i{x} j{y}");
            }
            //T down
            else if ( HasRoomAtCoord((x,bot),(x,y))
                     && HasRoomAtCoord((left,y),(x,y))
                     && HasRoomAtCoord((right,y),(x,y)))
            {
                var tDownRoom = Instantiate(m_T_Down_Room, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(tDownRoom);
                Debug.Log($"Create T_DOWN at i{x} j{y}");
            }
            //T right
            else if (HasRoomAtCoord((right,y),(x,y))
                     && HasRoomAtCoord((x,top),(x,y))
                     && HasRoomAtCoord((x,bot),(x,y)))
            {
                var tRightRoom = Instantiate(m_T_Right_Room, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(tRightRoom);
                Debug.Log($"Create T_RIGHT at i{x} j{y}");
            }
            //T left
            else if (HasRoomAtCoord((left,y),(x,y))
                     && HasRoomAtCoord((x,top),(x,y))
                     && HasRoomAtCoord((x,bot),(x,y)))
            {
                var tLeftRoom = Instantiate(m_T_Left_Room, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(tLeftRoom);
                Debug.Log($"Create T_RIGHT at i{x} j{y}");
            }
            //Top Bot
            else if (HasRoomAtCoord((x,top),(x,y))
                     && HasRoomAtCoord((x,bot),(x,y)))
            {
                var topBotRoom = Instantiate(m_topBotRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(topBotRoom);
                Debug.Log($"Create TOP_BOT at i{x} j{y}");
            }
            //Left Right
            else if (HasRoomAtCoord((left,y),(x,y))
                     && HasRoomAtCoord((right,y),(x,y)))
            {
                var leftRightRoom = Instantiate(m_leftRightRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(leftRightRoom);
                Debug.Log($"Create LEFT_RIGHT at i{x} j{y}");
            }
            //Check if this is top left corner
            else if (HasRoomAtCoord((right,y),(x,y))
                     && HasRoomAtCoord((x,bot),(x,y))
                     && !IsStartRoom((left,y)))
            {
                var topLeftRoom = Instantiate(m_topLeftCornerRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(topLeftRoom);
                Debug.Log($"Create TOP_LEFT corner at i{x} j{y}");
            }
            //Check if is top right corner
            else if (HasRoomAtCoord((left,y),(x,y))
                     && HasRoomAtCoord((x,bot),(x,y))
                     && !IsStartRoom((right,y)))
            {
                var topRightRoom = Instantiate(m_topRightCornerRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(topRightRoom);
                Debug.Log($"Create TOP_RIGHT corner at i{x} j{y}");
            }
            //Bot left corner
            else if (HasRoomAtCoord((right,y),(x,y))
                     && HasRoomAtCoord((x,top),(x,y))
                     && !IsStartRoom((left,y)))
            {
                var botLeftRoom = Instantiate(m_botLeftCornerRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(botLeftRoom);
                Debug.Log($"Create BOT_LEFT corner at i{x} j{y}");
            }
            //Bot Right corner
            else if (HasRoomAtCoord((left,y),(x,y))
                     && HasRoomAtCoord((x,top),(x,y))
                     && !IsStartRoom((left,y)))
            {
                var botRightRoom = Instantiate(m_botRightCornerRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(botRightRoom);
                Debug.Log($"Create BOT_RIGHT corner at i{x} j{y}");
            }
            //Plus shape
            else if(HasRoomAtCoord((left,y),(x,y))
                    && HasRoomAtCoord((right,y),(x,y))
                    && HasRoomAtCoord((x,top),(x,y))
                    && HasRoomAtCoord((x,bot),(x,y)))
            {
                var plusRoom = Instantiate(m_plusRoom, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(plusRoom);
                Debug.Log($"Create PLUS at i{x} j{y}");
            }
            //Ending room
            else
            {
                var endRoom = Instantiate(EndRoomUp, GetPosFromCoord(x, y), Quaternion.identity);
                GeneratedRooms.Add(endRoom);
                Debug.Log($"Create Ending at i{x} j{y}");
            }
        }
    }

    private bool IsStartRoom((int x, int y) coord)
    {
        return (coord ==(2,0));
    }
    private bool HasRoomAtCoord((int x ,int y) coord, (int x,int y) current)
    {
        if (coord.x < 0 || coord.x >= 5) return false;
        if (coord.y < 0 || coord.y >= 5) return false;
        
        for (int i = 0; i < m_roomCoordList.Count; i++)
        {
            if (m_roomCoordList[i] == coord)
            {
                if (coord == (2, 0)
                    && current != (2, 1))
                {
                    return false;
                }
                return true;
            }
        }

        return false;
    }

    
    private Vector2 GetPosFromCoord(int x, int y)
    {
        var orgX = 2;
        var orgY = 0;

        var diffX = x - orgX;
        var diffY = y - orgY;

        return new Vector2(diffX * 23, diffY * 23);
    }
    
    private void OnDrawGizmos()
    {
        if (worldArr == null) return;
        
        for (int i = 0; i < worldArr.GetLength(0); i++)
        {
            for (int j = 0; j < worldArr.GetLength(1); j++)
            {
                if (worldArr[i, j] == -1)
                {
                    Gizmos.color = Color.yellow;
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawCube(new Vector3(i,j),Vector3.one * 0.2f);
            }
        }
    }
}
