using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProceduralGeneration
{
    public class CaveGenerator
    {
        // array declaration
        /// <summary>
        /// Returns map of solid and vacant spaces
        /// </summary>
        public bool[,] Map { get; }
        /// <summary>
        /// Returnes a grid of marching square configurations
        /// </summary>
        public byte[,] MarchingSquares { get; }
        public int[] BaseTriangles { get; }
        public int[] WallTriangles { get; }

        public float[] BaseVerticesX { get; }
        public float[] BaseVerticesY { get; }
        public float[] BaseVerticesZ { get; }

        public float[] WallVerticesX { get; }
        public float[] WallVerticesY { get; }
        public float[] WallVerticesZ { get; }

        public int TotalBaseTriangles { get; }
        public int TotalBaseVertices { get; }
        public int TotalWallTriangles { get; }
        public int TotalWallVertices { get; }

        public int Seed { get; }

        // External Functions
        [DllImport("Procedural Generation Library.dll")]
        protected static extern IntPtr GenerateCave(int width, int height, int fillPercent, int smoothingIterations, int borderSize,
        int wallThresholdSize, int roomThresholdSize, int passageWidth, bool useRandomSeed, int seed,
        bool generateMesh, float tileSize, float wallHeight);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern bool IsWall(int x, int y, IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern int GetMarchingSquareValue(int x, int y, IntPtr cavePointer);

        //[DllImport("Procedural Generation Library.dll")]
        //protected static extern IntPtr GenerateMesh(IntPtr cavePointer, float gridSize, float wallHeight);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern int GetTotalBaseTriangles(IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern int GetTotalBaseVertices(IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern int GetTotalWallTriangles(IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern int GetTotalWallVertices(IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern IntPtr GetBaseTriangles(IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern IntPtr GetBaseVerticies(IntPtr cavePointer, char component);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern IntPtr GetWallTriangles(IntPtr cavePointer);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern IntPtr GetWallVerticies(IntPtr cavePointer, char component);

        [DllImport("Procedural Generation Library.dll")]
        protected static extern int GetSeedValue(IntPtr cavePointer);

        // Constructor
        public CaveGenerator(int width = 128, int height = 72, int fillPercent = 53, int smoothingIterations = 5, int borderSize = 3,
                        int wallThresholdSize = 50, int roomThresholdSize = 50, int passageWidth = 1,
                        bool useRandomSeed = false, int seed = 0, bool generateMesh = false, float tileSize = 1, float wallHeight = 5)
        {
            IntPtr cave =
                GenerateCave(width, height, fillPercent, smoothingIterations, borderSize, wallThresholdSize, roomThresholdSize,
                             passageWidth, useRandomSeed, seed, generateMesh, tileSize, wallHeight);

            Map = new bool[height + (borderSize * 2), width + (borderSize * 2)];
            MarchingSquares = new byte[height + (borderSize * 2) -1, width + (borderSize * 2) -1];

            Seed = GetSeedValue(cave);

            //IntPtr mesh = GenerateMesh(cave, 1, 5);

            // fill map array
            for (int y = 0; y < Map.GetLength(0); y++)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                {
                    Map[y, x] = IsWall(x, y, cave);
                }
            }

            // fill marching squares array
            for (int y = 0; y < MarchingSquares.GetLength(0) - 1; y++)
            {
                for (int x = 0; x < MarchingSquares.GetLength(1) - 1; x++)
                {
                    MarchingSquares[y, x] = (byte) GetMarchingSquareValue(x, y, cave);
                }
            }

            if (generateMesh)
            {
                /******************
                 * BASE TRIANGLES *
                 ******************/
                TotalBaseTriangles = GetTotalBaseTriangles(cave);
                TotalBaseVertices = GetTotalBaseVertices(cave);

                // Get the base triangle indices
                IntPtr trianglesPointer = GetBaseTriangles(cave);
                BaseTriangles = new int[TotalBaseTriangles];
                Marshal.Copy(trianglesPointer, BaseTriangles, 0, TotalBaseTriangles);

                // X component
                IntPtr verticesPointer = GetBaseVerticies(cave, 'x');
                BaseVerticesX = new float[TotalBaseTriangles];
                Marshal.Copy(verticesPointer, BaseVerticesX, 0, TotalBaseVertices);

                // Y component
                verticesPointer = GetBaseVerticies(cave, 'y');
                BaseVerticesY = new float[TotalBaseTriangles];
                Marshal.Copy(verticesPointer, BaseVerticesY, 0, TotalBaseVertices);

                // Z component
                verticesPointer = GetBaseVerticies(cave, 'z');
                BaseVerticesZ = new float[TotalBaseTriangles];
                Marshal.Copy(verticesPointer, BaseVerticesZ, 0, TotalBaseVertices);

                /******************
                 * WALL TRIANGLES *
                 ******************/
                TotalWallTriangles = GetTotalWallTriangles(cave);
                TotalWallVertices = GetTotalWallVertices(cave);

                // Get the wall triangle indices
                trianglesPointer = GetWallTriangles(cave);
                WallTriangles = new int[TotalWallTriangles];
                Marshal.Copy(trianglesPointer, WallTriangles, 0, TotalWallTriangles);

                // X component
                verticesPointer = GetWallVerticies(cave, 'x');
                WallVerticesX = new float[TotalWallVertices];
                Marshal.Copy(verticesPointer, WallVerticesX, 0, TotalWallVertices);

                // Y component
                verticesPointer = GetWallVerticies(cave, 'y');
                WallVerticesY = new float[TotalWallVertices];
                Marshal.Copy(verticesPointer, WallVerticesY, 0, TotalWallVertices);

                // Z component
                verticesPointer = GetWallVerticies(cave, 'z');
                WallVerticesZ = new float[TotalWallVertices];
                Marshal.Copy(verticesPointer, WallVerticesZ, 0, TotalWallVertices);
            }
        }

    }
}