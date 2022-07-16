using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelGenerator levelGenerator = (LevelGenerator)target;

        if (GUILayout.Button("Generate level"))
        {
            levelGenerator.GenerateLevel();
        }

        if (GUILayout.Button("Delete level"))
        {
            levelGenerator.DeleteLevel();
        }
    }
}
