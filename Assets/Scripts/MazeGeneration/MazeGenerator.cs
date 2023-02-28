using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private GameObject pillarPrefab;
    [SerializeField] private float nodeSize;
    [SerializeField] private Vector2Int size;
    [SerializeField] private int seed;

    private MazeNode[,] maze;

    // Generate random seed for deterministic maze-generation
    public void SetRandomSeed()
    {
        seed = Random.Range(int.MinValue, int.MaxValue);
    }

    // Clear previously generated maze 
    public void ClearMaze()
    {
        for (int i = transform.childCount; i > 0; --i)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            else
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    // Generate new maze based on seed
    public void GenerateMaze()
    {
        Random.InitState(seed);

        ClearMaze();
        maze = new MazeNode[size.x, size.y];

        // Create all MazeNodes
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (float)(size.x - 1) / 2, 0, y - (float)(size.y - 1) / 2);
                nodePos *= nodeSize;
                
                maze[x,y] = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
            }
        }

        //TODO Create better way to handle the maze-mash than using pillars. It causes light-glitching. Better to generate the walls after generating the maze.
        // Create pillars
        for (int x = 0; x < size.x + 1; x++)
        {
            for (int y = 0; y < size.y + 1; y++)
            {
                Vector3 pillarPos = new Vector3(x - (float)(size.x - 1) / 2 - 0.5f, 0, y - (float)(size.y - 1) / 2 - 0.5f);
                pillarPos *= nodeSize;
                
                Instantiate(pillarPrefab, pillarPos, Quaternion.identity, transform);
            }
        }

        // Select starting node
        Vector2Int startIndex = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
        maze[startIndex.x, startIndex.y].Visit();

        // Initialize stack for backtracking
        List<Vector2Int> stack = new List<Vector2Int>();
        stack.Add(startIndex);

        while (stack.Count > 0)
        {
            Vector2Int currentNode = stack[stack.Count - 1];

            // List of possible neighbors neighbours
            List<Vector2Int> possibleNodes = new List<Vector2Int>();

            for (int i = -1; i <= 1; i += 2)
            {
                possibleNodes.Add(new Vector2Int(currentNode.x + i, currentNode.y));
                possibleNodes.Add(new Vector2Int(currentNode.x, currentNode.y + i));
            }

            // Filter out any indexes outside the mazegrid, and where the corresponding mazeNode has already been visited
            possibleNodes = possibleNodes.FindAll(n => (insideMaze(n.x, n.y) && !maze[n.x,n.y].Visited()));

            // If there are no possible moves
            if (possibleNodes.Count == 0)
            {
                // Remove the last element to backtrack
                stack.RemoveAt(stack.Count - 1);
            }
            else
            {
                // If we have 1..4 possible moves, select a random move
                Vector2Int chosenNode = possibleNodes[Random.Range(0, possibleNodes.Count)];
                Vector2Int direction = chosenNode - currentNode;

                // Remove the corresponding walls
                switch ((direction.x, direction.y))
                {
                    case (1, 0):
                        // If we're going right, then remove the right wall from current node, and left wall of chosenNode, etc...
                        maze[currentNode.x, currentNode.y].RemoveWall(WallType.Right);
                        maze[chosenNode.x, chosenNode.y].RemoveWall(WallType.Left);
                        break;
                    case (-1, 0):
                        // Left
                        maze[currentNode.x, currentNode.y].RemoveWall(WallType.Left);
                        maze[chosenNode.x, chosenNode.y].RemoveWall(WallType.Right);
                        break;
                    case (0, 1):
                        // Up
                        maze[currentNode.x, currentNode.y].RemoveWall(WallType.Top);
                        maze[chosenNode.x, chosenNode.y].RemoveWall(WallType.Bot);
                        break;
                    case (0, -1):
                        // Down
                        maze[currentNode.x, currentNode.y].RemoveWall(WallType.Bot);
                        maze[chosenNode.x, chosenNode.y].RemoveWall(WallType.Top);
                        break;
                }

                // Add the chosen node to the stack and mark it as visited
                stack.Add(chosenNode);
                maze[chosenNode.x, chosenNode.y].Visit();
            }
        }
    }

    private bool insideMaze(int x, int y)
    {
        return 0 <= x && x < size.x && 0 <= y && y < size.y;
    }
}
