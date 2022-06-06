using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProceduralGeneration;

public class RoomsFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] int minRoomWidth = 4;
    [SerializeField] int minRoomHeight = 4;
    [Range(1, 10)]
    [SerializeField] int offset = 1;
    [SerializeField] bool randomWalkRooms;

    public override void Generate()
    {
        dungeon = new RoomFirstDungeon(dungeonWidth, dungeonHeight, startPosition.x, startPosition.y, minRoomWidth, minRoomHeight, offset,
                                        randomWalkRooms,
            randomWalkParameters.iterations, randomWalkParameters.walkLength, randomWalkParameters.startRandomlyEachIteration);

        GenerateTiles();
    }
}
