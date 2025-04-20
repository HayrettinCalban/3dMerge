using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject[] mergePrefabs;

    private GameObject currentBall;
    private float spawnDelay = 1f;
    private float spawnTimer = 0f;

    void Start()
    {
        SpawnRandomBall();
    }

    void Update()
    {
        if (currentBall == null)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDelay)
            {
                SpawnRandomBall();
                spawnTimer = 0f;
            }
        }
    }

    public void SpawnRandomBall()
    {
        if (currentBall != null) return;
        if (mergePrefabs == null || mergePrefabs.Length == 0) return;

        int maxLevel = 0;
        BallMerge[] allBalls = FindObjectsByType<BallMerge>(FindObjectsSortMode.None);
        foreach (var ball in allBalls)
        {
            if (ball.prefabIndex > maxLevel)
                maxLevel = ball.prefabIndex;
        }
        int maxSpawnLevel = Mathf.Min(maxLevel, mergePrefabs.Length - 1);
        int rareLevel = maxSpawnLevel - 1;
        if (rareLevel < 0) rareLevel = 0;
        System.Collections.Generic.List<int> weightedLevels = new System.Collections.Generic.List<int>();
        for (int i = 0; i < maxSpawnLevel; i++)
        {
            int weight = (i == rareLevel) ? 1 : 6;
            for (int w = 0; w < weight; w++)
                weightedLevels.Add(i);
        }
        if (weightedLevels.Count == 0)
            weightedLevels.Add(0);
        int randomIndex = Random.Range(0, weightedLevels.Count);
        int prefabIndexToSpawn = weightedLevels[randomIndex];
        GameObject prefabToSpawn = mergePrefabs[prefabIndexToSpawn];

        currentBall = Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        var merge = currentBall.GetComponent<BallMerge>();
        if (merge != null)
        {
            merge.mergePrefabs = mergePrefabs;
            merge.prefabIndex = prefabIndexToSpawn;
        }
        var controller = currentBall.GetComponent<BallController>();
        if (controller != null) controller.enabled = true;
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        if (rb != null) rb.useGravity = false;
    }

    public void OnBallLeftSpawn()
    {
        if (currentBall != null)
        {
            var controller = currentBall.GetComponent<BallController>();
            if (controller != null) controller.enabled = false;
            currentBall = null;
        }
        spawnTimer = 0f;
    }
}
