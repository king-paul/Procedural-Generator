using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

using ProceduralGeneration;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters; // scriptable object

    // Generates the dungeon. Overrides method from suberclass
    public override void Generate()
    {
        dungeon = new RandomWalkRoom(dungeonWidth, dungeonHeight, startPosition.x, startPosition.y,
            randomWalkParameters.iterations, randomWalkParameters.walkLength, false);

        GenerateTiles();
    }

}
