using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Loot/LootTable")]
public class LootTable : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float dropChance = 0.5f;
        public int minCount = 1;
        public int maxCount = 1;
    }

    public Entry[] entries;
    [SerializeField] private float scatterRadius = 1.5f;
    [SerializeField] private int sortingOrder = 5;

    public void Drop(Vector2 position)
    {
        foreach (var entry in entries)
        {
            if (entry.prefab == null) continue;
            if (Random.value > entry.dropChance) continue;

            int count = UnityEngine.Random.Range(entry.minCount, entry.maxCount + 1);
            for (int i = 0; i < count; i++)
            {
                Vector2 offset = UnityEngine.Random.insideUnitCircle.normalized
                    * UnityEngine.Random.Range(scatterRadius * 0.3f, scatterRadius);
                GameObject spawned = UnityEngine.Object.Instantiate(entry.prefab, position + offset, Quaternion.identity);
                foreach (var sr in spawned.GetComponentsInChildren<SpriteRenderer>())
                    sr.sortingOrder = sortingOrder;
            }
        }
    }
}
