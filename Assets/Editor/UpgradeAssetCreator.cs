using UnityEngine;
using UnityEditor;

public class UpgradeAssetCreator
{
    [MenuItem("Tools/Create Upgrade Assets")]
    public static void CreateUpgradeAssets()
    {
        // Find the PlasticBullet prefab
        string[] guids = AssetDatabase.FindAssets("PlasticBullet t:Prefab");
        Bullet plasticBulletPrefab = null;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go != null && go.GetComponent<PlasticBullet>() != null)
            {
                plasticBulletPrefab = go.GetComponent<Bullet>();
                break;
            }
        }

        // Find the TriBullet prefab
        guids = AssetDatabase.FindAssets("TriBullet t:Prefab");
        Bullet triBulletPrefab = null;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go != null && go.GetComponent<TriBullet>() != null)
            {
                triBulletPrefab = go.GetComponent<Bullet>();
                break;
            }
        }

        CreateBulletUpgrade("Upgrade_PlasticBullet", "Plastic Bullet", plasticBulletPrefab);
        CreateBulletUpgrade("Upgrade_TriBullet", "Tri Bullet", triBulletPrefab);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Upgrade assets created in Assets/Upgrades/");
        if (plasticBulletPrefab == null) Debug.LogWarning("PlasticBullet prefab not found — assign it manually.");
        if (triBulletPrefab == null)     Debug.LogWarning("TriBullet prefab not found — assign it manually.");
    }

    private static void CreateBulletUpgrade(string filename, string displayName, Bullet prefab)
    {
        string path = $"Assets/Upgrades/{filename}.asset";
        UpgradeData existing = AssetDatabase.LoadAssetAtPath<UpgradeData>(path);
        if (existing != null)
        {
            Debug.Log($"{filename} already exists, skipping.");
            return;
        }

        UpgradeData data = ScriptableObject.CreateInstance<UpgradeData>();
        data.upgradeName = displayName;
        data.type = UpgradeType.BulletSlot;
        data.bulletPrefab = prefab;

        AssetDatabase.CreateAsset(data, path);
    }
}
