using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Building_Status : MonoBehaviour
{
    public int hp;
    GameObject attacker;
    public GameObject particle;
    GameObject player;
    GameObject manager;
    Unit_Building_RevisionManager revisionManager;

    public int destroy_damage = 5000;
    public int farmable = 1;
    int farmCountDefault = 0;
    public float time_for_farming = 3f;
    public float time_for_repair = 1f;
    public float repairCost = 8;
    public int repairHp = 100;

    float farmingCoolDefault = 90f;
    float farmingCool;

    void Start()
    {
        farmingCool = farmingCoolDefault;
        manager = GameObject.FindGameObjectWithTag("GameController");
        revisionManager = manager.GetComponent<Unit_Building_RevisionManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        farmCountDefault = farmable;
    }

    void Update()
    {
        if(farmable == 0)
        {
            farmingCool -= Time.deltaTime;
            if(farmingCool <= 0)
            {
                farmable = farmCountDefault;
                farmingCool = farmingCoolDefault;
            }
        }
    }

    void SetAttacker(GameObject _attacker)
    {
        attacker = _attacker;
    }

    void Damaged(int dmg)
    {
        if (this.tag == "PlayerBuilding")
        {
            hp -= dmg;
            if (hp <= 0)
            {
                Instantiate(particle, transform.position, Quaternion.identity); 

                manager.SendMessage("Out_Order_Clicked", SendMessageOptions.DontRequireReceiver);

                player.SendMessage("Damaged", 50000, SendMessageOptions.DontRequireReceiver);

                Destroy(gameObject);
            }
        }
    }

    void PlayerIsIn()
    {
        this.tag = "PlayerBuilding";
    }

    void PlayerIsOut()
    {
        this.tag = "Building";
    }

    void Farmed() //건물 파밍. Building_UI에서 호출됨
    {
        farmable--;
    }

    void Repair()
    {
        hp += Mathf.RoundToInt(repairHp * revisionManager.buildingRepair);
    }


}
