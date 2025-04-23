using UnityEditor;
using UnityEngine;

public class ItemSpawnScript : MonoBehaviour
{
    public Terrain terrain;
    public GameObject itemPrefab;
    public int numberOfItems = 20;
    public float itemHeightOffset = 0.5f;
    public float maxSlopeAngle = 100.0f;

    void Start()
    {
        PlacePositions();
    }
    void PlacePositions()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        GameObject parent = new GameObject();

        int placed = 0;
        int attempts = 0;
        int maxAttempts = numberOfItems * 10;

        while (placed < numberOfItems && attempts < maxAttempts)
        {
            float x = Random.Range(0, terrainData.size.x);
            float z = Random.Range(0, terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z));
            Vector3 spawnPosition = new Vector3(x, y, z) + terrainPos;

            Vector3 normal = terrainData.GetInterpolatedNormal(x / terrainData.size.x, z / terrainData.size.z);
            float slopeAngle = Vector3.Angle(normal, Vector3.up);

            if (slopeAngle <= maxSlopeAngle)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(itemPrefab);
                go.transform.position = spawnPosition;
                go.transform.parent = parent.transform;

                placed++;
            }

            attempts++;
        }
    }
}