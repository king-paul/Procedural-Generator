using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    /// <summary>
    /// The grid to generate the mesh inside
    /// </summary>
    //public SquareGrid squareGrid;
    public MeshFilter walls;
    public MeshFilter cave;
    public bool is2D; // toggles 2D/3D mode

    public float wallHeight = 5;    
    public int textureTiling = 10;

    List<Vector3> vertices;
    List<int> triangles;
    List<List<int>> outlines = new List<List<int>>(); // stores all of the vertices that connect outlines

    // contains the vertices that have already been checked
    // uses a hash set to make contains checks quicker
    HashSet<int> checkedVertices = new HashSet<int>();

    // list of all the triangles contained in the cave mesh
    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();

    Square[,] marchingSquares;

    Vector3 CoordToWorldPoint(int width, int height, Vector2 position)
    {
        return new Vector3(-width / 2 + .5f + position.x, 0, -height / 2 + .5f + position.y);
    }


    // Called from Map Generator class
    /// <summary>
    /// Generates a new mesh to go on the grid
    /// </summary>
    /// <param name="configs">2D array of marching square configuration values</param>
    /// <param name="squareSize">The scale of the space taken up by each square on the grid</param>
    public void GenerateMesh(byte[,] configs, float squareSize)
    {
        // delete all existing dictionary values, outlines and checked vertices
        // before generating a new mesh
        triangleDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();  
        
        marchingSquares = new Square[configs.GetLength(0), configs.GetLength(1)];

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int y = 0; y < configs.GetLength(0); y++)
        {
            for (int x = 0; x < configs.GetLength(1); x++)
            {                

                marchingSquares[y, x] = new Square(CoordToWorldPoint(configs.GetLength(1), configs.GetLength(0), new Vector2(x, y)),
                                                   squareSize, configs[y,x]);

                TriangulateSquare(marchingSquares[y, x]);
            }

        }
        
        // create the new mesh
        Mesh mesh = new Mesh();        

        // assig the vertices and triangles to the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        cave.mesh = mesh;
        
        // Get vertex percentages        
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < uvs.Length; i++)
        {
            // x percent = map width / 2
            float percentX = Mathf.InverseLerp(-marchingSquares.GetLength(1) / 2 * squareSize, marchingSquares.GetLength(1) / 2 * squareSize, vertices[i].x) * textureTiling;
            // y percent = map height / 2
            float percentY = Mathf.InverseLerp(-marchingSquares.GetLength(0) / 2 * squareSize, marchingSquares.GetLength(0) / 2 * squareSize, vertices[i].z) * textureTiling;
            uvs[i] = new Vector2(percentX, percentY);
        }
        mesh.uv = uvs;        

        if (is2D)
        {
            //Generate2DColliders();
        }
        else
        {
            CreateWallMesh();
        }

    }

    /// <summary>
    /// Adds 2D colliders to the generated cave mesh
    /// </summary>
    private void Generate2DColliders()
    {
        EdgeCollider2D[] currentColliders = gameObject.GetComponents<EdgeCollider2D>();

        for (int i = 0; i < currentColliders.Length; i++)
        {
            Destroy(currentColliders[i]);
        }

        CalculateMeshOutlines();

        // iterate through the mesh outlines adding collider points from the edge points
        foreach (List<int> outline in outlines)
        {
            EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
            Vector2[] edgePoints = new Vector2[outline.Count];

            // iterate through all points in the next outline
            for (int i = 0; i < outline.Count; i++)
            {
                edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].z);
            }
            edgeCollider.points = edgePoints;

        }
    }

    /// <summary>
    /// Creates the walls of the caves based on generated layout
    /// </summary>
    void CreateWallMesh()
    {
        CalculateMeshOutlines();

        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();

        // iterate through all the outlines adding each of the vertices
        // to the wall vertices
        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;

                wallVertices.Add(vertices[outline[i]]); // left vertex
                wallVertices.Add(vertices[outline[i + 1]]); // right vertex
                wallVertices.Add(vertices[outline[i]] - Vector3.up * wallHeight); // bottom left vertex
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); // bottom right vertex

                // Add indices to wall trianges by winding anti-clockwise around walls

                // first triangle in square
                wallTriangles.Add(startIndex + 0);
                wallTriangles.Add(startIndex + 2);
                wallTriangles.Add(startIndex + 3);
                // second triangle in square
                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);
            }
        }

        // adds vertices and triangles to unity's mesh object
        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        walls.mesh = wallMesh;

        // attach a mesh collider to all walls in the generated mesh
        MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
        wallCollider.sharedMesh = wallMesh;
    }

    #region private functions
    /// <summary>
    /// Produces different triangular shapes from a square based on the value passed in
    /// </summary>
    /// <param name="square">The square object to extract the configuration from</param>
    void TriangulateSquare(Square square)
    {
        switch (square.configuration)
        {
            case 0: // if no points are selected then we don't have a mesh
                break;

            // 1 point selections
            case 1: // 0001
                MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                break;

            case 2: // 0010
                MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
                break;

            case 4: // 0100
                MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
                break;

            case 8: // 1000
                MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                break;

            // 2 point selections
            case 3: // 0011
                MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;
            case 6: // 0110
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                break;
            case 9: // 1001
                MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                break;
            case 12: // 1100
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                break;
            case 5: // 0101
                MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom,
                               square.bottomLeft, square.centreLeft);
                break;
            case 10: // 1010
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight,
                               square.centreBottom, square.centreLeft);
                break;

            // 3 point selections
            case 7: // 0111
                MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                break;

            case 11: // 1011
                MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                break;

            case 13: // 1101
                MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                break;

            case 14: // 1110
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                break;

            // 4 point selection: there entire square is a wall
            case 15: // 1111
                MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);

                // mark all vertices in in this configuration as checked
                checkedVertices.Add(square.topLeft.vertexIndex);
                checkedVertices.Add(square.topRight.vertexIndex);
                checkedVertices.Add(square.bottomRight.vertexIndex);
                checkedVertices.Add(square.bottomLeft.vertexIndex);
                break;
        }
    }

    /// <summary>
    /// Creates triangles from a series of points then builds a mesh out of them
    /// </summary>
    /// <param name="points">Takes in a node array of any size contining the points</param>
    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points); // turns points into vertices

        // if there are 3 or more vertices create a triangle      
        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        // if there are 4 vertices create a second triangles
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        // if there are 5 vertices create third triangles
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        // if there are 6 vertices create fourth triangles
        if (points.Length >= 6)
            CreateTriangle(points[0], points[3], points[4]);
    }

    /// <summary>
    /// Turns points passed in into vertices
    /// </summary>
    /// <param name="points">Takes in a node array of any size contining the points</param>
    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    // creates a triangle out of 3 nodes 
    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA, triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        // checks if the triangle already contains the key        
        if (triangleDictionary.ContainsKey(vertexIndexKey))
        {
            // if it does then add the triangle to the list
            triangleDictionary[vertexIndexKey].Add(triangle);
        }
        else
        {
            // otherwise create a new list of triangles and add
            // the triangle to the list
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangleList);
        }

    }


    /// <summary>
    /// Runs through every single vertex in the map and checks if it is an outline vertex
    /// If it is then it follows the outline all the way around to the starting point
    /// </summary>
    void CalculateMeshOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutLineVertex = GetConnectedOutlineVertex(vertexIndex);

                if (newOutLineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);

                    FollowOutline(newOutLineVertex, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }

        }
    }

    void FollowOutline(int vertexIndex, int outlineIndex)
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);

        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex, outlineIndex);
        }
    }

    /// <summary>
    /// Given an outlined vertex, find a connected vertex that is an outlined edge
    /// </summary>
    /// <param name="vertexIndex">The key to use to look up the triangles dictionary with</param>
    /// <returns></returns>
    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

        // look through the vertices of each triangle connected to the vertex        
        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];

                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    // if it has an outline edge connected to it return it
                    if (IsOutlineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                }

            }
        }

        return -1; // no outline edge was found
    }

    /// <summary>
    /// Checks if the edge formed by 2 vertices is an outline edge or not
    /// </summary>
    /// <returns>returns true or false based on wehter the shared triangles = 1</returns>
    bool IsOutlineEdge(int vertexA, int vertexB)
    {
        // Get a list of all triangles that vertex A belongs to
        List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];

        // Check how many of those triangles belong to vertex B
        int sharedTriangleCount = 0;
        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {

            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }

        // If vertex A and vertex B only share one common triangle then
        // it is an outline edge
        return sharedTriangleCount == 1;
    }
    #endregion

    /*
    private void OnDrawGizmos()
    {
        // if the square grid has been defined iterate through it
        if (marchingSquares != null)
        {
            for (int x = 0; x < marchingSquares.GetLength(0); x++)
            {
                for (int y = 0; y < marchingSquares.GetLength(1); y++)
                {
                    // Draws the control nodes. Black if they are active, white if they are not

                    // draw top left control node
                    Gizmos.color = (marchingSquares[x, y].topLeft.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(marchingSquares[x, y].topLeft.position, Vector3.one * .4f);

                    // draw top right contol node
                    Gizmos.color = (marchingSquares[x, y].topRight.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(marchingSquares[x, y].topRight.position, Vector3.one * .4f);

                    // draw bottom right control node
                    Gizmos.color = (marchingSquares[x, y].bottomRight.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(marchingSquares[x, y].bottomRight.position, Vector3.one * .4f);

                    // draw bottom left control node
                    Gizmos.color = (marchingSquares[x, y].bottomLeft.active) ? Color.black : Color.white;
                    Gizmos.DrawCube(marchingSquares[x, y].bottomLeft.position, Vector3.one * .4f);

                    // draw create midpoint positions
                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(marchingSquares[x, y].centreTop.position, Vector3.one * .15f);
                    Gizmos.DrawCube(marchingSquares[x, y].centreRight.position, Vector3.one * .15f);
                    Gizmos.DrawCube(marchingSquares[x, y].centreBottom.position, Vector3.one * .15f);
                    Gizmos.DrawCube(marchingSquares[x, y].centreLeft.position, Vector3.one * .15f);

                }
            }
        }
    }*/
}