using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombieBurning : MonoBehaviour
{
    Unit_Status unitStat;
    WaitForSeconds delay = new WaitForSeconds(1f);
    LightRotate sun;
    bool isBurned = false;
    string sunString = "Sun";
    void Start()
    {
        sun = GameObject.FindGameObjectWithTag(sunString).GetComponent<LightRotate>();
        unitStat = GetComponent<Unit_Status>();
        if (!sun.isDay)
        {
            StartCoroutine(BurnCheck());
        }
    }

    IEnumerator BurnCheck()
    {
        while (true)
        {
            if (sun.isDay && !isBurned)
            {
                isBurned = true;
                unitStat.hp = (int)(unitStat.hp * 0.3f);
            }
            yield return delay;
        }
    }
}
