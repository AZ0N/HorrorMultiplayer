using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private float nodeSize;
    [SerializeField] private Vector2Int size;

    private List<MazeNode> maze = new List<MazeNode>();

    public void ClearMaze()
    {
        for (int i = transform.childCount; i > 0; --i)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void GenerateMaze()
    {
        ClearMaze();

        // Create all MazeNodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (float)(size.x - 1) / 2, 0, y - (float)(size.y - 1) / 2);
                nodePos *= nodeSize;

                MazeNode node = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                maze.Add(node);
            }
        }
    }
}
