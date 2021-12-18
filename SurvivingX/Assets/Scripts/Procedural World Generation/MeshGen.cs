using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen
{
    public static MeshData createTerrainMesh(float[,] hMap, float multiplier, AnimationCurve heightCurve, int levelOfDetail)
    {

        AnimationCurve curve = new AnimationCurve(heightCurve.keys);

        int width = hMap.GetLength(0);
        int height = hMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int increment = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int verticesPerLine = ((width - 1) / increment) + 1;

        MeshData mesh = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for(int y=0; y<height; y+=increment)
        {
            for(int x=0; x<width; x+=increment)
            { 
                mesh.vertices[vertexIndex] = new Vector3(topLeftX + x, curve.Evaluate(hMap[x, y]) * multiplier, topLeftZ - y);
                mesh.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x<width-1 && y<height-1)
                {
                    mesh.addTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    mesh.addTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return mesh;
    }
}


