using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] createMap(int width, int height, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] map = new float[width, height];

        System.Random rand = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];

        for(int i=0; i<octaves; i++)
        {
            float offsetX = rand.Next(-100000, 100000) + offset.x;
            float offsetY = rand.Next(-100000, 100000) + offset.y;
            offsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.0001f;

        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - width/2) / scale * frequency + offsets[i].x;
                    float sampleY = (y - height/2) / scale * frequency + offsets[i].y;

                    float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlin * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoise) maxNoise = noiseHeight;
                else if (noiseHeight < minNoise) minNoise = noiseHeight;

                map[x, y] = noiseHeight;
            }

        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, y] = Mathf.InverseLerp(minNoise, maxNoise, map[x, y]);
            }
        }

        return map;
    }
}
