using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyBoss;
    public GameObject subBoss;
    Transform tr;
    public float day_spawn_per_second = 5f;
    public float night_spawn_per_second = 2f;
    float decresedTime;
    float waitTime;
    public bool nightCardActivated = false;

    public bool mob_spawnAble = true;

    GameManage manage;

    public Transform[] spawnPoints;

    GameObject sun;
    LightRotate sun_system;

    // Start is called before the first frame update
    void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
        sun_system = sun.GetComponent<LightRotate>();
        tr = GetComponent<Transform>();
        manage = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManage>();
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        
        while (manage.isGameOver == false)
        {
            int _random = Random.Range(0, spawnPoints.Length);
            if (mob_spawnAble)
            {
                Instantiate(enemy, spawnPoints[_random].position, tr.rotation);
            }
            
            
            if (sun_system.isDay)
            {
                waitTime = day_spawn_per_second;
            }
            else
            {
                waitTime = night_spawn_per_second;
            }
            
            yield return new WaitForSeconds(waitTime);
        }
        
        yield return new WaitForSeconds(waitTime);
    }

    public void BossSpawn()
    {

        if(sun_system.dayCount == 21)
        {
            int _random = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyBoss, spawnPoints[_random].position, tr.rotation);
        }
        else
        {
            int _random = Random.Range(0, spawnPoints.Length);
            Instantiate(subBoss, spawnPoints[_random].position, tr.rotation);
        }
        
    }

    public void SpawnFaster()
    {
        night_spawn_per_second -= 0.5f;
        day_spawn_per_second -= 1f;
    }

    public void ExpCardActivated(float _ref, bool _isTrue)
    {
        if (_isTrue)
        {
            decresedTime = night_spawn_per_second * (_ref * 1f);
            night_spawn_per_second = decresedTime;
            nightCardActivated = _isTrue;
        }
        else
        {
            night_spawn_per_second += decresedTime;
            nightCardActivated = _isTrue;
        }
        
    }
}
