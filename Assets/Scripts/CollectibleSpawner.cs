using UnityEngine;

public class CollectibleSpawner : MonoBehaviour {
    [SerializeField] private GameObject collectiblePrefab;
    [SerializeField] private int collectibleCount = 10;
    [SerializeField] private Vector3 spawnAreaMin;
    [SerializeField] private Vector3 spawnAreaMax;

    void Start() {
        SpawnCollectibles();
    }

    private void SpawnCollectibles() {
        int spawned = 0;

        while (spawned < collectibleCount) {
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            Collider[] colliders = Physics.OverlapSphere(randomPosition, 1f);
            if (colliders.Length == 0) {
                Instantiate(collectiblePrefab, randomPosition, Quaternion.identity);
                spawned++;
            }
        }
    }

}
