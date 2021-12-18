using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    public int triangleIndex;

    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        triangles = new int[(width - 1) * (height - 1)*6];
        uvs = new Vector2[width * height];
    }

    public void addTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh createMesh()
    {
        Mesh m = new Mesh();
        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = uvs;
        m.RecalculateNormals();
        return m;
    }
}
