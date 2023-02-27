using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (MazeGenerator))]
public class MazeGeneratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        MazeGenerator mazeGenerator = (MazeGenerator) target;

        DrawDefaultInspector();
        if (GUILayout.Button("Generate Maze"))
        {
            //TODO Call mazeGenerator.generateMaze()
            Debug.Log($"Generate {mazeGenerator.size.x}x{mazeGenerator.size.y} Maze");
        }
    }
}