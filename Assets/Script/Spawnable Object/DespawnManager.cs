using System.Collections.Generic;
using UnityEngine;

public class DespawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float despawnDistanceBehind = 20f;

    private List<GameObject> trackedObjects = new List<GameObject>();

    private void Update()
    {
        for (int i = trackedObjects.Count - 1; i >= 0; i--)
        {
            if (!trackedObjects[i].activeInHierarchy)
            {
                trackedObjects.RemoveAt(i);
                continue;
            }

            if (trackedObjects[i].transform.position.z < player.position.z - despawnDistanceBehind)
            {
                trackedObjects[i].SetActive(false);
                trackedObjects.RemoveAt(i);
            }
        }
    }

    public void TrackObject(GameObject obj)
    {
        trackedObjects.Add(obj);
    }
}
