using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S4Controller : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public float minEnemyDelay = 4f;
    public float maxEnemyDelay = 5f;

    public float timeElapsed;
    public float enemyDelay;

       // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EnemySpawnTimer");
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        float decraeseDelayOverTime = maxEnemyDelay-(maxEnemyDelay-minEnemyDelay) / 30f * timeElapsed;
        enemyDelay = Mathf.Clamp(decraeseDelayOverTime, minEnemyDelay, maxEnemyDelay);
    }

    void SpawnEnemy()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Instantiate(randomEnemyPrefab, randomSpawnPoint.position, Quaternion.identity);
    }


    IEnumerator EnemySpawnTimer()
    {
        yield return new WaitForSeconds(enemyDelay);
        SpawnEnemy();
        StartCoroutine("EnemySpawnTimer");

    }


}
