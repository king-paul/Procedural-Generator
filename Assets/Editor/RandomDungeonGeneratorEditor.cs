using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator creator;

    private HashSet<Vector2Int> floorPositions;
    private HashSet<Vector2Int> wallPositions;

    private void Awake()
    {
        creator = (AbstractDungeonGenerator) target; // set target of custom inspector        
    }

    // create custom inspector with a button
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            creator.GenerateDungeon();

        }
    }

}
