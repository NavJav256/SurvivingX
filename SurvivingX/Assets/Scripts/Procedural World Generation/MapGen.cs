using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public enum DrawMode { noise, colour, mesh};

    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool update;

    public TerrainType[] regions;

    public void generateMap()
    {
        float[,] map = Noise.createMap(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset);
        Color[] cMap = new Color[mapWidth * mapHeight];

        for(int y=0; y<mapHeight; y++)
        {
            for(int x=0; x<mapWidth; x++)
            {
                float currentHeight = map[x, y];
                for(int i=0; i<regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        cMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = GetComponent<MapDisplay>();

        if (drawMode == DrawMode.noise) display.drawMap(TextureGen.createTextureFromHeight(map));
        else if (drawMode == DrawMode.colour) display.drawMap(TextureGen.createTextureFromColour(cMap, mapWidth, mapHeight));
        else if (drawMode == DrawMode.mesh) display.drawMesh(MeshGen.createTerrainMesh(map), TextureGen.createTextureFromColour(cMap, mapWidth, mapHeight));
    }

    public void OnValidate()
    {
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;

    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

}