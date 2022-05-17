using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using ProceduralGeneration;

public class GameManager : MonoBehaviour
{
    struct MyStruct
    {
        public int a, b;
    }

    // Start is called before the first frame update
    void Start()
    {
        DungeonGenerator dungeon = new DungeonGenerator(50);

        int[,] mapData = dungeon.GetMap();

        string map = GetMapString(mapData);
        Debug.Log("Map Data:\n" + map);
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
                    mapString += "o";
                else if (value == -1)
                    mapString += "*";
            }

            mapString += "\n";
        }

        return mapString;
    }

}