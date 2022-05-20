using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProceduralGeneration
{
	public class DungeonGenerator
	{
		protected int[,] map;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="startX"></param>
		/// <param name="startY"></param>
		/// <param name="iterations"></param>
		/// <param name="walkLength"></param>
		/// <param name="startRandomly"></param>
		/// <returns></returns>
		[DllImport("Procedural Generation Library.dll")]
		protected static extern IntPtr CreateRandomWalkRoom(
			int width, int height, int startX, int startY, int iterations, int walkLength, bool startRandomly = true);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="startX"></param>
		/// <param name="staryY"></param>
		/// <param name="corridorLength"></param>
		/// <param name="totalCorridors"></param>
		/// <param name="roomPercent"></param>
		/// <param name="roomWalkIterations"></param>
		/// <param name="roomWalkLength"></param>
		/// <param name="startRandomlyEachWalk"></param>
		/// <returns></returns>
		[DllImport("Procedural Generation Library.dll")]
		protected static extern IntPtr CreateCorridorFirstDungeon(
								int width, int height, int startX, int staryY, int corridorLength, int totalCorridors, float roomPercent,
								int roomWalkIterations = 15, int roomWalkLength = 10, bool startRandomlyEachWalk = false);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dungeonPtr"></param>
		[DllImport("Procedural Generation Library.dll")]
		protected static extern void GenerateDungeon(IntPtr dungeonPtr);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dungeonPtr"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		[DllImport("Procedural Generation Library.dll")]
		protected static extern int GetSpaceValue(IntPtr dungeonPtr, int x, int y);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		protected DungeonGenerator(int width, int height)
		{
			//this.size = size;
			map = new int[width, height];
		}

		/// <summary>
		/// Fills the map array with integer values by reading the map data from the DLL file
		/// </summary>
		/// <param name="dungeonPointer">Reference to the object return from DLL</param>
		protected void BuildMap(IntPtr dungeonPointer)
		{
			GenerateDungeon(dungeonPointer);

			for (int y = 0; y < map.GetLength(0); y++)
			{
				for (int x = 0; x < map.GetLength(1); x++)
				{
					map[x, y] = GetSpaceValue(dungeonPointer, x, y);
				}

			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int[,] GetMap()
		{
			return map;
		}

	}

	// subclass 1
	public class RandomWalkRoom : DungeonGenerator
	{
		private readonly IntPtr dungeonPointer;

		public RandomWalkRoom(int width = 50, int height = 50, int startX = 25, int startY = 25,
			int iterations = 50, int walkLength = 15, bool startRandomly = false) : base(width, height)
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