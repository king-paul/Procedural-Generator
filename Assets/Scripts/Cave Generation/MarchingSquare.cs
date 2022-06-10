using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storse a 2D array of squares
/// </summary>
public class SquareGrid
{
    /// <summary>
    /// 2D array to hold each square
    /// </summary>
    public Square[,] squares;

    /// <summary>
    /// Fills the 2D array of squares using parameters
    /// </summary>
    /// <param name="map">2D array of integers to be passed in from the map generator</param>
    /// <param name="squareSize">The scale of the squares inside the grid</param>
    public SquareGrid(bool[,] map, float squareSize)
    {
        int nodeCountX = map.GetLength(1); // Total number of colums in the grid
        int nodeCountY = map.GetLength(0); // Total number of rows in the grid

        // width and height based on the number of rows and columns and the size of each square
        float mapWidth = nodeCountX * squareSize;
        float mapHeight = nodeCountY * squareSize;

        // 2d array of controls nodes to be placed on the square
        ControlNode[,] controlNodes = new ControlNode[nodeCountY, nodeCountX];

        // iterates through the grid
        for (int y = 0; y < nodeCountY; y++)
        {
            for (int x = 0; x < nodeCountX; x++)
            {            
                // gets the position of next control node  
                Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0,
                                          -mapHeight / 2 + y * squareSize + squareSize / 2);

                // add a new control node to the array based the position, whether the value in the array is a wall
                // and the size of the square
                controlNodes[y, x] = new ControlNode(pos, map[y, x], squareSize);
            }
        }

        // defines the sise of the square array and fills it with square objects
        squares = new Square[nodeCountY - 1, nodeCountX - 1];
        // iterates through the squares array
        for (int y = 0; y < nodeCountY - 1; y++)
        {
            for (int x = 0; x < nodeCountX - 1; x++)
            {            
                // creates a new square and defines the 4 control nodes on it
                squares[y, x] = new Square(controlNodes[y + 1, x], controlNodes[y + 1, x + 1], controlNodes[y, x + 1], controlNodes[y, x]);
            }
        }

    }

}

/// <summary>
/// Defines a Square to be drawn on a square grid
/// </summary>
public class Square
{
    public ControlNode topLeft, topRight, bottomRight, bottomLeft;
    public Node centreTop, centreRight, centreBottom, centreLeft;

    /// <summary>
    /// A number between 0 and 15. There are 16 different binary configurations for each square/
    /// </summary>
    public byte configuration; // value is between 0000 and 1111

    /// <summary>
    /// Instance an instance of a Square. Take in the four cornoer nodes as parameters
    /// </summary>
    public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
    {
        // set the cornor nodes
        this.topLeft = topLeft;
        this.topRight = topRight;
        this.bottomRight = bottomRight;
        this.bottomLeft = bottomLeft;

        // set center nodes to the nodes connected to the corner nodes
        centreTop = topLeft.right;
        centreRight = bottomRight.above;
        centreBottom = bottomLeft.right;
        centreLeft = bottomLeft.above;

        // turns on the appropriate bit in the four bit number base on
        // which control nodes are active
        if (topLeft.active)
            configuration += 8; // turns on the first bit
        if (topRight.active)
            configuration += 4; // turns on the second bit
        if (bottomRight.active)
            configuration += 2; // turns on the thrid bit
        if (bottomLeft.active)
            configuration += 1; // turns on the fourth bit
    }

    public Square(Vector3 position, float squareSize, byte configuration)
    {
        this.configuration = configuration;

        switch(configuration)
        {
            case 0: // if no points are selected then we don't have a mesh
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, false);
            break;
            
            case 1: // 0001
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, true);
            break;

            case 2: // 0010
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, false);
            break;

            case 3: // 0011
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;

            case 4: // 0100
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, false);
                break;

            case 5: // 0101
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;

            case 6: // 0110
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, false);
                break;
            
            case 7: // 0111
                topLeft = new ControlNode(1, position, squareSize, false);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;

            case 8: // 1000
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft  = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, false);
                break;          
            
            case 9: // 1001
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft  = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;

            case 10: // 1010
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, false);
                break;

            case 11: // 1011
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, false);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;

            case 12: // 1100
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, false);
                break;

            case 13: // 1101
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, false);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;

            case 14: // 1110
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, false);
                break;

            // The entire square is a wall
            case 15: // 1111
                topLeft = new ControlNode(1, position, squareSize, true);
                topRight = new ControlNode(2, position, squareSize, true);
                bottomLeft  = new ControlNode(3, position, squareSize, true);
                bottomRight = new ControlNode(4, position, squareSize, true);
                break;
        }

        // set center nodes to the nodes connected to the corner nodes
        centreTop = topLeft.right;
        centreRight = bottomRight.above;
        centreBottom = bottomLeft.right;
        centreLeft = bottomLeft.above;

    }
}

// Class 3
/// <summary>
/// Midpoint nodes that go on each square by defining a position
/// </summary>
public class Node
{
    public Vector3 position;
    public int vertexIndex = -1; // the number of the vertex

    public Node() { }

    public Node(Vector3 pos)
    {
        position = pos;
    }
}

// Subclass of class 3
/// <summary>
/// The corner switches on the each square. Extends the Node class.
/// </summary>
public class ControlNode : Node
{
    public bool active;
    public Node above, right; // The positions above and to the right of the control node

    /// <summary>
    /// Constructor that creates a subtype of the node class and passes the position to the 
    /// super class
    /// </summary>
    /// <param name="pos">The position of the node. Passed to the node superclass</param>
    /// <param name="active">Sets whether or not the node is active</param>
    /// <param name="squareSize">Sets the scale of the square to be drawn</param>
    public ControlNode(Vector3 pos, bool active, float squareSize) : base(pos)
    {
        this.active = active;
        // set the position to the distance above
        above = new Node(position + Vector3.forward * squareSize / 2f);
        // set the position to the distance to the right
        right = new Node(position + Vector3.right * squareSize / 2f);
    }

    public ControlNode(int number, Vector3 squarePos, float squareSize, bool active)
    {
        Vector3 position;

        switch(number)
        {
            case 1: // top left
                position = new Vector3(squarePos.x - squareSize / 2, 0, squarePos.z + squareSize / 2);
                break;

            case 2: // top right
                position = new Vector3(squarePos.x + squareSize / 2, 0, squarePos.z + squareSize / 2);
                break;

            case 3: // bottom left
                position = new Vector3(squarePos.x - squareSize / 2, 0, squarePos.z - squareSize / 2);
                break;

            case 4: // bottom right
                position = new Vector3(squarePos.x + squareSize / 2, 0, squarePos.z - squareSize / 2);
                break;

            default: position = squarePos;
                break;
        }

        base.position = position;

        this.active = active;
        // set the position to the distance above
        above = new Node(position + Vector3.forward * squareSize / 2f);
        // set the position to the distance to the right
        right = new Node(position + Vector3.right * squareSize / 2f);
    }

}