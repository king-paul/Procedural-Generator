using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// triangle structure that hold 3 integers for the vertex indicies
/// </summary>
struct Triangle
{
    public int vertexIndexA;
    public int vertexIndexB;
    public int vertexIndexC;
    int[] vertices;

    public Triangle(int a, int b, int c)
    {
        vertexIndexA = a;
        vertexIndexB = b;
        vertexIndexC = c;

        vertices = new int[3];
        vertices[0] = a;
        vertices[1] = b;
        vertices[2] = c;
    }

    // determins what value to return when object is used like an array
    public int this[int i]
    {
        get
        {
            return vertices[i];
        }
    }

    /// <summary>
    /// Checkes whether a vertex is in a tiangle
    /// </summary>
    /// <param name="vertexIndex">The index to check</param>
    /// <returns>return a true or false based on wehter the vertex is found</returns>
    public bool Contains(int vertexIndex)
    {
        return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
    }
}