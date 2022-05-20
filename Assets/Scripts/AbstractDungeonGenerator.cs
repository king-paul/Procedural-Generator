using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProceduralGeneration;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField]
    protected int dungeonWidth = 200;
    [SerializeField]
    protected int dungeonHeight = 200;

    protected DungeonGenerator dungeon;

    protected HashSet<Vector2Int> floorPositions;
    protected HashSet<Vector2Int> wallPositions;

    private void Awake()
    {
        floorPositions = new HashSet<Vector2Int>();
        wallPositions = new HashSet<Vector2Int>();
    }

    // entry point
    public virtual void GenerateDungeon()
    {
        tilemapVisualizer.Clear();

        if (floorPositions != null)
            floorPositions.Clear();
        else
            floorPositions = new HashSet<Vector2Int>();

        if(wallPositions != null)
            wallPositions.Clear();
        else
            wallPositions = new HashSet<Vector2Int>();

        RunProceduralGeneration();
    }

    public abstract void RunProceduralGeneration();

    protected void BuildTileData()
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

    public void GenerateTiles()
    {
        BuildTileData(); // get the tile positions

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions); // adds floor tiles to the tile map
        tilemapVisualizer.PaintWallTiles(wallPositions);
    }

}
