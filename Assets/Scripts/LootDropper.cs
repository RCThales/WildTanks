using UnityEngine;

// Add to any enemy. Assign a LootTable in the Inspector.
public class LootDropper : MonoBehaviour
{
    [SerializeField] private LootTable lootTable;

    public void Drop(Vector2 position)
    {
        lootTable?.Drop(position);
    }
}
