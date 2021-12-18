using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen
{
    public static MeshData createTerrainMesh(float[,] hMap)
    {
        int width = hMap.GetLength(0);
        int height = hMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;


        MeshData mesh = new MeshData(width, height);
        int vertexIndex = 0;

        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                mesh.vertices[vertexIndex] = new Vector3(topLeftX + x, hMap[x, y], topLeftZ - y);
                mesh.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x<width-1 && y<height-1)
                {
                    mesh.addTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    mesh.addTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return mesh;
    }
}


