using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProceduralGeneration;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    public float roomPercent = 0.8f;
    
    /*
    public CorridorFirstDungeonGenerator(DungeonGenerator dungeon) : base(dungeon)
    {

    }*/

    // Generates the dungeon. Overrides method from suberclass
    public override void RunProceduralGeneration()
    {
        dungeon = new CorridorFirstDungeon(dungeonWidth, dungeonHeight, startPosition.x, startPosition.y, // basic parameters
                        corridorLength, corridorCount, roomPercent, // corridor paramaeters
                        randomWalkParameters.iterations, randomWalkParameters.walkLength); // random walk parameters

        GenerateTiles();
    }
}
