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

    private HashSet<Vector2Int> floorPositions;
    private HashSet<Vector2Int> wallPositions;

    // Generates the dungeon. Overrides method from suberclass
    protected override void RunProceduralGeneration()
    {
        generator = new DungeonGenerator(50, startPosition.x, startPosition.y,
            randomWalkParameters.iterations, randomWalkParameters.walkLength);

        floorPositions = new HashSet<Vector2Int>();
        wallPositions = new HashSet<Vector2Int>();

        BuildTileData(); // get the tile positions

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions); // adds floor tiles to the tile map
        tilemapVisualizer.PaintWallTiles(wallPositions);

        //WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    protected void BuildTileData()
    {
        int[,] mapValues = generator.GetMap();

        for (int y = 0; y < mapValues.GetLength(0); y++)
        {
            for (int x = 0; x < mapValues.GetLength(1); x++)
            {
                if (mapValues[x, y] == 1)
                    floorPositions.Add(new Vector2Int(x, y));
                else if(mapValues[x, y] == -1)
                    wallPositions.Add(new Vector2Int(x, y));
            }
        }
       
    }


}
