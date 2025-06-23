using System.Collections.Generic;
using UnityEngine;
using static PoolEnum;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public PoolTag tag;   
        public GameObject prefab;
        public int size;
        public bool randomRotation;
    }

    [SerializeField] private List<Pool> pools;
    private Dictionary<PoolTag, Queue<GameObject>> poolDictionary;
    private Dictionary<PoolTag, bool> rotation = new Dictionary<PoolTag, bool>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        poolDictionary = new Dictionary<PoolTag, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
            rotation.Add(pool.tag, pool.randomRotation);
        }
    }

    public GameObject SpawnFromPool(PoolTag tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        if (this.rotation[tag])
        {
           
            float angle = Random.Range(0f, 360f);
            objectToSpawn.transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + angle, rotation.eulerAngles.z);
        }
        else
        {
            objectToSpawn.transform.rotation = rotation;
        }

        DestrutableMetriod metriod = objectToSpawn.GetComponent<DestrutableMetriod>(); //register // the controller to the metriod for hits 
        if (metriod != null)
        {
            metriod.SetController(Controller.Instance);
        }

        //   objectToSpawn.transform.rotation = rotation;
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;



    }
}
