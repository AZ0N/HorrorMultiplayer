using System;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
    bool visited = false;

    [SerializeField]
    private GameObject TopWall;
    [SerializeField]
    private GameObject BotWall;
    [SerializeField]
    private GameObject RightWall;
    [SerializeField]
    private GameObject LeftWall;

    public void RemoveWall(WallType wallType)
    {
        switch (wallType)
        {
            case WallType.Top:
                TopWall.SetActive(false);
                break;
            case WallType.Bot:
                BotWall.SetActive(false);
                break;
            case WallType.Right:
                RightWall.SetActive(false);
                break;
            case WallType.Left:
                LeftWall.SetActive(false);
                break;
        }
    }

    public void Visit()
    {
        visited = true;
    }

    public bool Visited()
    {
        return visited;
    }

}

public enum WallType {Top, Bot, Right, Left}