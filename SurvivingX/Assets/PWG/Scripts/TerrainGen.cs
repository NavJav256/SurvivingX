using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{
	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public int colliderLODIndex;
	public LODInfo[] detailLevels;

	public MeshSettings meshSettings;
	public HeightMapSettings heightMapSettings;
	public TextureData textureSettings;

	public Transform viewer;
	public Material mapMaterial;

	public Enemies[] enemies;
	public Vegetation[] vegetation;

	Vector2 viewerPosition;
	Vector2 viewerPositionOld;

	float meshWorldSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, Chunk> terrainChunkDictionary = new Dictionary<Vector2, Chunk>();
	List<Chunk> visibleTerrainChunks = new List<Chunk>();


	void Start() 
	{
		textureSettings.ApplyToMaterial(mapMaterial);
		textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);

		float maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
		meshWorldSize = meshSettings.meshWorldSize;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / meshWorldSize);

		UpdateVisibleChunks();
	}

	void Update() 
	{
		viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

		if (viewerPosition != viewerPositionOld) 
		{
			foreach (Chunk chunk in visibleTerrainChunks) 
			{
				chunk.UpdateCollisionMesh();
			}
		}

		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) 
		{
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks();
		}
	}
		
	void UpdateVisibleChunks() 
	{
		HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
		for (int i = visibleTerrainChunks.Count - 1; i >= 0; i--) 
		{
			alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
			visibleTerrainChunks[i].UpdateTerrainChunk();
		}
			
		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / meshWorldSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / meshWorldSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) 
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) 
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord)) 
				{
					if (terrainChunkDictionary.ContainsKey(viewedChunkCoord)) terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
					else {
						Chunk newChunk = new Chunk(viewedChunkCoord, heightMapSettings, meshSettings, textureSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial, enemies, vegetation);
						terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
						newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
						newChunk.Load();
					}
				}

			}
		}
	}

	void OnTerrainChunkVisibilityChanged(Chunk chunk, bool isVisible) 
	{
		if (isVisible) visibleTerrainChunks.Add(chunk);
		else visibleTerrainChunks.Remove(chunk);
	}

}

[System.Serializable]
public struct LODInfo 
{
	[Range(0,MeshSettings.numSupportedLODs-1)]
	public int lod;
	public float visibleDstThreshold;


	public float sqrVisibleDstThreshold
	{
		get {
			return visibleDstThreshold * visibleDstThreshold;
		}
	}
}

[System.Serializable]
public struct Enemies
{
	public GameObject prefab;
	public int count;
}

[System.Serializable]
public struct Vegetation
{
	public GameObject prefab;
	public int count;
}
