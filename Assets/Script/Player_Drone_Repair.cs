using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drone_Repair : MonoBehaviour
{
    Unit_Building_Status buildingStat;

    public bool isRepairActive = false;
    WaitForSeconds repairDelay = new WaitForSeconds(0.2f);
    public int repairPerSec = 5;
    public int repairMaxHp = 500;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Repair());
    }

    IEnumerator Repair()
    {
        while (true)
        {
            if (isRepairActive && buildingStat != null)
            {
                if(buildingStat.hp < repairMaxHp)
                {
                    buildingStat.hp += (int)(repairPerSec * 0.2f);
                    if(buildingStat.hp > repairMaxHp)
                    {
                        buildingStat.hp = repairMaxHp;
                    }
                }
            }
            else
            {
                buildingStat = null;
            }
            yield return repairDelay;
        }
    }

    public void GetBuildingStat(GameObject _building)
    {
        buildingStat = _building.GetComponentInParent<Unit_Building_Status>();
    }
}