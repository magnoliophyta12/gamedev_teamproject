using UnityEditor;
using UnityEngine;

using System.Collections;

public class ItemSpawnScript : MonoBehaviour
{
    public Terrain terrain;
    public GameObject itemPrefab;
    public int numberOfItems = 100;
    public float itemHeightOffset = 0.5f;
    public float maxSlopeAngle = 100.0f;
    public int itemsPerBatch = 10; 
    public float pauseBetweenBatches = 0.25f;

    void Start()
    {
        StartCoroutine(PlaceItemsInBatches());
    }

    IEnumerator PlaceItemsInBatches()
    {
        if (!terrain || !itemPrefab)
        {
            Debug.LogWarning("Terrain or Item Prefab not assigned!");
            yield break;
        }

        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        GameObject parent = new GameObject("ItemContainer");

        int placed = 0;
        int attempts = 0;
        int maxAttempts = numberOfItems * 10;

        while (placed < numberOfItems && attempts < maxAttempts)
        {
            int batchPlaced = 0;

            while (batchPlaced < itemsPerBatch && placed < numberOfItems && attempts < maxAttempts)
            {
                float x = Random.Range(0, terrainData.size.x);
                float z = Random.Range(0, terrainData.size.z);
                float y = terrain.SampleHeight(new Vector3(x, 0, z));
                Vector3 spawnPosition = new Vector3(x, y + itemHeightOffset, z) + terrainPos;

                Vector3 normal = terrainData.GetInterpolatedNormal(x / terrainData.size.x, z / terrainData.size.z);
                float slopeAngle = Vector3.Angle(normal, Vector3.up);

                if (slopeAngle <= maxSlopeAngle)
                {
#if UNITY_EDITOR
                    GameObject go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(itemPrefab);
#else
                    GameObject go = Instantiate(itemPrefab);
#endif
                    go.transform.position = spawnPosition;
                    go.transform.parent = parent.transform;

                    placed++;
                    batchPlaced++;
                }

                attempts++;
            }

            yield return new WaitForSeconds(pauseBetweenBatches);
        }

        Debug.Log($" Spawn complete: {placed}/{numberOfItems} items placed.");
    }
}