﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen 
{
	public static MeshData createTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail) 
	{
		AnimationCurve curve = new AnimationCurve(heightCurve.keys);

		int increment = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

		int borderedSize = heightMap.GetLength(0);
		int meshSize = borderedSize - 2 * increment;
		int meshSizeUnsimplified = borderedSize - 2;

		float topLeftX = (meshSizeUnsimplified - 1) / -2f;
		float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

		int verticesPerLine = (meshSize - 1) / increment + 1;

		MeshData meshData = new MeshData(verticesPerLine);

		int[,] vertexIndicesMap = new int[borderedSize, borderedSize];

		int borderVertexIndex = -1;
		int meshVertexIndex = 0;

		for (int y = 0; y < borderedSize; y += increment)
		{
			for (int x = 0; x < borderedSize; x += increment)
			{
				bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

				if(isBorderVertex)
                {
					vertexIndicesMap[x, y] = borderVertexIndex;
					borderVertexIndex--;
                }
				else
                {
					vertexIndicesMap[x, y] = meshVertexIndex;
					meshVertexIndex++;
                }

			}
		}

		for (int y = 0; y < borderedSize; y += increment)
		{
			for (int x = 0; x < borderedSize; x += increment)
			{
				int vertexIndex = vertexIndicesMap[x, y];

				Vector2 percent = new Vector2((x - increment) / (float) meshSize, (y - increment) / (float) meshSize);
				float height = curve.Evaluate(heightMap[x, y]) * heightMultiplier;
				Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y * meshSizeUnsimplified);

				meshData.addVertex(vertexPosition, percent, vertexIndex);

				if (x < borderedSize - 1 && y < borderedSize - 1)
				{
					int a = vertexIndicesMap[x, y];
					int b = vertexIndicesMap[x + increment, y];
					int c = vertexIndicesMap[x, y + increment];
					int d = vertexIndicesMap[x + increment, y + increment];
					meshData.addTriangle(a, d, c);
					meshData.addTriangle(d, a, b);
				}
				
				vertexIndex++;
			}
		}

		return meshData;
	}
}

public class MeshData 
{
	Vector3[] borderVertices;
	Vector3[] vertices;
	int[] borderTriangles;
	int[] triangles;
	Vector2[] uvs;

	int triangleIndex;
	int borderTriangleIndex;

	public MeshData(int verticesPerLine)
	{
		borderVertices = new Vector3[verticesPerLine * 4 + 4];
		vertices = new Vector3[verticesPerLine * verticesPerLine];
		borderTriangles = new int[verticesPerLine * 24];
		triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];
		uvs = new Vector2[verticesPerLine * verticesPerLine];
	}

	public void addVertex(Vector3 position, Vector2 uv, int index) 
    {
		if (index < 0) borderVertices[- index - 1] = position;
		else
		{
			vertices[index] = position;
			uvs[index] = uv;
		}
    }

	public void addTriangle(int a, int b, int c) 
	{
		if(a < 0 || b < 0 || c < 0)
        {
			borderTriangles[borderTriangleIndex] = a;
			borderTriangles[borderTriangleIndex + 1] = b;
			borderTriangles[borderTriangleIndex + 2] = c;
			borderTriangleIndex += 3;
		} else
        {
			triangles[triangleIndex] = a;
			triangles[triangleIndex + 1] = b;
			triangles[triangleIndex + 2] = c;
			triangleIndex += 3;
        }

	}

	private Vector3[] calculateNormals()
    {
		Vector3[] vertexNormals = new Vector3[vertices.Length];
		int triangleCount = triangles.Length / 3;
        for (int i = 0; i < triangleCount; i++)
        {
			int triangleIndex = i * 3;

			int vertexIndexA = triangles[triangleIndex];
			int vertexIndexB = triangles[triangleIndex + 1];
			int vertexIndexC = triangles[triangleIndex + 2];

			Vector3 triangleNormal = calculateSurfaceNormal(vertexIndexA, vertexIndexB, vertexIndexC);

			vertexNormals[vertexIndexA] += triangleNormal;
			vertexNormals[vertexIndexB] += triangleNormal;
			vertexNormals[vertexIndexC] += triangleNormal;
		}

		int borderTriangleCount = borderTriangles.Length / 3;
		for (int i = 0; i < borderTriangleCount; i++)
		{
			int triangleIndex = i * 3;

			int vertexIndexA = borderTriangles[triangleIndex];
			int vertexIndexB = borderTriangles[triangleIndex + 1];
			int vertexIndexC = borderTriangles[triangleIndex + 2];

			Vector3 triangleNormal = calculateSurfaceNormal(vertexIndexA, vertexIndexB, vertexIndexC);

			if (vertexIndexA >= 0) vertexNormals[vertexIndexA] += triangleNormal;
			if (vertexIndexB >= 0) vertexNormals[vertexIndexB] += triangleNormal;
			if (vertexIndexC >= 0) vertexNormals[vertexIndexC] += triangleNormal;
		}

		for (int i = 0; i < vertexNormals.Length; i++)
        {
			vertexNormals[i].Normalize();
        }

		return vertexNormals;
    }

	private Vector3 calculateSurfaceNormal(int indexA, int indexB, int indexC)
    {
		Vector3 pointA = indexA < 0 ? borderVertices[- indexA - 1] : vertices[indexA];
		Vector3 pointB = indexB < 0 ? borderVertices[- indexB - 1] : vertices[indexB];
		Vector3 pointC = indexC < 0 ? borderVertices[- indexC - 1] : vertices[indexC];

		Vector3 sideAB = pointB - pointA;
		Vector3 sideAC = pointC - pointA;

		return Vector3.Cross(sideAB, sideAC).normalized;
	}

	public Mesh createMesh() 
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = calculateNormals();
		return mesh;
	}

}