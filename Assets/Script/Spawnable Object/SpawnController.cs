using System.Collections.Generic;
using UnityEngine;
using static PoolEnum;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private DespawnManager despawnManager;
    [SerializeField] private float spawnDistance = 250f;
    [SerializeField] private float spawnSpread = 50f;
    [SerializeField] private float spawnInterval = 1.5f;

    private List<SpawnModel> spawnables;
    private float totalWeight;

    private void Start()
    {
        spawnables = new List<SpawnModel>()
        {
            new SpawnModel(PoolTag.NonDestructibleMetroid, 50),
            new SpawnModel(PoolTag.DestructibleMetroid, 30),
            new SpawnModel(PoolTag.ShieldOrb, 10),
            new SpawnModel(PoolTag.ThrusterOrb, 10),
            new SpawnModel(PoolTag.SolarOrb, 10)
        };

        totalWeight = 0f;
        foreach (var item in spawnables)
            totalWeight += item.SpawnWeight;

        InvokeRepeating(nameof(Spawn), 1f, spawnInterval);
    }

    private void Spawn()
    {
        Vector3 forwardDir = player.forward.normalized;

        Vector3 spawnPos = player.position + forwardDir * spawnDistance;
        spawnPos += new Vector3(
            Random.Range(-spawnSpread, spawnSpread),
            Random.Range(-spawnSpread, spawnSpread),
            Random.Range(-spawnSpread, spawnSpread)
        );

        PoolTag selectedTag = GetWeightedRandomTag();

       GameObject obj =  PoolManager.Instance.SpawnFromPool(selectedTag, spawnPos, Quaternion.identity);

        if (obj != null)
        {
           // obj.transform.LookAt(player);
            despawnManager.TrackObject(obj);
        }
        else
        {
            Debug.LogWarning("Failed to spawn object from pool: " + selectedTag);
        }
    }

    private PoolTag GetWeightedRandomTag()
    {
        if (spawnables == null || spawnables.Count == 0 || totalWeight <= 0f)
        {
            Debug.LogWarning("Spawnables list is empty.");
            return PoolTag.NonDestructibleMetroid;  // Safe fallback
        }

        float randomWeight = Random.Range(0, totalWeight);
        float runningTotal = 0f;

        foreach (var obj in spawnables)
        {
            runningTotal += obj.SpawnWeight;
            if (randomWeight <= runningTotal)
                return obj.PoolTag;
        }

        return spawnables[0].PoolTag; // Fallback if the loop doesn't return a object //check
    }
}

