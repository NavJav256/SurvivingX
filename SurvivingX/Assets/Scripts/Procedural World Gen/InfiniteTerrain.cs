using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
	const float viewerMoveThresholdForChunkUpdate = 25f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public LODInfo[] detailLevels;
	public static float maxViewDst;

	public Transform viewer;
	public Material mapMaterial;

	public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	static MapGen mapGenerator;
	int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, Chunk> chunkDictionary = new Dictionary<Vector2, Chunk>();
	static List<Chunk> chunksVisibleLastUpdate = new List<Chunk>();

	void Start() 
	{
		mapGenerator = GetComponent<MapGen>();

		maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
		chunkSize = MapGen.mapChunkSize - 1;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

		updateVisibleChunks();
	}

	void Update() 
	{
		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z);

		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) 
		{
			viewerPositionOld = viewerPosition;
			updateVisibleChunks();
		}
	}
		
	void updateVisibleChunks() 
	{

		for (int i = 0; i < chunksVisibleLastUpdate.Count; i++) 
		{
			chunksVisibleLastUpdate[i].setVisible(false);
		}
		chunksVisibleLastUpdate.Clear();
			
		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) 
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++) 
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (chunkDictionary.ContainsKey(viewedChunkCoord)) chunkDictionary[viewedChunkCoord].updateChunk();
				else chunkDictionary.Add(viewedChunkCoord, new Chunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
			}
		}
	}

	public class Chunk 
	{
		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		MeshCollider meshCollider;

		LODInfo[] detailLevels;
		LODMesh[] lodMeshes;
		LODMesh collisionLODMesh;

		MapData mapData;
		bool mapDataReceived;
		int previousLODIndex = -1;

		public Chunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material) 
		{
			this.detailLevels = detailLevels;

			position = coord * size;
			bounds = new Bounds(position,Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = new GameObject("Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshCollider = meshObject.AddComponent<MeshCollider>();
			meshRenderer.material = material;

			meshObject.transform.position = positionV3;
			meshObject.transform.parent = parent;
			setVisible(false);

			lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++) 
			{
				lodMeshes[i] = new LODMesh(detailLevels[i].lod, updateChunk);
				if (detailLevels[i].useForCollider) collisionLODMesh = lodMeshes[i];
			}

			mapGenerator.requestMapData(position,onMapDataReceived);
		}

		void onMapDataReceived(MapData mapData) 
		{
			this.mapData = mapData;
			mapDataReceived = true;

			updateChunk();
		}

	

		public void updateChunk() 
		{
			if (mapDataReceived)
			{
				float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				if (visible) 
				{
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++) 
					{
						if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold) lodIndex = i + 1;
						else break;
					}

					if (lodIndex != previousLODIndex) 
					{
						LODMesh lodMesh = lodMeshes[lodIndex];
						if (lodMesh.hasMesh)
						{
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						} else if (!lodMesh.hasRequestedMesh) lodMesh.requestMesh(mapData);
					}

					if(lodIndex == 0)
                    {
						if (collisionLODMesh.hasMesh) meshCollider.sharedMesh = collisionLODMesh.mesh;
						else if (!collisionLODMesh.hasRequestedMesh) collisionLODMesh.requestMesh(mapData);
                    }

					chunksVisibleLastUpdate.Add(this);

				}

				setVisible (visible);
			}
		}

		public void setVisible(bool visible) 
		{
			meshObject.SetActive(visible);
		}

		public bool isVisible() {
			return meshObject.activeSelf;
		}

	}

	class LODMesh 
	{
		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		int lod;
		System.Action updateCallback;

		public LODMesh(int lod, System.Action updateCallback)
		{
			this.lod = lod;
			this.updateCallback = updateCallback;
		}

		void onMeshDataReceived(MeshData meshData)
		{
			mesh = meshData.createMesh();
			hasMesh = true;

			updateCallback();
		}

		public void requestMesh(MapData mapData)
		{
			hasRequestedMesh = true;
			mapGenerator.requestMeshData(mapData, lod, onMeshDataReceived);
		}

	}

	[System.Serializable]
	public struct LODInfo
	{
		public int lod;
		public float visibleDstThreshold;
		public bool useForCollider;
	}

}
