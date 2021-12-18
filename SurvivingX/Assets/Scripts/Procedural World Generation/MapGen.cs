using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGen : MonoBehaviour
{
    public enum DrawMode { noise, colour, mesh};

    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int LOD;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public bool update;

    public TerrainType[] regions;

    Queue<ThreadInfo<MapData>> mapDataThreadQ = new Queue<ThreadInfo<MapData>>();
    Queue<ThreadInfo<MeshData>> meshDataThreadQ = new Queue<ThreadInfo<MeshData>>();

    public void requestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            mapDataThread(callback);
        };

        new Thread(threadStart).Start();
    }

    private void mapDataThread(Action<MapData> callback)
    {
        MapData mapData = generateMapData();
        lock (mapDataThreadQ)
        {
            mapDataThreadQ.Enqueue(new ThreadInfo<MapData>(callback, mapData));
        }
    }

    public void requestMeshData(MapData mapData, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            meshDataThread(mapData, callback);
        };

        new Thread(threadStart).Start();
    }

    private void meshDataThread(MapData mapData, Action<MeshData> callback)
    {
        MeshData meshData = MeshGen.createTerrainMesh(mapData.hMap, heightMultiplier, heightCurve, LOD);
        lock (meshDataThreadQ)
        {
            meshDataThreadQ.Enqueue(new ThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if(mapDataThreadQ.Count > 0)
        {
            for (int i=0; i<mapDataThreadQ.Count; i++)
            {
                ThreadInfo<MapData> threadInfo = mapDataThreadQ.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
        if (meshDataThreadQ.Count > 0)
        {
            for (int i = 0; i < meshDataThreadQ.Count; i++)
            {
                ThreadInfo<MeshData> threadInfo = meshDataThreadQ.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
    }

    private MapData generateMapData()
    {
        float[,] map = Noise.createMap(mapChunkSize, mapChunkSize, noiseScale, seed, octaves, persistance, lacunarity, offset);
        Color[] cMap = new Color[mapChunkSize * mapChunkSize];

        for(int y=0; y<mapChunkSize; y++)
        {
            for(int x=0; x<mapChunkSize; x++)
            {
                float currentHeight = map[x, y];
                for(int i=0; i<regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        cMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        return new MapData(map, cMap);
    }

    public void drawMapInEditor()
    {

        MapData mapData = generateMapData();


        MapDisplay display = GetComponent<MapDisplay>();

        if (drawMode == DrawMode.noise) display.drawMap(TextureGen.createTextureFromHeight(mapData.hMap));
        else if (drawMode == DrawMode.colour) display.drawMap(TextureGen.createTextureFromColour(mapData.cMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.mesh) display.drawMesh(MeshGen.createTerrainMesh(mapData.hMap, heightMultiplier, heightCurve, LOD), TextureGen.createTextureFromColour(mapData.cMap, mapChunkSize, mapChunkSize));
    }

    public void OnValidate()
    {
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;

    }

    struct ThreadInfo<T>
    {
        public Action<T> callback;
        public T param;

        public ThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            param = parameter;
        }
    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}

public struct MapData
{
    public float[,] hMap;
    public Color[] cMap;

    public MapData(float[,] heightMap, Color[] colourMap)
    {
        hMap = heightMap;
        cMap = colourMap;
    }
}