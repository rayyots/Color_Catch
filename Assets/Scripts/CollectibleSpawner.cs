using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;  // Array of collectible prefabs (red, blue, yellow)
    public Vector3 spawnAreaSize = new Vector3(10f, 1f, 10f);  // Size of the spawning area
    public float spawnInterval = 2f;  // Time between each spawn

    void Start()
    {
        StartSpawning();  // Start spawning when the game begins
    }

    public void StartSpawning()
    {
        InvokeRepeating("SpawnCollectible", 0f, spawnInterval);  // Start spawning collectibles
    }

    public void StopSpawning()
    {
        CancelInvoke("SpawnCollectible");  // Stop spawning collectibles
    }

    void SpawnCollectible()
    {
        // Generate a random position within the spawn area
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            0.5f,  // Make sure it's slightly above the ground
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Randomly choose a collectible prefab
        GameObject collectible = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];

        // Instantiate the collectible at the random position
        Instantiate(collectible, spawnPosition, Quaternion.identity);
    }
}
