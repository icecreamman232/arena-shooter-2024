using System;
using System.Collections;
using System.Collections.Generic;
using JustGame.Script.Level;
using JustGame.Scripts.Managers;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
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

        StartCoroutine(CreateRooms());
    }
    
    [SerializeField] private int[,] plan;

    [ContextMenu("Test")]
    private void CreateRoomPlan()
    {
        plan = new int[5, 5];

        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                plan[i, j] = -1;
            }
        }

        int x = 2;
        int y = 0;
        plan[x, y] = 1;

        int direction = -1;
        int counter = 0;
        int breakCounter = 0;
        while (counter < MaxNormalRoom || breakCounter < 4)
        {
            direction =  MathHelpers.ChooseRandomValueFrom(0, 1, 2, 3);
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
                    if (plan[tempLeft, y] == -1)
                    {
                        plan[tempLeft, y] = 1;
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
                    if (plan[x, tempTop] == -1)
                    {
                        plan[x, tempTop] = 1;
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
                    if (plan[tempRight, y] == -1)
                    {
                        plan[tempRight, y] = 1;
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
                    if (plan[x, tempBot] == -1)
                    {
                        plan[x, tempBot] = 1;
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


    private IEnumerator CreateRooms()
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
        yield return new WaitForSecondsRealtime(1.0f);

        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if(i == 2 && j == 0) continue;
                
                if (plan[i, j] == 1)
                {
                    var top = j + 1;
                    var bot = j - 1;
                    var left = i - 1;
                    var right = i + 1;
                    
                    //T up
                    if (top < 5 && left >= 0 && right < 5 && plan[i,top] == 1 && plan[left,j]==1 && plan[right,j]==1)
                    {
                        var tUpRoom = Instantiate(m_T_Up_Room, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(tUpRoom);
                        Debug.Log($"Create T_UP at i{i} j{j}");
                    }
                    //T down
                    else if (bot >= 0 && left >= 0 && right < 5 && plan[i, bot] == 1 && plan[left, j] == 1 &&
                             plan[right, j] == 1)
                    {
                        var tDownRoom = Instantiate(m_T_Down_Room, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(tDownRoom);
                        Debug.Log($"Create T_DOWN at i{i} j{j}");
                    }
                    //T right
                    else if (right < 5 && top < 5 && bot >=0 && plan[right,j]==1 && plan[i,top]==1 && plan[i,bot]==1)
                    {
                        var tRightRoom = Instantiate(m_T_Right_Room, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(tRightRoom);
                        Debug.Log($"Create T_RIGHT at i{i} j{j}");
                    }
                    else if (left >= 0 && top < 5 && bot >=0 && plan[left,j]==1 && plan[i,top]==1 && plan[i,bot]==1)
                    {
                        var tLeftRoom = Instantiate(m_T_Left_Room, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(tLeftRoom);
                        Debug.Log($"Create T_RIGHT at i{i} j{j}");
                    }
                    else if (top < 5 && bot >= 0 && plan[i,top] == 1 && plan[i,bot]==1)
                    {
                        var topBotRoom = Instantiate(m_topBotRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(topBotRoom);
                        Debug.Log($"Create TOP_BOT at i{i} j{j}");
                    }
                    else if (left >= 0 && right < 5 && plan[left,j] == 1 && plan[right,j] == 1)
                    {
                        var leftRightRoom = Instantiate(m_leftRightRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(leftRightRoom);
                        Debug.Log($"Create LEFT_RIGHT at i{i} j{j}");
                    }
                    //Check if this is top left corner
                    else if (bot >= 0 && right < 5 && plan[right, j] == 1 && plan[i,bot] == 1)
                    {
                        var topLeftRoom = Instantiate(m_topLeftCornerRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(topLeftRoom);
                        Debug.Log($"Create TOP_LEFT corner at i{i} j{j}");
                    }
                    //Check if is top right corner
                    else if (left >= 0 && bot >= 0 && plan[left,j] == 1 && plan[i,bot] == 1)
                    {
                        var topRightRoom = Instantiate(m_topRightCornerRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(topRightRoom);
                        Debug.Log($"Create TOP_RIGHT corner at i{i} j{j}");
                    }
                    else if (top < 5 && right < 5 && plan[right,j] == 1 && plan[i,top] == 1)
                    {
                        var botLeftRoom = Instantiate(m_botLeftCornerRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(botLeftRoom);
                        Debug.Log($"Create BOT_LEFT corner at i{i} j{j}");
                    }
                    else if (top < 5 && left >= 0 && plan[left,j] == 1 && plan[i,top] == 1)
                    {
                        var botRightRoom = Instantiate(m_botRightCornerRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(botRightRoom);
                        Debug.Log($"Create BOT_RIGHT corner at i{i} j{j}");
                    }
                    else
                    {
                        var plusRoom = Instantiate(m_plusRoom, GetPosFromCoord(i, j), Quaternion.identity);
                        GeneratedRooms.Add(plusRoom);
                        Debug.Log($"Create PLUS at i{i} j{j}");
                    }
                }
            }
        }

        IsGenerating = false;
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
        if (plan == null) return;
        
        for (int i = 0; i < plan.GetLength(0); i++)
        {
            for (int j = 0; j < plan.GetLength(1); j++)
            {
                if (plan[i, j] == -1)
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
