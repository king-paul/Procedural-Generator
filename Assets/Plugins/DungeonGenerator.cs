using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProceduralGeneration
{
	public enum TileType
    {
		Empty,
		Floor,
		WallFull,
		Top,
		Bottom,
		SideLeft,
		SideRight,
		SideBottom,
		InnerCornerDownLeft,
		InnerCornerDownRight,
		DiagonalCornerDownLeft,
		DiagonalCornerDownRight,
		DiagonalCornerUpLeft,
		DiagonalCornerUpRight
	}

	public class DungeonGenerator
	{		
		protected TileType[,] map;

		[DllImport("Procedural Generation Library.dll")]
		public static extern void TestFunction();

		[DllImport("Procedural Generation Library.dll")]
		protected static extern IntPtr CreateRandomWalkRoom(
			int width, int height, int startX, int startY, int iterations, int walkLength, bool startRandomly = true);
		
		[DllImport("Procedural Generation Library.dll")]
		protected static extern IntPtr CreateCorridorFirstDungeon(
								int width, int height, int startX, int staryY, int corridorLength, int totalCorridors, float roomPercent,
								int roomWalkIterations = 15, int roomWalkLength = 10, bool startRandomlyEachWalk = false);

		[DllImport("Procedural Generation Library.dll")]
		protected static extern void GenerateDungeon(IntPtr dungeonPtr);
 

		[DllImport("Procedural Generation Library.dll")]
		protected static extern int GetSpaceValue(IntPtr dungeonPtr, int x, int y);

		protected DungeonGenerator(int width, int height)
		{
			//this.size = size;
			map = new TileType[width, height];
		}

		protected void BuildMap(IntPtr dungeonPointer)
        {
			GenerateDungeon(dungeonPointer);

			for (int y = 0; y < map.GetLength(0); y++)
			{
				for (int x = 0; x < map.GetLength(1); x++)
				{
					map[y, x] = (TileType) GetSpaceValue(dungeonPointer, x, y);
				}

			}
		}

		public TileType[,] GetMap()
		{
			return map;
		}

    }

    // subclass 1
    public class RandomWalkRoom : DungeonGenerator
    {
		private readonly IntPtr dungeonPointer;

		public RandomWalkRoom(int width = 50, int height = 50, int startX = 25, int startY = 25, 
			int iterations = 50, int walkLength =15, bool startRandomly = false) : base(width, height)
        {
			dungeonPointer = CreateRandomWalkRoom(width, height, startX, startY, iterations, walkLength, startRandomly);
			BuildMap(dungeonPointer);
			
		}

    }

    // subclass 2
    public class CorridorFirstDungeon : DungeonGenerator
    {

		private readonly IntPtr dungeonPointer;

		public CorridorFirstDungeon(int dungeonWidth, int dungeonHeight, int startX, int staryY, int corridorLength, int totalCorridors, float roomPercent,
								       int roomWalkIterations = 15, int roomWalkLength = 10, bool startRandomlyEachWalk = false) 
									 : base(dungeonWidth, dungeonHeight)
        {
			dungeonPointer = CreateCorridorFirstDungeon(dungeonWidth, dungeonHeight, startX, staryY, corridorLength, totalCorridors, roomPercent,
								roomWalkIterations, roomWalkLength, startRandomlyEachWalk);
			BuildMap(dungeonPointer);
		}
    }

}
