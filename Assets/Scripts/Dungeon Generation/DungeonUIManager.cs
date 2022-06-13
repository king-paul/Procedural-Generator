using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ProceduralGeneration;
using System;

public class DungeonUIManager : MonoBehaviour
{
    //public AbstractDungeonGenerator generator;
    public TilemapVisualizer tilemapVisualizer;
    public bool logInput = false;

    public GameObject uIPanel;

    [Header("Basic Settings")]
    [Header("GUI Components")]    
    public Toggle corridorFirstToggle;
    public Toggle roomFirstToggle;
    public TMP_InputField startPositionXField;
    public TMP_InputField startPositionYField;
    public TMP_InputField dungeonWidthField;
    public TMP_InputField dungeonHeightField;

    [Header("Corridor Settings")]
    public TMP_InputField corridorLengthField;
    public TMP_InputField corridorCountField;
    public Slider roomFillSlider;
    public TextMeshProUGUI roomFillLabel;

    [Header("Room Settings")]    
    public TMP_InputField minRoomWidthField;
    public TMP_InputField minRoomHeightField;
    public Slider offsetSlider;
    public TextMeshProUGUI offsetLabel;
    public Toggle randomWalkToggle;
    [Header("Random Walk Settings")]
    public GameObject randomWalkPanel;
    public TMP_InputField iterationsField;
    public TMP_InputField walklengthField;
    public Toggle startRandomlyToggle;
    [Header("ErrorText")]
    public TextMeshProUGUI errorText;

    DungeonGenerator dungeon;

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

    private void Update()
    {
        if(Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1))
        {
            if(uIPanel.activeInHierarchy)
                uIPanel.SetActive(false);
            else
                uIPanel.SetActive(true);
        }
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

    public void GenerateDungeonFromGUI()
    {
        try
        {
            int startX = int.Parse(startPositionXField.text);
            int startY = int.Parse(startPositionYField.text);
            int dungeonWidth = int.Parse(dungeonWidthField.text);
            int dungeonHeight = int.Parse (dungeonHeightField.text);

            int walkIterations = int.Parse(iterationsField.text);
            int walkLength = int.Parse(walklengthField.text);
            bool startRandomly = startRandomlyToggle.isOn;

            if (corridorFirstToggle.isOn)
            {
                int corridorLength = int.Parse(corridorLengthField.text);
                int corridorCount = int.Parse(corridorCountField.text);
                float roomFill = roomFillSlider.value;

                dungeon = new CorridorFirstDungeon(dungeonWidth, dungeonHeight, startX, startY, corridorLength, corridorCount,
                                         roomFill, walkIterations, walkLength, startRandomly);

                if (logInput)
                {
                    Debug.Log("Dungeon Type: Corridor First\n" +
                              "Start Position: " + startX + ", " + startY +
                              "\nDungeon Size: " + dungeonWidth + ", " + dungeonHeight);

                    Debug.Log("\nCorridorLength: " + corridorLength +
                              "\nCorridor Count: " + corridorCount +
                              "\nRoom Fill Percent: " + roomFill);
                }
            }
            else if(roomFirstToggle.isOn)
            {
                int minRoomWidth = int.Parse(minRoomWidthField.text);
                int minRoomHeight = int.Parse(minRoomHeightField.text);
                int offset = (int) offsetSlider.value;
                bool randomWalkRooms = randomWalkToggle.isOn;

                dungeon = new RoomFirstDungeon(dungeonWidth, dungeonHeight, startX, startY, minRoomWidth, minRoomHeight, offset, randomWalkRooms,
                    walkIterations, walkLength, startRandomly);

                if (logInput)
                {
                    Debug.Log("Dungeon Type: Room First\n" +
                              "Start Position: " + startX + ", " + startY +
                              "\nDungeon Size: " + dungeonWidth + ", " + dungeonHeight);

                    Debug.Log("\nMinimum Room Width: " + minRoomWidth +
                              "\nMinimum Room Height: " + minRoomHeight +
                              "\nRoom Offset: " + offset +
                              "\nRandom Walk Rooms: " + randomWalkRooms);
                }
            }

            if (logInput)
            {
                Debug.Log("Iterations: " + walkIterations +
                      "\nWalkLength: " + walkLength +
                      "\nStart Randomly Each Iteration: " + startRandomly);
            }

            errorText.gameObject.SetActive(false);
            GenerateTiles();

        } catch(FormatException e) 
        {
            //Debug.LogError(e.Message);
            errorText.gameObject.SetActive(true);
        }
    }

    public void UpdateRoomFillLabel()
    {        
        roomFillLabel.text = ((int)(roomFillSlider.value * 100)).ToString();
    }

    public void UpdateOffsetLabel()
    {
        offsetLabel.text = offsetSlider.value.ToString();
    }

    public void ToggleRandomWalkSettings(Toggle toggle)
    {
        if (toggle.isOn)
            randomWalkPanel.SetActive(true);
        else
            randomWalkPanel.SetActive(false);
    }

    #region tile generations
    private void GenerateTiles()
    {
        List<DungeonTile> tiles = GetTileData(); // get the tile positions and types

        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintTiles(tiles);
    }

    private List<DungeonTile> GetTileData()
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
    #endregion

}