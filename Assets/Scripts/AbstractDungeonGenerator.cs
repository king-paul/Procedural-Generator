using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProceduralGeneration;

public struct DungeonTile
{
    public DungeonTile(int x, int y, TileType type)
    {
        position = new Vector2Int(x, y);
        this.type = type;
    }
    
    // properties
    public Vector2Int Position { get => position; }
    public TileType Type { get => type; }

    Vector2Int position;
    TileType type;
}

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

        Generate();
    }

    public abstract void Generate();

    protected List<DungeonTile> GetTileData()
    {
        TileType[,] mapValues = dungeon.GetMap();
        List<DungeonTile> tiles = new List<DungeonTile>();

        for (int y = 0; y < mapValues.GetLength(0); y++)
        {
            for (int x = 0; x < mapValues.GetLength(1); x++)
            {
                if (mapValues[y, x] != TileType.Empty)
                     tiles.Add(new DungeonTile(x, y, mapValues[y, x]));
            }
        }

        return tiles;
    }

    public void GenerateTiles()
    {
        List<DungeonTile> tiles = GetTileData(); // get the tile positions and types

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintTiles(tiles);
    }

}
