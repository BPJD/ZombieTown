using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_MobSpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyBoss;
    Transform tr;

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        StartCoroutine(Spawn());
        StartCoroutine(BossSpawn());

    }

    IEnumerator Spawn()
    {
        while (true)
        {
            int _random = Random.Range(0, spawnPoints.Length);
            Instantiate(enemy, spawnPoints[_random].position, tr.rotation);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator BossSpawn()
    {
        while (true)
        {
            int _random = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyBoss, spawnPoints[_random].position, tr.rotation);
            yield return new WaitForSeconds(15f);
        }
    }
}
