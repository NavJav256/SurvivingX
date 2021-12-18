using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
    public Transform viewer;
    const float maxViewDistance = 300;

    static MapGen mapGen;
    public Material mapMat;

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksSeen;

    Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
    List<Chunk> chunksLastVisible = new List<Chunk>();

    void Start()
    {
        chunkSize = MapGen.mapChunkSize - 1;
        chunksSeen = Mathf.RoundToInt(maxViewDistance / chunkSize);
        mapGen = GetComponent<MapGen>();
    }

    void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        updateVisibleChunks();
    }

    void updateVisibleChunks()
    {

        for(int i=0; i<chunksLastVisible.Count; i++)
        {
            chunksLastVisible[i].setVisible(false);
        }

        chunksLastVisible.Clear();

        int currentChunkX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunksSeen; yOffset <= chunksSeen; yOffset++)
        {
            for(int xOffset = -chunksSeen; xOffset <= chunksSeen; xOffset++)
            {
                Vector2 chunkCoord = new Vector2(currentChunkX + xOffset, currentChunkY + yOffset);

                if(chunkDictionary.ContainsKey(chunkCoord))
                {
                    chunkDictionary[chunkCoord].updateChunk();
                    if (chunkDictionary[chunkCoord].isVisible()) chunksLastVisible.Add(chunkDictionary[chunkCoord]);
                } else
                {
                    chunkDictionary.Add(chunkCoord, new Chunk(chunkCoord, chunkSize, transform, mapMat));
                }
            }
        }
    }

    public class Chunk
    {

        Vector2 position;
        GameObject mesh;
        Bounds bounds;

        MeshRenderer renderer;
        MeshFilter filter;

        public Chunk(Vector2 coord, int size, Transform parent, Material mat)
        {
            position = coord * size;
            Vector3 position3 = new Vector3(position.x, 0, position.y);
            bounds = new Bounds(position, Vector2.one * size);

            mesh = new GameObject("Chunk");
            renderer = mesh.AddComponent<MeshRenderer>();
            filter = mesh.AddComponent<MeshFilter>();
            renderer.material = mat;
            mesh.transform.position = position3;
            mesh.transform.parent = parent;

            setVisible(false);

            mapGen.requestMapData(onMapDataReceived);
        }

        private void onMapDataReceived(MapData mapData)
        {
            mapGen.requestMeshData(mapData, onMeshDataReceived);
        }

        private void onMeshDataReceived(MeshData meshData)
        {
            filter.mesh = meshData.createMesh();
        }

        public void updateChunk()
        {
            float distanceFromEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = distanceFromEdge <= maxViewDistance;
            setVisible(visible);
        }

        public void setVisible(bool v)
        {
            mesh.SetActive(v);
        }

        public bool isVisible()
        {
            return mesh.activeSelf;
        }

    }

}
