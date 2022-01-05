using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{

    public NavMeshSurface[] surfaces;
    public bool isBaked;
    public Transform[] objectsToRotate;

    int totalCount;
    int activeCount;

    bool counted;
    bool done;

    int count;

    public NavigationBaker()
    {

    }

    // Use this for initialization
    void Start()
    {
        count = 0;
        isBaked = false;
        done = false;
    }

    private void Update()
    {

        if (!counted) GetSurfaces();
        if (!isBaked) Bake();
    }

    private void GetSurfaces()
    {
        StartCoroutine(Count());
        Debug.Log(totalCount);
        Debug.Log(activeCount);
        surfaces = new NavMeshSurface[totalCount];
        for (int i = 0; i < totalCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            Debug.Log(child.gameObject.GetComponent<NavMeshSurface>());
            surfaces[i] = child.gameObject.GetComponent<NavMeshSurface>();
        }
        counted = true;
    }
    IEnumerator Count()
    {
        totalCount = this.transform.childCount;
        yield return new WaitForSeconds(10);
    }

    private void Bake()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
        isBaked = true;
    }

}