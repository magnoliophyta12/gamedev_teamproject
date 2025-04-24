using UnityEngine;

public class GoalSpawner : MonoBehaviour
{
    public GameObject chestPrefab;
    public Terrain terrain;
    public float itemHeightOffset = 0.5f;
    public float maxSlopeAngle = 0f;
    private GameObject currentChest;
    private bool chestSpawned = false;

    private Vector3 lastSpawnPos; 

    public void SpawnChest()
    {
        if (chestSpawned)
        {
            Debug.Log("[GoalSpawner] Chest already spawned.");
            return;
        }

        if (currentChest != null)
            Destroy(currentChest);

        TerrainData data = terrain.terrainData;
        Vector3 terrainPos = terrain.GetPosition();

        int attempts = 0;
        int maxAttempts = 50;

        while (attempts < maxAttempts)
        {
            float offsetX = data.size.x * 0.25f;
            float offsetZ = data.size.z * 0.25f;

            float randX = Random.Range(offsetX, data.size.x - offsetX);
            float randZ = Random.Range(offsetZ, data.size.z - offsetZ);
            float y = terrain.SampleHeight(new Vector3(randX, 0, randZ)) + terrainPos.y;

            Vector3 normal = data.GetInterpolatedNormal(randX / data.size.x, randZ / data.size.z);
            float slopeAngle = Vector3.Angle(normal, Vector3.up);

            if (slopeAngle <= maxSlopeAngle)
            {
                Vector3 spawnPos = new Vector3(randX, y + itemHeightOffset, randZ) + terrainPos;
                lastSpawnPos = spawnPos; 

                currentChest = Instantiate(chestPrefab, spawnPos, Quaternion.identity);
                SetLayerRecursively(currentChest, LayerMask.NameToLayer("MapLayer"));
                chestSpawned = true;

                Debug.Log($"[GoalSpawner] Spawned chest at world position: {spawnPos}");
                return;
            }

            attempts++;
        }
    }

    public void ResetChestSpawn()
    {
        chestSpawned = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && chestSpawned)
            {
                player.transform.position = lastSpawnPos;
                Debug.Log("[GoalSpawner] Teleported player to chest position.");
            }
            else
            {
                Debug.LogWarning("[GoalSpawner] Chest not spawned or Player not found.");
            }
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}