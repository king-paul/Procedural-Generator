using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    // DThe different tile sprites from the tile map
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
                     wallInnerCornerDownLeft, wallInnerCornerDownRight,
                     wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft,
                     wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    // Renders the floor tiles on the screen
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }
    
    public void PaintWallTiles(IEnumerable<Vector2Int> wallPositions)
    {
        PaintTiles(wallPositions, wallTilemap, wallFull);
    }

    public void PaintSingleFloorTile(Vector2Int position)
    {
        PaintSingleTile(floorTilemap, floorTile, position);
    }

    // render a wall tile from the tilemap at specified position
    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        //Debug.Log(position + " type: " + binaryType);

        int typeAsInt = Convert.ToInt32(binaryType, 2); // converts string of binary numbers into an integer
        TileBase tile = null; // the type of tile to be painted

        // checks if the matching binary number is in any of the lists in the wall type helper and
        // if it is, sets the tile to that wall type
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        // if the tile has been assigned paint it
        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);
    }

    // Renders all tiles in a given list of positions
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    // Adds a single tile to the tilemap using specified parameters
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    // removes all tiles previous generated in the tilemap
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2); // converts string of binary numbers into an integer
        TileBase tile = null; // the type of tile to be painted

        // checks if the matching binary number is in any of the lists in the wall type helper and
        // if it is, sets the tile to that wall type
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }


        // if the tile has been assigned paint it
        if (tile != null)
            PaintSingleTile(wallTilemap, tile, position);

    }
}
