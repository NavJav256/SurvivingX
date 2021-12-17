using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer renderer;
    
    public void drawMap(float [,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        Color[] colourMap = new Color[width * height];
        
        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, map[x,y]);
            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        renderer.sharedMaterial.mainTexture = texture;
        renderer.transform.localScale = new Vector3(width, 1, height);
    }
}
