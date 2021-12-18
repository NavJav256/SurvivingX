using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter filter;
    public MeshRenderer meshRenderer;

    
    public void drawMap(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void drawMesh(MeshData meshData, Texture2D texture)
    {
        filter.sharedMesh = meshData.createMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
