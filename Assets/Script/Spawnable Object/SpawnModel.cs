

using static PoolEnum;

public class SpawnModel 
{
    public PoolTag PoolTag { get; private set; }
    public float SpawnWeight { get; private set; }

    public SpawnModel(PoolTag poolTag, float spawnWeight)
    {
        PoolTag = poolTag;
        SpawnWeight = spawnWeight;

    }
}
