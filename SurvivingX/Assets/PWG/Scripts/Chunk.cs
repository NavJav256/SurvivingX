using UnityEngine;
using UnityEngine.AI;

public class Chunk 
{
	const float colliderGenerationDistanceThreshold = 5;
	public event System.Action<Chunk, bool> onVisibilityChanged;
	public Vector2 coord;
	 
	GameObject meshObject;
	Vector2 sampleCentre;
	Bounds bounds;

	GameObject sampleCube;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider meshCollider;
    public NavMeshSurface navMesh;

	LODInfo[] detailLevels;
	LODMesh[] lodMeshes;
	int colliderLODIndex;

	HeightMap heightMap;
	bool heightMapReceived;
	int previousLODIndex = -1;
	bool hasSetCollider;
	float maxViewDst;

	HeightMapSettings heightMapSettings;
	MeshSettings meshSettings;
	TextureData textureData;
	Transform viewer;

	AreaBake areaBake;

	public Chunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, TextureData textureData, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material, Enemies[] enemies, Vegetation[] vegetation) 
	{
		this.coord = coord;
		this.detailLevels = detailLevels;
		this.colliderLODIndex = colliderLODIndex;
		this.heightMapSettings = heightMapSettings;
		this.meshSettings = meshSettings;
		this.textureData = textureData;
		this.viewer = viewer;

		sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
		Vector2 position = coord * meshSettings.meshWorldSize ;
		bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);

		meshObject = new GameObject("Chunk");
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		navMesh = meshObject.AddComponent<NavMeshSurface>();
		navMesh.collectObjects = CollectObjects.Children;
		areaBake = meshObject.AddComponent<AreaBake>();
		areaBake.surface = navMesh;
		areaBake.player = viewer;
		meshRenderer.material = material;

		navMesh.BuildNavMesh();

		System.Random rand = new System.Random();
		int enemyIndex = rand.Next(enemies.Length);

		sampleCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		sampleCube.AddComponent<Spawner>();
		sampleCube.transform.position = new Vector3(0, -1, 0);
		sampleCube.transform.parent = meshObject.transform;
		var slimeSpawner = sampleCube.GetComponent<Spawner>();
		slimeSpawner.player = viewer;
		slimeSpawner.prefab = enemies[enemyIndex].prefab;
		slimeSpawner.spawnAmount = 1;
		slimeSpawner.spawnLimit = enemies[enemyIndex].count;
		slimeSpawner.rate = StateController.enemySpawnRate;
		slimeSpawner.spawnThreshold = 25f; //chunk limit 24.5

        //Add Vegetation
        for (int i = 0; i < vegetation.Length; i++)
        {
			CreateVegetation(vegetation[i].prefab, vegetation[i].count, meshObject.transform);
        }
		

		meshObject.transform.position = new Vector3(position.x, 0, position.y);
		meshObject.transform.parent = parent;
		SetVisible(false);

		lodMeshes = new LODMesh[detailLevels.Length];
		for (int i = 0; i < detailLevels.Length; i++) 
		{
			lodMeshes[i] = new LODMesh(detailLevels[i].lod);
			lodMeshes[i].updateCallback += UpdateTerrainChunk;

			if (i == colliderLODIndex) lodMeshes[i].updateCallback += UpdateCollisionMesh;
		}
		maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
	}

	public void Load()
	{
		ThreadedDataRequester.RequestData(() => HeightMapGen.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived);
	}



	void OnHeightMapReceived(object heightMapObject) 
	{
		this.heightMap = (HeightMap) heightMapObject;
		heightMapReceived = true;
		UpdateTerrainChunk();
	}

	Vector2 viewerPosition 
	{
		get {
			return new Vector2(viewer.position.x, viewer.position.z);
		}
	}


	public void UpdateTerrainChunk() 
	{
		if (heightMapReceived) 
		{
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

			bool wasVisible = IsVisible();
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
					} 
					else if (!lodMesh.hasRequestedMesh) lodMesh.RequestMesh(heightMap, meshSettings);
				}


			}

			if (wasVisible != visible) 
			{				
				SetVisible(visible);
				if (onVisibilityChanged != null) onVisibilityChanged(this, visible);
			}
		}
	}

	public void UpdateCollisionMesh() 
	{
		if (!hasSetCollider)
		{
			float sqrDstFromViewerToEdge = bounds.SqrDistance(viewerPosition);

			if (sqrDstFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold) 
			{
				if (!lodMeshes[colliderLODIndex].hasRequestedMesh) lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
			}

			if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) 
			{
				if (lodMeshes[colliderLODIndex].hasMesh) 
				{
					meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
					hasSetCollider = true;
				}
			}
		}
	}

	private void CreateVegetation(GameObject prefab, int numberOfVegetation, Transform parent)
    {
		float startHeight = textureData.layers[1].startHeight;
		float endHeight = textureData.layers[2].startHeight;
		for (int i = 0; i < numberOfVegetation; i++)
		{
			var pos = new Vector3(Random.Range(-24.5f, 24.5f), Random.Range(0.02f, 0.1f), Random.Range(-24.5f, 24.5f));
			Object.Instantiate(prefab, pos, Quaternion.identity, parent);
		}
	}

	public void SetVisible(bool visible) 
	{
		meshObject.SetActive(visible);
	}

	public bool IsVisible() 
	{
		return meshObject.activeSelf;
	}

}

class LODMesh 
{
	public Mesh mesh;
	public bool hasRequestedMesh;
	public bool hasMesh;
	int lod;
	public event System.Action updateCallback;

	public LODMesh(int lod) 
	{
		this.lod = lod;
	}

	void OnMeshDataReceived(object meshDataObject) 
	{
		mesh = ((MeshData)meshDataObject).CreateMesh();
		hasMesh = true;
		updateCallback();
	}

	public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings) 
	{
		hasRequestedMesh = true;
		ThreadedDataRequester.RequestData(() => MeshGen.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);
	}

}