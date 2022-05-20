using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using ProceduralGeneration;

public class GameManager : MonoBehaviour
{
    //public AbstractDungeonGenerator generator;
    public TilemapVisualizer tilemapVisualizer;

    private HashSet<Vector2Int> floorPositions;
    private HashSet<Vector2Int> wallPositions;

    // Start is called before the first frame update
    void Start()
    {
        floorPositions = new HashSet<Vector2Int>();
        wallPositions = new HashSet<Vector2Int>();

        /*
        DungeonGenerator dungeon = new RandomWalkRoom();
            //new CorridorFirstDungeon(100, 100, 50, 50, 10, 15, 0.5f);

        var mapData = dungeon.GetMap();

        string map = GetMapString(mapData);
        Debug.Log("Map Data:\n" + map);*/
    }

    string GetMapString(int[,] mapData)
    {
        string mapString = "";

        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                int value = mapData[y, x];

                if (value == 0)
                    mapString += ". ";
                else if (value == 1)
                    mapString += "  ";
                else if (value == -1)
                    mapString += "*";
            }

            mapString += "\n";
        }

        return mapString;
    }

    public void BuildRoom()
    {
        //generator.RunProceduralGeneration();

        // clear data from previous generation
        tilemapVisualizer.Clear();
        floorPositions.Clear();
        wallPositions.Clear();

        DungeonGenerator dungeon = new RandomWalkRoom();
        BuildTileData(dungeon);
      
        tilemapVisualizer.PaintFloorTiles(floorPositions); // adds floor tiles to the tile map
        tilemapVisualizer.PaintWallTiles(wallPositions);

        //var builder = new SimpleRandomWalkDungeonGenerator(dungeon);

        //builder.GenerateTiles();
    }
    private void BuildTileData(DungeonGenerator dungeon)
    {
        int[,] mapValues = dungeon.GetMap();

        for (int y = 0; y < mapValues.GetLength(0); y++)
        {
            for (int x = 0; x < mapValues.GetLength(1); x++)
            {
                if (mapValues[x, y] == 1)
                    floorPositions.Add(new Vector2Int(x, y));
                else if (mapValues[x, y] == -1)
                    wallPositions.Add(new Vector2Int(x, y));
            }
        }

    }

}