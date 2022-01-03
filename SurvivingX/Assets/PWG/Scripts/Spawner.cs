using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform player;
    public int spawnAmount;
    public int spawnLimit;
    public float rate;
    public float spawnThreshold;

    float spawnTimer;

    Vector2 spawnerPosition
    {
        get
        {
            return new Vector2(this.transform.position.x, this.transform.position.z);
        }
    }

    Vector2 playerPosition
    {
        get
        {
            return new Vector2(player.position.x, player.position.z);
        }
    }

    void Start()
    {
        spawnTimer = rate;
    }

    void Update()
    {
        float distance = Vector2.Distance(playerPosition, spawnerPosition);
        if (distance <= spawnThreshold)
        {
            if (transform.childCount < spawnLimit)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0f) Spawn();
            }
        }
        Remove();
    }

    private void Spawn()
    {
        Vector3 spawnPos = new Vector3(Random.Range(this.transform.position.x-15f, this.transform.position.x+15f), 5, Random.Range(this.transform.position.z-15f, this.transform.position.z+15f));
        Instantiate(prefab, spawnPos, Quaternion.identity, this.transform);
    }

    private void Remove()
    {
        foreach (Transform child in transform)
        {
            if (child.transform.position.y <= -5f) Destroy(child.gameObject);
        }
    }
}
