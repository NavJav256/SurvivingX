using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGen : MonoBehaviour
{
	public enum DrawMode {NoiseMap, Mesh};
	public DrawMode drawMode;

	public const int mapChunkSize = 95;

	public ChunkData chunkData;
	public NoiseData noiseData;
	public TextureData textureData;

	public Material chunkMaterial;

	[Range(0,6)]
	public int editorPreviewLOD;
	public bool autoUpdate;

	Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
	Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

	private void onValuesUpdated()
    {
		if (!Application.isPlaying) drawInEditor();
    }

	private void onTextureValuesUpdated()
    {
		textureData.applyToMaterial(chunkMaterial);
    }

    private void OnValidate()
    {
		if (chunkData != null)
		{
			chunkData.onValuesUpdated -= onValuesUpdated;
			chunkData.onValuesUpdated += onValuesUpdated;
		}
		if (noiseData != null)
		{
			noiseData.onValuesUpdated -= onValuesUpdated;
			noiseData.onValuesUpdated += onValuesUpdated;
		}
		if (textureData != null)
		{
			textureData.onValuesUpdated -= onTextureValuesUpdated;
			textureData.onValuesUpdated += onTextureValuesUpdated;
		}
	}

    public void drawInEditor()
	{
		MapData mapData = generateMapData(Vector2.zero);
		MapDisplay display = GetComponent<MapDisplay>();

		if (drawMode == DrawMode.NoiseMap) display.drawTexture(TextureGen.createTextureFromHeightMap(mapData.heightMap));
		else if (drawMode == DrawMode.Mesh) display.drawMesh(MeshGen.createTerrainMesh(mapData.heightMap, chunkData.meshHeightMultiplier, chunkData.meshHeightCurve, editorPreviewLOD, chunkData.flatShading));
	}

	public void requestMapData(Vector2 centre, Action<MapData> callback)
	{
		ThreadStart threadStart = delegate 
		{
			mapDataThread(centre, callback);
		};

		new Thread(threadStart).Start();
	}

	void mapDataThread(Vector2 centre, Action<MapData> callback)
	{
		MapData mapData = generateMapData(centre);
		lock (mapDataThreadInfoQueue) 
		{
			mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
		}
	}

	public void requestMeshData(MapData mapData, int lod, Action<MeshData> callback)
	{
		ThreadStart threadStart = delegate 
		{
			meshDataThread(mapData, lod, callback);
		};

		new Thread(threadStart).Start();
	}

	void meshDataThread(MapData mapData, int lod, Action<MeshData> callback)
	{
		MeshData meshData = MeshGen.createTerrainMesh(mapData.heightMap, chunkData.meshHeightMultiplier, chunkData.meshHeightCurve, lod, chunkData.flatShading);
		lock (meshDataThreadInfoQueue) 
		{
			meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
		}
	}

	void Update() 
	{
		if (mapDataThreadInfoQueue.Count > 0)
		{
			for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) 
			{
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
				threadInfo.callback(threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count > 0) 
		{
			for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) 
			{
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
				threadInfo.callback(threadInfo.parameter);
			}
		}
	}

	MapData generateMapData(Vector2 centre)
	{
		float[,] noiseMap = Noise.createNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);
		return new MapData(noiseMap);
	}

	struct MapThreadInfo<T> 
	{
		public readonly Action<T> callback;
		public readonly T parameter;

		public MapThreadInfo(Action<T> callback, T parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}
	}

}

public struct MapData
{
	public readonly float[,] heightMap;

	public MapData(float[,] heightMap)
	{
		this.heightMap = heightMap;
	}
}
