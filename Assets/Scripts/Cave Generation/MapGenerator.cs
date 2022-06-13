using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralGeneration;

public class MapGenerator : MonoBehaviour
{
	[Header("Map Size")]
	public int width = 128;
	public int height = 72;

	[Header("Random Number Generation")]
	public int seed = 0;
	public bool useRandomSeed;

	[Header("Properties")]
	[Range(0, 100)]
	public int randomFillPercent = 50;

	[Range(0, 10)]
	public int smoothingIterations = 5;

	[Range(0, 10)]
	public int borderSize = 3; // the size of the boarder around the map

	[Range(0, 200)]
	public int minimumWallSize = 50; // the minimum size of a wall
	[Range(0, 100)]
	public int minimumRoomSize = 50; // the minimum size of a room

	[Range(1, 10)]
	public int passageWidth = 1;

	[Header("Mesh Generation")]
	//public MeshFilter caveMesh;
	//public MeshFilter wallsMesh;
	public int tileSize = 1;
	//public bool is2D;

	int[,] map;

	CaveGenerator cave;
	MeshGeneratorNew meshGenerator;

	//Mesh mesh;

	// Start is called before the first frame update
	void Start()
	{
		meshGenerator = GetComponent<MeshGeneratorNew>();
		GenerateMap();
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			GenerateMap();
		}
	}

	void GenerateMap()
    {
		cave = new CaveGenerator(width, height, randomFillPercent, smoothingIterations, borderSize, minimumWallSize,
								minimumRoomSize, passageWidth, useRandomSeed, seed, false, tileSize);

		//cave.MarchingSquares;
		/*
		Vector3[] vertices = new Vector3[cave.TotalBaseVertices];

		for(int i = 0; i < vertices.Length; i++)
		{
			vertices[i].x = cave.BaseVerticesX[i];
			vertices[i].y = cave.BaseVerticesY[i];
			vertices[i].z = cave.BaseVerticesZ[i];
		}

		mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = cave.BaseTriangles;		
		mesh.RecalculateNormals();

		caveMesh.mesh = mesh;*/

		//meshGenerator.GenerateMesh(cave.Map, tileSize);
		meshGenerator.GenerateMesh(cave.MarchingSquares, tileSize, true);
	}

	void PrintCaveToConsole()
    {
		string caveString = "";

		for (int row = 0; row < cave.Map.GetLength(0); row++)
		{
			for (int col = 0; col < cave.Map.GetLength(1); col++)
			{
				if (cave.Map[row, col])
					caveString += "*";
				else
					caveString += ".";
			}

			caveString += "\n";
		}

		Debug.Log(caveString);
	}

	void PrintMarchingSquareToConsole()
    {
		string caveString = "";

		for (int row = 0; row < cave.MarchingSquares.GetLength(0); row++)
		{
			for (int col = 0; col < cave.MarchingSquares.GetLength(1); col++)
			{
				if (cave.MarchingSquares[row, col] == 15)
					caveString += "*";
				else if (cave.MarchingSquares[row, col] == 0)
					caveString += ".";
				else
					caveString += cave.MarchingSquares[row, col].ToString("X");
			}

			caveString += "\n";
		}

		Debug.Log(caveString);
	}	

}