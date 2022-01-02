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
        
    }

    void Update()
    {
        float distance = Vector2.Distance(playerPosition, spawnerPosition);
        if (distance <= spawnThreshold) Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-15f, 15f), 10, Random.Range(-15f, 15f));
            Instantiate(prefab, spawnPos, Quaternion.identity, this.transform);
        }
    }
}
