using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using ProceduralGeneration;

public class GameManager : MonoBehaviour
{
    //public AbstractDungeonGenerator generator;
    public TilemapVisualizer tilemapVisualizer;
    AbstractDungeonGenerator generator;

    // Start is called before the first frame update
    void Start()
    {        
        //dungeon = new SimpleRandomWalkDungeonGenerator();
            //new CorridorFirstDungeon(100, 100, 50, 50, 10, 15, 0.5f);

        /*
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
        // clear data from previous generation
        generator.Generate();
    }

}