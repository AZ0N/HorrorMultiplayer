using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeNode nodePrefab;
    [SerializeField] private Vector2Int size;

    public void GenerateMaze()
    {
        Debug.Log("Generate Maze");
    }
}
