using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

public class AreaBake : MonoBehaviour
{
    public NavMeshSurface surface;
    public Transform player;
    private float updateRate = 0.05f;
    private float movementThreshold = 24.5f;
    private Vector3 navMeshSize = new Vector3(150, 10, 150);
    private Vector3 worldAnchor;
    private NavMeshData navMeshData;
    private List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

    private void Start()
    {
        navMeshData = new NavMeshData();
        NavMesh.AddNavMeshData(navMeshData);
        BuildNavMesh(false);
        StartCoroutine(CheckPlayerMovement());
    }

    private IEnumerator CheckPlayerMovement()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);
        while (true)
        {
            if (Vector3.Distance(worldAnchor, player.transform.position) > movementThreshold)
            {
                BuildNavMesh(true);
                worldAnchor = player.transform.position;
            }
            yield return wait;
        }
    }

    private void BuildNavMesh(bool async)
    {
        Bounds navMeshBounds = new Bounds(player.transform.position, navMeshSize);
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
        List<NavMeshModifier> modifiers;

        if (surface.collectObjects == CollectObjects.Children) modifiers = new List<NavMeshModifier>(surface.GetComponentsInChildren<NavMeshModifier>());
        else modifiers = NavMeshModifier.activeModifiers;

        for (int i = 0; i < modifiers.Count; i++)
        {
            if (((surface.layerMask & (1 << modifiers[i].gameObject.layer)) == 1) && modifiers[i].AffectsAgentType(surface.agentTypeID))
            {
                markups.Add(new NavMeshBuildMarkup()
                {
                    root = modifiers[i].transform,
                    overrideArea = modifiers[i].overrideArea,
                    area = modifiers[i].area,
                    ignoreFromBuild = modifiers[i].ignoreFromBuild,
                });
            }
        }

        if (surface.collectObjects == CollectObjects.Children) NavMeshBuilder.CollectSources(surface.transform, surface.layerMask, surface.useGeometry, surface.defaultArea, markups, sources);
        else NavMeshBuilder.CollectSources(navMeshBounds, surface.layerMask, surface.useGeometry, surface.defaultArea, markups, sources);

        sources.RemoveAll(source => source.component != null && source.component.gameObject.GetComponent<NavMeshAgent>() != null);

        if (async) NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, surface.GetBuildSettings(), sources, new Bounds(player.transform.position, navMeshSize));
        else NavMeshBuilder.UpdateNavMeshData(navMeshData, surface.GetBuildSettings(), sources, new Bounds(player.transform.position, navMeshSize));
    }

}
