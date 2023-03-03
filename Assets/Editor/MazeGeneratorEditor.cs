using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MazeGenerator mazeGenerator = (MazeGenerator)target;

        GUILayout.Label("Generation:");
        if (GUILayout.Button("Generate Maze"))
        {
            mazeGenerator.GenerateMaze();
        }
        if (GUILayout.Button("Generate Random Maze"))
        {
            mazeGenerator.SetRandomSeed();
            mazeGenerator.GenerateMaze();
        }
        if (GUILayout.Button("Random Seed"))
        {
            mazeGenerator.SetRandomSeed();
        }
        if (GUILayout.Button("Clear Maze"))
        {
            mazeGenerator.ClearMaze();
        }
    }
}